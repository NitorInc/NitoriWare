using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EikiJudge_PortalsController : MonoBehaviour
{
    public static EikiJudge_PortalsController controller;

    public Transform heavenTransform;
    public Transform hellTransform;

    public EikiJudge_Controller.Direction heavenDirection;
    public EikiJudge_Controller.Direction hellDirection;
    
    private void Awake()
    {
        controller = this;

        // randomly flip position of the heaven/hell portals
        if (Random.value > 0.5f)
        {
            heavenTransform.position = new Vector2(heavenTransform.position.x * -1, heavenTransform.position.y);
            hellTransform.position = new Vector2(hellTransform.position.x * -1, hellTransform.position.y);

            hellDirection = EikiJudge_Controller.Direction.right;
            heavenDirection = EikiJudge_Controller.Direction.left;
        }
        else
        {
            hellDirection = EikiJudge_Controller.Direction.left;
            heavenDirection = EikiJudge_Controller.Direction.right;
        }
    }
}
