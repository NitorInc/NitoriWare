using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinningChar : MonoBehaviour {

    public int spinCount;
    private Vector2 lastVec2Angle;
    private Vector2 lastMouseAngle;
    private Vector2 currentAngle;
    private float lastAngle;
    private float currentAngleChange;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        // Check if we can spin and whether the mouse is in position to
		if (spinCount > 0 && CameraHelper.isMouseOver(gameObject.GetComponent<CircleCollider2D>()))
        {
            // Check for mouse input and interact if so
            if (Input.GetMouseButtonDown(0))
            {
                lastVec2Angle = (Vector2)(CameraHelper.getCursorPosition() - transform.position);
                lastAngle = lastVec2Angle.getAngle();
            } else if (Input.GetMouseButton(0))
            {
                currentAngle = (Vector2)(CameraHelper.getCursorPosition() - transform.position);
                currentAngleChange = currentAngle.getAngle();
                lastMouseAngle = currentAngle;
            }
        }
	}
}
