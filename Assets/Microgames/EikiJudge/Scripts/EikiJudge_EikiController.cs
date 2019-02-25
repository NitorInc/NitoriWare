using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EikiJudge_EikiController : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer eikiArmSprite;
    [SerializeField]
    private Sprite eikiArmLeft, eikiArmRight;
    [SerializeField]
    private Animator animator;

    public EikiJudge_Controller controller;

    private EikiJudge_Controller.Direction judgementDirection;

    public bool armIsLeft, armIsRight, armIsIdle;


    void Start()
    {
        eikiArmSprite.sprite = null;
        //controller.SpawnSouls();
        
    }

    void Update()
    {
        // If all souls are instantiated
        if (controller.allSoulsReady)
        {
            EikiJudgement();
            AnimateArm();

            // If direction != none 
            if (judgementDirection != EikiJudge_Controller.Direction.none)
            {
                controller.SendJudgement(judgementDirection);
            }
        }
    }

    // Key input / move Eiki's arm / set a direction for the soul
    void EikiJudgement()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            judgementDirection = EikiJudge_Controller.Direction.left;
        }

        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            judgementDirection = EikiJudge_Controller.Direction.right;
        }

        else if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow))
        {
            judgementDirection = EikiJudge_Controller.Direction.none;
        }
        else
        {
            judgementDirection = EikiJudge_Controller.Direction.none;
        }
    }

    // Animate arm...
    void AnimateArm()
    {
        switch (judgementDirection)
        {
            case EikiJudge_Controller.Direction.left:
                if (armIsRight)
                {
                    animator.Play("EikiJudge_right-left", 0, 0f);
                    armIsRight = false;
                }
                else
                {
                    animator.Play("EikiJudge_idle-left", 0, 0f);
                }
                break;
            case EikiJudge_Controller.Direction.right:
                if (armIsLeft)
                {
                    animator.Play("EikiJudge_left-right", 0, 0f);
                    armIsLeft = false;
                }
                else
                {
                    animator.Play("EikiJudge_idle-right", 0, 0f);
                }
                break;
            default:
                break;
        }
    }

}
