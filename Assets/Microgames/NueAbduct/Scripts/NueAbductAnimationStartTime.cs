using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NueAbductAnimationStartTime : MonoBehaviour
{
    [SerializeField]
    private float startTime = 0.0f;

	void Start ()
	{
	    GetComponent<Animator>().Play("UFOTitsClip", -1, startTime);
	}
}
