using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CupShuffleCup : MonoBehaviour
{
	public int position;
	public Vector3 leftmostPosition;
	public float cupSeparation;
	public bool isCorrect;
	public AnimationCurve moveCurve;

	private Animation currentAnimation;
	private struct Animation
	{
		public bool active, goingUp;
		public int cupDistance;
		public float startTime, duration;
	}

	void Start()
	{
		transform.position = leftmostPosition + (Vector3.right * (float)position * cupSeparation);
		currentAnimation.active = false;
	}

	public void startAnimation(int cupDistance, float duration, bool goingUp)
	{
		currentAnimation.active = true;
		currentAnimation.startTime = Time.time;
		currentAnimation.cupDistance = cupDistance;
		currentAnimation.duration = duration;
		currentAnimation.goingUp = goingUp;
	}
	
	void Update ()
	{
		if (currentAnimation.active)
			updateAnimation();
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
			position += currentAnimation.cupDistance;
			lerpPosition(0f);
			currentAnimation.active = false;
		}
	}

	void lerpPosition(float time)
	{
		transform.position = leftmostPosition + new Vector3(((float)position * cupSeparation)
			+ Mathf.Lerp(0f, (float)cupSeparation * currentAnimation.cupDistance, time),
			moveCurve.Evaluate(time) * (currentAnimation.goingUp ? 1f : -1f), transform.position.z);
	}

}
