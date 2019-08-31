using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KasenPetsHands : MonoBehaviour {

	void Start () {
		
	}

	void update () {
		
	}

	void LateUpdate () {
		UpdatePosition ();
	}

	//Basically the same as FollowCursor, but only affects the y-coordinate.
	void UpdatePosition(){
		Vector3 cursorPosition = CameraHelper.getCursorPosition();
		cursorPosition.x = transform.position.x;
		cursorPosition.z = transform.position.z;
		transform.position = cursorPosition;
	}
}
