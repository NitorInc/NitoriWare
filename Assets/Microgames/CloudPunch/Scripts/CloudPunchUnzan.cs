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

    private float initialX;
    private float lastPunchTime = 0f;

	void Start ()
    {
        initialX = transform.position.x;
	}
	
	void LateUpdate()
    {
        if (Time.time > lastPunchTime + punchMoveCooldownTime)
            updateMovement();
        if (Time.time > lastPunchTime + punchRePunchCooldownTime && Input.GetMouseButtonDown(0))
            punch();
	}

    void updateMovement()
    {
        var cursorPosition = CameraHelper.getCursorPosition();
        var goalPosition = new Vector3(initialX, cursorPosition.y, transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, goalPosition, Time.deltaTime * speedToGoalPosition);
    }

    void punch()
    {
        rigAnimator.SetTrigger("Punch");
        lastPunchTime = Time.time;
    }
}
