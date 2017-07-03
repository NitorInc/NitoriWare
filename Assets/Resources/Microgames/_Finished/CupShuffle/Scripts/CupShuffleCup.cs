using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CupShuffleCup : MonoBehaviour
{
	public int position;
	public Vector3 leftmostPosition;
	public float cupSeparation;
	public AnimationCurve moveCurve;
	public Animator animator;
	public Collider2D clickCollider;

	[SerializeField]
	private bool _isCorrect;
	public bool isCorrect
	{
		get{return _isCorrect;}
		set { _isCorrect = value; animator.SetBool("isCorrect", value);}
	}

	[SerializeField]
	private bool _chosen;
	public bool chosen
	{
		get { return _chosen; }
		set { _chosen = value; animator.SetBool("chosen", value); }
	}

	[SerializeField]
	private CupShuffleController.State _state;
	public CupShuffleController.State state
	{
		get { return _state; }
		set { _state = value; animator.SetInteger("state", (int)value); }
	}

	private Animation currentAnimation;
	private struct Animation
	{
		public bool active, goingUp, backwards, fakeout;
		public int cupDistance;
		public float startTime, duration;
	}

	void Awake()
	{
		isCorrect = isCorrect;
		state = state;
	}

	void Start()
	{
		transform.position = leftmostPosition + (Vector3.right * (float)position * cupSeparation);
		currentAnimation.active = false;
	}

	public void startAnimation(int cupDistance, float duration, bool goingUp, bool backwards, bool fakeout)
	{
		currentAnimation.active = true;
		currentAnimation.startTime = Time.time;
		currentAnimation.cupDistance = cupDistance;
		currentAnimation.duration = duration;
		currentAnimation.goingUp = goingUp;
		currentAnimation.backwards = backwards;
		currentAnimation.fakeout = fakeout;
	}
	
	void Update ()
	{
		if (currentAnimation.active)
			updateAnimation();
		else if (state == CupShuffleController.State.Choose)
			checkForClick();
	}

	void checkForClick()
	{
		if (Input.GetMouseButtonDown(0) && CameraHelper.isMouseOver(clickCollider))
		{
			chosen = true;
			if (isCorrect)
			{
				CupShuffleController.instance.victory();
			}
			else
			{
				CupShuffleController.instance.failure();
			}
		}
	}

	void updateAnimation()
	{
		if (Time.time >= currentAnimation.startTime + currentAnimation.duration)
		{
			endAnimation();
		}
		else
		{
			lerpPosition((Time.time - currentAnimation.startTime) / currentAnimation.duration);
		}
	}

	public void endAnimation()
	{
		if (currentAnimation.active)
		{
			if (!currentAnimation.fakeout)
				position += currentAnimation.cupDistance;
			lerpPosition(0f);
			currentAnimation.active = false;
		}
	}

	void lerpPosition(float time)
	{
		if (currentAnimation.fakeout && time > .5f)
			time = 1f - time;
		transform.position = leftmostPosition + new Vector3(((float)position * cupSeparation)
			+ Mathf.Lerp(0f, (float)cupSeparation * currentAnimation.cupDistance, time),
			moveCurve.Evaluate(time) * (float)currentAnimation.cupDistance * (currentAnimation.backwards ? -1f : 1f), transform.position.z);
	}

}
