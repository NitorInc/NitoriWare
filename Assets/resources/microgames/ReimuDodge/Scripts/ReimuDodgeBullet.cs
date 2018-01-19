using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReimuDodgeBullet : MonoBehaviour
{
	//Editor variables

	[Header("How fast the bullet goes")]
	[SerializeField]
	private float speed = 1f;

	[Header("Firing delay in seconds")]
	[SerializeField]
	private float delay = 1f;



	//Other variables

	//Stores the direction of travel for the bullet
	private Vector2 trajectory;

	// Use this for initialization
	void Start ()
	{
		Invoke ("SetTrajectory", delay);
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (trajectory != null)
		{
			// Move the bullet based on calculated trajectory, speed, and deltatime
			Vector2 newPosition = (Vector2)transform.position + (trajectory * speed * Time.deltaTime);
			transform.position = newPosition;
		}
	}

	void SetTrajectory()
	{
		// Find the player object in the scene and calculate trajectory towards them
		GameObject player = GameObject.Find ("Player");
		trajectory = (player.transform.position - transform.position).normalized;
	}
}
