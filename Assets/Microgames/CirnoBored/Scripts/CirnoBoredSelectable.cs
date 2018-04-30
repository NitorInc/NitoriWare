using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CirnoBoredSelectable : MonoBehaviour
{
    public bool victoryItem;
    public Collider2D clickCollider;
    public Animator itemAnimator;
    public Animator cirnoAnimator;
    public Transform selectGoalAnchor;
    public float selectMoveSpeed;
    public float idleAnimationOffset;
    public AudioClip victoryClip;
    public AudioClip lossClip;

    private State state;
    public enum State
    {
        Default = 0,
        Victory = 1,
        Loss = 2
    }

    
	void Start ()
    {
        state = State.Default;
        itemAnimator.Play("Idle", 0, idleAnimationOffset);
	}
	
	void Update ()
    {
        if (!MicrogameController.instance.getVictoryDetermined())
            updateSelect();
        else if (state != State.Default)
            updateSelectedMovement();
    }

    void updateSelect()
    {
        if (CameraHelper.isMouseOver(clickCollider))
        {
            itemAnimator.SetBool("hovering", true);
            if (Input.GetMouseButtonDown(0))
                select();
        }
        else
        {
            itemAnimator.SetBool("hovering", false);
        }
    }

    void select()
    {
        state = victoryItem ? State.Victory : State.Loss;
        MicrogameController.instance.setVictory(victoryItem, true);
        //transform.parent = null;
        itemAnimator.SetBool("hovering", false);
        itemAnimator.SetInteger("state", (int)state);
    }

    void playResultAnimation()
    {
        cirnoAnimator.SetInteger("state", (int)state);
        itemAnimator.SetBool("inPlace", true);
        MicrogameController.instance.playSFX(victoryItem ? victoryClip : lossClip);
    }

    void updateSelectedMovement()
    {
        if (transform.moveTowards(selectGoalAnchor.position, selectMoveSpeed))
        {
            playResultAnimation();
            enabled = false;
        }
    }
    
}
