using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KasenPetsAnimal : MonoBehaviour {

	[Header("Speed")]
	[SerializeField]
	private float speed = 1f;

	//Direction of travel.
	private Vector2 trajectory = Vector2.up;


	SpriteRenderer myRenderer;


	//Sets trajectory to a random direction, and rotates sprite accordingly.

	//Currently the animal moves faster the greater y is, because x-axis movement speed is constant.
	//Might change that later.
	void Start () {
		myRenderer = GetComponentInChildren<SpriteRenderer> ();

		//x becomes either 1 or -1, y becomes anything between -1 and 1.
		int x = 1;
		float y = Random.value;

		switch (Random.Range (0, 4)) {
		case 0:
			break;
		case 1:
			x *= -1;
			break;
		case 2:
			y *= -1;
			break;
		case 3:
			x *= -1;
			y *= -1;
			break;
		}

		Vector2 newTrajectory = new Vector2 (x, y);
		//Rotate sprite.
		transform.rotation = Quaternion.Euler (0, 0, Vector2.Angle(trajectory, newTrajectory) * -x/Mathf.Abs(x));
		trajectory = newTrajectory;
	}

	//Bounce off walls.
	void OnTriggerEnter2D(Collider2D other){
		if (other.CompareTag ("MicrogameTag1") == true) {
			//hit ceiling or floor.
			trajectory.y *= -1;
			transform.rotation = Quaternion.Euler (0, 0, 180 - transform.rotation.eulerAngles.z);
		} else {
			//Hit hand.
			trajectory.x *= -1;
			transform.rotation = Quaternion.Euler (0, 0, -transform.rotation.eulerAngles.z);
		}
	}

	

	//Move along trajectory.
	void Update () {
		transform.position = (Vector2)transform.position + (trajectory * speed * Time.deltaTime);
	}
}
