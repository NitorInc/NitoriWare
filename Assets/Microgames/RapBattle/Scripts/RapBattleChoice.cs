using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RapBattleChoice : MonoBehaviour
{
    [SerializeField]
    private Animator rigAnimator;
    [SerializeField]
    private bool isCorrect;
    [SerializeField]
    private Vector2 selectCamShake;

    private bool isHovering = false;
    private bool isChosen = false;

	void Start ()
    {
		
	}
	
	void LateUpdate()
    {
        if (MicrogameController.instance.getVictoryDetermined())
        {
            enabled = false;
            rigAnimator.SetTrigger("Discard");
            return;
        }

        rigAnimator.SetBool("Hover", isHovering);
	}

    private void OnMouseOver()
    {
        if (!enabled)
            return;

        isHovering = true;
        if (Input.GetMouseButtonDown(0))
        {
            MicrogameController.instance.setVictory(isCorrect);
            rigAnimator.SetTrigger("Choose");
            CameraShake.instance.xShake = selectCamShake.x;
            CameraShake.instance.yShake = selectCamShake.y;
            enabled = false;
        }
    }

    private void OnMouseExit()
    {
        isHovering = false;
    }
}
