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
    private EikiJudge_Controller.Direction lastJudgementDirection;

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
        lastJudgementDirection = judgementDirection;

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            judgementDirection = EikiJudge_Controller.Direction.left;
            animator.Play("EikiJudge_idle-left", 0, 0f);
        }

        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            judgementDirection = EikiJudge_Controller.Direction.right;
            animator.Play("EikiJudge_idle-right", 0, 0f);
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
        switch (lastJudgementDirection)
        {
            case EikiJudge_Controller.Direction.none:

                break;
            case EikiJudge_Controller.Direction.left:
                break;
            case EikiJudge_Controller.Direction.right:
                break;
            default:
                break;
        }
    }

}
