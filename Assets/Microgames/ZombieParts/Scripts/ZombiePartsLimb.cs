﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MouseGrabbable))]
public class ZombiePartsLimb : MonoBehaviour {

	[HideInInspector]
	public bool inCorrectPosition = false;
	[HideInInspector]
	public Vector3 correctPosition = Vector3.zero;


	[SerializeField]
	private float returnSpeed = 5f;
	[SerializeField]
	private float snapSpeed = 15f;


	private MouseGrabbable grabbable;
	private Vector3 startPosition;
	private bool completed = false;
	private bool lastUpdateGrabbable = false;
	private bool isReturning = true;


	public bool GetComplete () {
		return completed;
	}


	public MouseGrabbable GetMouseGrabbable () {
		return grabbable;
	}


	// Use this for initialization
	private void Start () {
		grabbable = GetComponent<MouseGrabbable>();
		startPosition = transform.position;
	}
	

	// Update is called once per frame
	private void Update () {
		if (!completed) {
			if (isReturning) {
				MoveToPosition(startPosition, returnSpeed);
				if ((transform.position - startPosition).magnitude < 0.1f) {
					isReturning = false;
					// transform.position = startPosition;
				}
			} else if (lastUpdateGrabbable && !grabbable.grabbed) {
				if (inCorrectPosition) {
					Complete();
				} else {
					isReturning = true;
				}
			}

			lastUpdateGrabbable = grabbable.grabbed;
		} else {
			MoveToPosition(correctPosition, snapSpeed);
		}
	}


	private void Complete () {
		Debug.Log("COMPLETED");
		completed = true;
		isReturning = false;
		grabbable.enabled = false;
	}


	public bool GetIsReturning ()
	{
		return isReturning;
	}


	private void MoveToPosition (Vector3 newPosition, float speed) {
		transform.position = Vector3.Lerp(transform.position, newPosition, speed * Time.deltaTime);
	}

}
