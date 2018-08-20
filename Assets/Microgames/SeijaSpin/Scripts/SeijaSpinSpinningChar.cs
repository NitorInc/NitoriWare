using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeijaSpinSpinningChar : MonoBehaviour {

    public int spinCount;
    private Vector2 lastVec2Angle;
    private Vector2 lastMouseAngle;
    private Vector2 currentAngle;
    public Animator seijaAnim, spinAnim;
    private float lastAngle;
    private float currentAngleChange;
    private float angleDifference;
    private float totalSpin = 0f;

    private State state = State.Default;
    // Borrowed with love from Spaceship for Seija
    private enum State
    {
        Default,
        Victory,
        Failure
    }

    private State spinState = State.Default;
    // For Reimu, Mari, and Saku
    private enum SpinState
    {
        Start,
        Spinning,
        SpinOut,
        FightBack
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        // Check if we can spin and whether the mouse is in position to
		if (totalSpin < spinCount*360f && CameraHelper.isMouseOver(gameObject.GetComponent<CircleCollider2D>()))
        {
            // Check for mouse input and interact if so
            if (Input.GetMouseButtonDown(0))
            {
                lastVec2Angle = (Vector2)(CameraHelper.getCursorPosition() - transform.position);
                lastAngle = lastVec2Angle.getAngle();
            } else if (Input.GetMouseButton(0))
            {
                spinAnim.SetInteger("state", (int)SpinState.Spinning);
                currentAngle = (Vector2)(CameraHelper.getCursorPosition() - transform.position);
                currentAngleChange = currentAngle.getAngle();
                lastMouseAngle = currentAngle;
                angleDifference = MathHelper.getAngleDifference(currentAngleChange, lastAngle);
                if (GameObject.Find("Arrows").GetComponent<SeijaSpinArrows>().flipped && angleDifference < 0)
                {
                    totalSpin += Mathf.Abs(angleDifference);
                    lastAngle = currentAngleChange;
                    transform.eulerAngles += Vector3.back * angleDifference;
                } else if (!GameObject.Find("Arrows").GetComponent<SeijaSpinArrows>().flipped && angleDifference > 0)
                {
                    totalSpin += Mathf.Abs(angleDifference);
                    lastAngle = currentAngleChange;
                    transform.eulerAngles += Vector3.back * angleDifference;
                }
            }
        }
        if (totalSpin > spinCount*360f)
        {
            MicrogameController.instance.setVictory(victory: true, final: true);
            spinAnim.SetInteger("state", (int)SpinState.SpinOut);
            seijaAnim.SetInteger("state", (int)State.Victory);
        } else if (!Input.GetMouseButton(0))
        {
            spinAnim.SetInteger("state", (int)SpinState.Start);
        }
	}
}
