using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

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
    [SerializeField]
    private float maxY = 5 * 4f / 3f;
    [SerializeField]
    private float maxX = 5;

    private Vector2 lastPosition;

    public Vector2 CurrentVelocity { get; private set; }
    public float CurrentSpeed => CurrentVelocity.magnitude;

    private bool wasPaused;
    public bool WasPaused
    {
        get { return wasPaused; }
        set { wasPaused = value; }
    }

    void Start ()
    {
        transform.position = GetRayCastPosition();
        lastPosition = transform.position;
        cursorTransform.position = transform.position;
	}
	
	void Update ()
    {
        //tiltTransform.localEulerAngles = Vector3.right * Mathf.Clamp(transform.root.position.y * 30f, -maxTiltAngle, maxTiltAngle);
        updatePosition();
    }

    void updatePosition()
    {
        if (MicrogameController.instance.getVictoryDetermined())
        {
            //transform.position += currentVelocity * Time.deltaTime;
            return;
        }

        if (wasPaused && !MicrogameController.instance.getVictoryDetermined())
        {
            cursorTransform.position = GetRayCastPosition();
            transform.position = new Vector3(cursorTransform.position.x, cursorTransform.position.y, transform.position.z);
            lastPosition = transform.position;
            wasPaused = false;
            return;
        }

        var cursorPosition = (Vector2)cursorTransform.position;
        var positionDiff = (cursorPosition - lastPosition);
        if (positionDiff.magnitude > maxMoveSpeed * Time.deltaTime)
            positionDiff = positionDiff.resize(maxMoveSpeed * Time.deltaTime);
        CurrentVelocity = positionDiff / Time.deltaTime;
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

    private void LateUpdate()
    {
        //if (Input.mousePosition.x <= 0 || Input.mousePosition.y <= 0 || Input.mousePosition.x >= Screen.width - 1 || Input.mousePosition.y >= Screen.height - 1) return;
            cursorTransform.position = GetRayCastPosition();
    }

    Vector3 GetRayCastPosition()
    {
        var pos = Input.mousePosition;
        var buffer = ((float)Screen.width - ((float)Screen.height * 4f / 3f)) / 2f;
        //print(Screen.width);
        //print(Screen.height);
        pos = new Vector3(Mathf.Clamp(pos.x, buffer, Screen.width - buffer), Mathf.Clamp(pos.y, 0f, Screen.height - 1));
        //print(pos.x);
        //print((Screen.height * 4f / 3f) - 2f);
        var ray = MainCameraSingleton.instance.ScreenPointToRay(pos);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo))
            return hitInfo.point;
        return Vector3.zero;
    }
}
