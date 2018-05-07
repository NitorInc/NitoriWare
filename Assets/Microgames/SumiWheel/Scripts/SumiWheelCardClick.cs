using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SumiWheelCardClick : MonoBehaviour
{
    private SumiWheelCardController cardController;

    private bool clickable = true;

    public void setController(SumiWheelCardController cardController)
    {
        this.cardController = cardController;
    }

    void OnMouseDown()
    {
        if (!clickable || MicrogameController.instance.getVictoryDetermined())
            return;

        cardController.clickCard(this);
        clickable = false;

        var spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        foreach (var spriteRenderer in spriteRenderers)
        {
            var color = spriteRenderer.color;
            color.a = .5f;
            spriteRenderer.color = color;
        }
    }

    public bool isClickable() => clickable;
}
