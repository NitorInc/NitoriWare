using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReimuDodgeBullet : MonoBehaviour {

	//In-editor variable. To be set to the Reimu object.
	[Header("The thing to fly towards")]
	[SerializeField]
	private GameObject target;

	//Bullet speed.
	[Header("How fast the bullet goes")]
	[SerializeField]
	private float speed = 1f;

	//Bullet delay before being fired.
	[Header("Firing delay in seconds")]
	[SerializeField]
	private float delay = 1f;

	//Bullet direction of travel.
	private Vector2 trajectory;

	void Start () {
		Invoke("SetTrajectory", delay);
	}
	
	// Update is called once per frame
	void Update () {
		//Move towards the player's starting positon.
		if (trajectory != null) {
			Vector2 newPosition = (Vector2)transform.position + (trajectory * speed * Time.deltaTime);
			transform.position = newPosition;
		}
	}

	void SetTrajectory() {
		trajectory = (target.transform.position - transform.position).normalized;
	}
}
