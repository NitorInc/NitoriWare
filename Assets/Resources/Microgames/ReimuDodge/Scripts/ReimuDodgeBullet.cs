﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReimuDodgeBullet : MonoBehaviour {

	[Header("How fast the bullet goes")]
	[SerializeField]
	private float speed = 1f;

	[Header("Firing delay in seconds")]
	[SerializeField]
	private float delay = 1f;

	void SetTrajectory()
	{
		GameObject player = GameObject.Find ("Player");
		trajectory = (player.transform.position - transform.position).normalized;
	}

	private Vector2 trajectory;

	// Use this for initialization
	void Start () { 
		Invoke ("SetTrajectory", delay);
	}
	
	// Update is called once per frame
	void Update () {
		if (trajectory != null)
		{
			// Move the bullet a certain distance based on trajectory speed and time
			Vector2 newPosition = (Vector2)transform.position + (trajectory * speed * Time.deltaTime);
			transform.position = newPosition;
		}
	}
}
