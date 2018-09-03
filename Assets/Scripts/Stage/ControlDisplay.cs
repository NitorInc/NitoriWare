using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlDisplay : MonoBehaviour
{

#pragma warning disable 0649
    [SerializeField]
    private SpriteRenderer controlRenderer;
    [SerializeField]
    private Text controlText;
#pragma warning restore 0649

    public void setControlScheme(MicrogameTraits.ControlScheme controlScheme)
    {
        //TODO re-enable command warnings?
        controlRenderer.sprite = GameController.instance.getControlSprite(controlScheme);
        controlText.text = TextHelper.getLocalizedTextNoWarnings("stage.control." + controlScheme.ToString().ToLower(), getDefaultControlString(controlScheme));
    }

    string getDefaultControlString(MicrogameTraits.ControlScheme controlScheme)
    {
        switch (controlScheme)
        {
            case (MicrogameTraits.ControlScheme.Key):
                return "USE DA KEYZ";
            case (MicrogameTraits.ControlScheme.Mouse):
                return "USE DA MOUSE";
            default:
                return "USE SOMETHING";
        }
    }
}
