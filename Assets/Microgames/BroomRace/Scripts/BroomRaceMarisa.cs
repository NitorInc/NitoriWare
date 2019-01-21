using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BroomRaceMarisa : MonoBehaviour {

	[SerializeField]
	float moveSpeed;

	[SerializeField]
	float yBound;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.UpArrow)) 
		{
			transform.position += Vector3.up * moveSpeed * Time.deltaTime;
			if (transform.position.y > yBound) 
				transform.position = new Vector3 (transform.position.x, yBound, transform.position.z);
		}

		if (Input.GetKey (KeyCode.DownArrow)) 
		{
			transform.position += Vector3.down * moveSpeed * Time.deltaTime;

			if (transform.position.y < -yBound ) 
				transform.position = new Vector3 (transform.position.x, -yBound, transform.position.z);
		}



	}
}
