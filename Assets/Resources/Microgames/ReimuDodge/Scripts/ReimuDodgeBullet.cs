using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReimuDodgeBullet : MonoBehaviour {

	[Header("How fast the bullet goes")]
	[SerializeField]
	private float speed = 1f;

	[Header("Firing delay in sceonds")]
	[SerializeField]
	private float delay = 1f;

	private Vector2  trajectory;
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
			Vector2 newPosition = (Vector2)transform.position + (trajectory * speed * Time.deltaTime);
			transform.position = newPosition;
		}
	}

	void SetTrajectory()
	{
		GameObject player = GameObject.Find ("Player");
		trajectory = (player.transform.position - transform.position).normalized;
	}
}
