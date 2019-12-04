using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudPunchUnzan : MonoBehaviour
{
    [SerializeField]
    private float speedToGoalPosition = 40f;
    [SerializeField]
    private float punchRePunchCooldownTime = .25f;
    [SerializeField]
    private float punchMoveCooldownTime = .1f;
    [SerializeField]
    private Animator rigAnimator;
    [SerializeField]
    private float punchScreenshake = .2f;

    private float initialX;
    private float lastPunchTime = -99999f;

	void Start ()
    {
        initialX = transform.position.x;
        updateMovement(true);
    }
	
	void LateUpdate()
    {
        if (Time.time > lastPunchTime + punchMoveCooldownTime)
            updateMovement(false);
        if (Time.time > lastPunchTime + punchRePunchCooldownTime && Input.GetMouseButtonDown(0))
            punch();
	}

    void updateMovement(bool snap)
    {
        CloudPunchPiece.awaitingPunch = false;
        var cursorPosition = CameraHelper.getCursorPosition();
        var goalPosition = new Vector3(initialX, cursorPosition.y, transform.position.z);
        if (snap)
            transform.position = goalPosition;
        else
            transform.position = Vector3.MoveTowards(transform.position, goalPosition, Time.deltaTime * speedToGoalPosition);
    }

    void punch()
    {
        rigAnimator.SetTrigger("Punch");
        CloudPunchPiece.awaitingPunch = true;
        lastPunchTime = Time.time;
        CameraShake.instance.setScreenShake(punchScreenshake);
    }
}
