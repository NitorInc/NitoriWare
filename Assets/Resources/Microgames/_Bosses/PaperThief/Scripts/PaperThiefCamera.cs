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
	private float shiftSpeed, nitoriFollowSpeed;
	[SerializeField]
	private AnimationCurve velocityOverTime;
#pragma warning restore 0649


    private BoxCollider2D boxCollider;
	private Vector3 goalPosition;
	private float goalSize, lerpSize, lerpSizeDistance, startTime;
	private bool mustShift, scroll, _followNitori;

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
		goalSize = Camera.main.orthographicSize;
		goalPosition = transform.localPosition;
		startTime = Time.time;
		scroll = true;
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
		lerpSize = Camera.main.orthographicSize;
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
	
	void Update()
	{
        if (followNitori)
            updateNitoriShiftGoal();

		if (mustShift)
			updateShift();
		else if (scroll)
			updateScroll();
	}

	void updateScroll()
	{
		transform.position += Vector3.right * velocityOverTime.Evaluate(Time.time - startTime) * Time.deltaTime;
	}

	void updateShift()
	{
		if (transform.moveTowardsLocal((Vector2)goalPosition, shiftSpeed))
		{
			Camera.main.orthographicSize = goalSize;
			mustShift = false;
		}
		else
			Camera.main.orthographicSize = Mathf.Lerp(goalSize, lerpSize, ((Vector2)(goalPosition - transform.localPosition)).magnitude / lerpSizeDistance);
		//Debug.Log(lerpSizeDistance + " and " + ((Vector2)(goalPosition - transform.localPosition)).magnitude);
		//Debug.Log("also " + lerpSize + " and " + goalSize);
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
        setGoalSize(Camera.main.orthographicSize);
    }
}
