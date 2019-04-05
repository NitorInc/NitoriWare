using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeijaSpinFinger : MonoBehaviour {

    private Vector3 originalPosition;

	// Use this for initialization
	void Start () {
        originalPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        if (!MicrogameController.instance.getVictory() && Input.GetMouseButton(0))
        {
            Vector2 cursorLocation = (Vector2)(CameraHelper.getCursorPosition());
            transform.position = new Vector3(originalPosition.x + cursorLocation.x / 5, originalPosition.y + cursorLocation.y / 5, originalPosition.z);
        }
    }
}
