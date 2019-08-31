using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KasenPetsHands : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

	void update () {
		
	}
	
	// Update is called once per frame
	void LateUpdate () {
		UpdatePosition ();
	}

	void UpdatePosition(){
		Vector3 cursorPosition = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		cursorPosition.x = transform.position.x;
		cursorPosition.z = transform.position.z;
		transform.position = cursorPosition;
	}
}
