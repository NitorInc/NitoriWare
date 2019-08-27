using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewGame6Animal : MonoBehaviour {

	[Header("Speed")]
	[SerializeField]
	private float speed = 1f;

	//Direction of travel.
	private Vector2 trajectory;


	//Sets trajectory to a random direction. x is either 1 or -1, y is between -1 and 1.
	void Start () {
		int x = 1;
		float y = Random.value;

		switch (Random.Range (0, 3)) {
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
		
		trajectory = new Vector2(x, y);
	}
		
	void OnTriggerEnter2D(Collider2D other){
		if (other.CompareTag ("MicrogameTag1") == true) {
			trajectory.y *= -1;
		} else {
			trajectory.x *= -1;
		}
	}

	//Move along trajectory.
	void Update () {
		transform.position = (Vector2)transform.position + (trajectory * speed * Time.deltaTime);
	}
}
