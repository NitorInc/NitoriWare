using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EikiJudge_EikiController : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer eikiArmSprite;
    [SerializeField]
    private Sprite eikiArmLeft, eikiArmRight;

    public EikiJudge_Controller controller;

    private EikiJudge_Controller.Direction judgementDirection;

    void Start()
    {
        eikiArmSprite.sprite = null;
        controller.SpawnSouls();
    }

    void Update()
    {
        // If all souls are instantiated AND game isn't lost
        if (controller.allSoulsReady && controller.wasted == false)
        {
            EikiJudgement();

            // If direction != none 
            if (judgementDirection != EikiJudge_Controller.Direction.none)
            {
                //soulsList[0].soulController.SendTheSoul(judgementDirection);
                controller.SendJudgement(judgementDirection);
            }
        }
    }

    // Key input / move Eiki's arm / set a direction for the soul
    // TODO: refactor to implement animation
    void EikiJudgement()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            eikiArmSprite.sprite = eikiArmLeft;
            judgementDirection = EikiJudge_Controller.Direction.left;
        }

        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            eikiArmSprite.sprite = eikiArmRight;
            judgementDirection = EikiJudge_Controller.Direction.right;
        }

        else if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow))
        {
            eikiArmSprite.sprite = null;
            judgementDirection = EikiJudge_Controller.Direction.none;
        }

        else
        {
            judgementDirection = EikiJudge_Controller.Direction.none;
        }
    }
}
