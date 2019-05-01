using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RapBattleChoice : MonoBehaviour
{
    [SerializeField]
    private Animator rigAnimator;
    [SerializeField]
    private TextMeshPro tmpComponent;
    [SerializeField]
    private Vector2 selectCamShake;

    private bool isHovering = false;
    private bool isChosen = false;
    private bool isCorrect;

    private float text;

    public void setData(string text, Color color, bool isCorrect)
    {
        tmpComponent.text = text;
        tmpComponent.color = color;
        this.isCorrect = isCorrect;
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
        if (!enabled || MicrogameController.instance.getVictoryDetermined())
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
