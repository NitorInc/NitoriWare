using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanBlowFanMovement : MonoBehaviour
{
    [SerializeField]
    private Transform cursorTransform;
    [SerializeField]
    private Transform rotateTransform;
    [SerializeField]
    private Transform tiltTransform;

    [SerializeField]
    private float maxMoveSpeed;
    [SerializeField]
    private float maxTiltAngle;
    [SerializeField]
    private float rotateSpeedMult = 1f;
    [SerializeField]
    private float tiltSpeedMoveSpeedPower = 1f;
    [SerializeField]
    private float tiltSpeedMult = 1f;
    [SerializeField]
    private float tiltAboutFaceSpedMult = 2f;

    private Vector2 lastPosition;

    void Start ()
    {
        lastPosition = transform.position;
	}
	
	void Update ()
    {
        //tiltTransform.localEulerAngles = Vector3.right * Mathf.Clamp(transform.root.position.y * 30f, -maxTiltAngle, maxTiltAngle);
        updatePosition();
    }

    void updatePosition()
    {
        var cursorPosition = (Vector2)cursorTransform.position;
        var positionDiff = (cursorPosition - lastPosition);
        if (positionDiff.magnitude > maxMoveSpeed * Time.deltaTime)
            positionDiff = positionDiff.resize(maxMoveSpeed * Time.deltaTime);
        transform.position = transform.position + (Vector3)positionDiff;

        if (cursorPosition != lastPosition)
        {

            // Rotation first
            var currentRotation = rotateTransform.localEulerAngles.z;
            var goalRotation = positionDiff.getAngle();

            bool headingBackwards = false;
            if (Mathf.Abs(Mathf.DeltaAngle(currentRotation, goalRotation)) > 90f)
            {
                // Add 180 to our goal angle if it's more than 180 degrees away, taking the shorter route
                headingBackwards = true;
                goalRotation = MathHelper.trueMod(goalRotation + 180f, 360f);
            }

            var rotationDiff = (Mathf.Abs(Mathf.DeltaAngle(goalRotation, currentRotation)));
            var newRotation = Mathf.MoveTowardsAngle(currentRotation, goalRotation,
                positionDiff.magnitude * rotationDiff * rotateSpeedMult);

            rotateTransform.localEulerAngles = Vector3.forward * newRotation;


            // Now tilt
            var currentTilt = tiltTransform.localEulerAngles.y;
            var goalTilt = maxTiltAngle * (headingBackwards ? 1f : -1f);
            var tiltDiff = Mathf.Abs(Mathf.DeltaAngle(currentTilt, goalTilt));
            
            var newTilt = Mathf.MoveTowardsAngle(currentTilt, goalTilt,
                Mathf.Pow(positionDiff.magnitude, tiltSpeedMoveSpeedPower)
                * tiltSpeedMult
                * ((tiltDiff > Mathf.Abs(goalTilt)) ? tiltAboutFaceSpedMult : 1f));
            
            tiltTransform.localEulerAngles = Vector3.up * newTilt;
            
            lastPosition = (Vector2)transform.position;
        }
    }
}
