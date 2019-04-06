using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NitoriLookTarget : MonoBehaviour
{

#pragma warning disable 0649
    [SerializeField]
    private Collider lookCollider;
    [SerializeField]
    private Animator rigAnimator;
    [SerializeField]
    private float stillLookTime;
    [SerializeField]
    private float movingLookTime;
    [SerializeField]
    private AudioClip victoryClip;
#pragma warning restore 0649

    private float stillLookTimer;
    private float movingLookTimer;

    private bool targetInSight;

    void Start()
	{
        targetInSight = false;

        stillLookTimer = stillLookTime;
        movingLookTimer = movingLookTime;
    }

    void LateUpdate()
    {
        transform.rotation = MainCameraSingleton.instance.transform.rotation;
        updateVictoryLogic();
    }

    void updateVictoryLogic()
    {
        if (targetInSight)
        {
            if (!isMouseMoving())
            {
                stillLookTimer -= Time.deltaTime;
                if (stillLookTimer <= 0f)
                {
                    victory();
                    return;
                }
            }
            else
                stillLookTimer = stillLookTime;

            movingLookTimer -= Time.deltaTime;
            if (movingLookTimer <= 0f)
            {
                victory();
                return;
            }
        }
        else
        {
            stillLookTimer = stillLookTime;
            movingLookTimer = movingLookTime;
        }
    }

    void victory()
    {
        MicrogameController.instance.setVictory(true, true);
        rigAnimator.Play("Victory");
        MicrogameController.instance.playSFX(victoryClip);
        enabled = false;
    }

    bool isMouseMoving()
    {
        return Input.GetAxis("Mouse Y") != 0f || Input.GetAxis("Mouse X") != 0f;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other == lookCollider)
            targetInSight = true;
    }

    void OnTriggerStay(Collider other)
    {
        if (other == lookCollider)
            targetInSight = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other == lookCollider)
            targetInSight = false;
    }

}
