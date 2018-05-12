using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyTrapController : MonoBehaviour {
	
	

    // Use this for initialization
    void Start () {
		
	}
	
	void LateUpdate()
	{
		Vector3 cursorPosition = CameraHelper.getCursorPosition();
		cursorPosition.y = transform.position.y;
		cursorPosition.z = transform.position.z;
		transform.position = cursorPosition;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
