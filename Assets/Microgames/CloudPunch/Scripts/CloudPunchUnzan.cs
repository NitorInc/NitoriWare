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
    [SerializeField]
    private float punchLossCheckTimer = .7f;
    [SerializeField]
    private float maxBeatsLeftBeforeInput = 6f;

    private float initialX;
    private float lastPunchTime = -99999f;

	void Start ()
    {
        //initialX = transform.position.x;
        //updateMovement(true);
    }
	
	void LateUpdate()
    {
        //if (Time.time > lastPunchTime + punchMoveCooldownTime)
        //    updateMovement(false);
        if (Time.time > lastPunchTime + punchRePunchCooldownTime && Input.GetKeyDown(KeyCode.Space)
            && MicrogameController.instance.session.BeatsRemaining <= maxBeatsLeftBeforeInput)
            punch();
        if (MicrogameController.instance.getVictory())
        {
            rigAnimator.SetTrigger("Victory");
            enabled = false;
        }
	}

    void updateMovement(bool snap)
    {
        CloudPunchPiece.awaitingPunch = false;
        var cursorPosition = CameraHelper.getCursorPosition() - MainCameraSingleton.instance.transform.position;
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
        //Invoke("checkLoss", punchLossCheckTimer);
    }

    void checkLoss()
    {
        //if (!MicrogameController.instance.getVictoryDetermined())
        //    MicrogameController.instance.setVictory(false);
    }
}
