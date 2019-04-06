using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperThiefCamera : MonoBehaviour
{
	public static PaperThiefCamera instance;

#pragma warning disable 0649
    [SerializeField]
	private Transform follow;
	[SerializeField]
	private float shiftSpeed, nitoriFollowSpeed, chaseShiftSpeed;
	[SerializeField]
	private AnimationCurve velocityOverTime;
#pragma warning restore 0649


    private BoxCollider2D boxCollider;
	private Vector3 goalPosition, chaseGoalOffset, chaseOffset;
	private float goalSize, lerpSize, lerpSizeDistance, startTime;
	private bool mustShift, scroll, _followNitori, chase;

    public bool followNitori
    {
        get { return _followNitori; }
        set
        {
            if (value)
            {
                stopScroll();
                updateNitoriShiftGoal();
            }
            _followNitori = value;
        }
    }


	void Awake()
	{
		instance = this;
		boxCollider = GetComponent<BoxCollider2D>();
		goalSize = MainCameraSingleton.instance.orthographicSize;
		goalPosition = transform.localPosition;
		startTime = Time.time;
		scroll = true;
        chase = false;
	}

	public void setGoalPosition(Vector3 goalPosition)
	{
		this.goalPosition = goalPosition;
		mustShift = true;
	}

	public void stopScroll()
	{
		scroll = false;
	}

	public void setGoalSize(float goalSize)
	{
		this.goalSize = goalSize;
		lerpSize = MainCameraSingleton.instance.orthographicSize;
		lerpSizeDistance = ((Vector2)(goalPosition - transform.localPosition)).magnitude;
		mustShift = true;
	}

	public void setFollow(Transform follow)
	{
		this.follow = follow;
	}

    public void setShiftSpeed(float shiftSpeed)
    {
        this.shiftSpeed = shiftSpeed;
    }

    public float getCurrentShiftSpeed()
    {
        return velocityOverTime.Evaluate(Time.time - startTime);
    }
	
	public void Update()
	{
        if (followNitori)
            updateNitoriShiftGoal();
        else if (chase)
            updateChase();

        if (mustShift)
			updateShift();
		else if (scroll)
			updateScroll();
	}

    void updateChase()
    {
        if (chaseOffset != chaseGoalOffset)
        {
            chaseOffset.x += chaseShiftSpeed * Time.deltaTime;
            if (chaseOffset.x >= chaseGoalOffset.x)
                chaseOffset = chaseGoalOffset;
        }

        transform.position = PaperThiefNitori.instance.transform.position + chaseOffset;
    }

	void updateScroll()
	{
		transform.position += Vector3.right * getCurrentShiftSpeed() * Time.deltaTime;
	}

	void updateShift()
	{
		if (transform.moveTowardsLocal2D((Vector2)goalPosition, shiftSpeed))
		{
			MainCameraSingleton.instance.orthographicSize = goalSize;
			mustShift = false;
		}
		else
			MainCameraSingleton.instance.orthographicSize = Mathf.Lerp(goalSize, lerpSize, ((Vector2)(goalPosition - transform.localPosition)).magnitude / lerpSizeDistance);
	}

	void updateFollow()
	{
		boxCollider.enabled = true;

		Vector3 bounds = boxCollider.bounds.extents;
		transform.position = new Vector3(
			Mathf.Clamp(transform.position.x, follow.position.x - bounds.x, follow.position.x + bounds.x),
			Mathf.Clamp(transform.position.y, follow.position.y - bounds.y, follow.position.y + bounds.y),
			transform.position.z);

		boxCollider.enabled = false;
	}

    void updateNitoriShiftGoal()
    {
        Vector3 nitoriPosition = PaperThiefNitori.instance.transform.localPosition,
            camPosition = transform.localPosition;
        setGoalPosition(new Vector3(nitoriPosition.x, camPosition.y, camPosition.z));
        setShiftSpeed(nitoriFollowSpeed);
        setGoalSize(MainCameraSingleton.instance.orthographicSize);
    }

    public void startChase()
    {
        chase = true;
        chaseOffset = chaseGoalOffset = transform.position - PaperThiefNitori.instance.transform.position;
        chaseGoalOffset = new Vector3(7.5f, chaseGoalOffset.y, chaseGoalOffset.z);

        PaperThiefController.instance.startScene(PaperThiefController.Scene.BeginChase);
    }

    public void stopChase()
    {
        chase = enabled = false;
    }
}
