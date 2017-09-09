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

    private float defaultScale;

    private State state;
    public enum State
    {
        Default = 0,
        Victory = 1,
        Loss = 2
    }

    
	void Start ()
    {
        defaultScale = transform.localScale.x;
        state = State.Default;
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
