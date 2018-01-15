using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeachBallPressSpaceToWin : MonoBehaviour {

	public GameObject BeachBallBeachBall;
	public Animator BeachBallBeachBallAnimator;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Space))
		{
			// Ball goes up and hopefully through the hoop

			PlayBallAnimation();
		}
		
	}

	public void PlayBallAnimation()
	{BeachBallBeachBallAnimator.Play("BeachBallBallGoesUp");
	}
}
