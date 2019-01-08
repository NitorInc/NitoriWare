using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodRoastStopAnimationOnResult : MonoBehaviour
{
    private Animator rigAnimator;
    
	void Start ()
    {
        rigAnimator = GetComponent<Animator>();
	}
	
	void Update ()
    {
		if (MicrogameController.instance.getVictoryDetermined())
        {
            rigAnimator.speed = 0f;
            enabled = false;
        }
	}
}
