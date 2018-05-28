using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SumiWheelThoughtCard : MonoBehaviour
{
    [SerializeField]
    private bool darkenOnSelect;

    private SumiWheelCardClick card;

    public void setCard(SumiWheelCardClick card)
    {
        this.card = card;
        copySpriteAttributes();
    }

    void copySpriteAttributes()
    {
        GetComponent<SpriteRenderer>().sprite = card.GetComponent<SpriteRenderer>().sprite;
        GetComponent<SpriteRenderer>().color = card.GetComponent<SpriteRenderer>().color;
        var icon = transform.GetChild(0);
        var cardIcon = card.transform.GetChild(0);
        icon.GetComponent<SpriteRenderer>().color = cardIcon.GetComponent<SpriteRenderer>().color;
        icon.GetComponent<SpriteRenderer>().sprite = cardIcon.GetComponent<SpriteRenderer>().sprite;
    }

    public SumiWheelCardClick getCard() => card;

    public void select()
    {
        if (darkenOnSelect)
        {
            var spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
            foreach (var spriteRenderer in spriteRenderers)
            {
                var color = spriteRenderer.color;
                color.a = .5f;
                spriteRenderer.color = color;
            }
        }
    }
}
