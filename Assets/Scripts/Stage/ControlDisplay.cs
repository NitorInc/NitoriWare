using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ControlDisplay : MonoBehaviour
{

#pragma warning disable 0649
    [SerializeField]
    private SpriteRenderer controlRenderer;
    [SerializeField]
    private Text controlText;
    [SerializeField]
    private TextMeshPro controlTmpComponent;
    [SerializeField]
    private StageController stageController;
#pragma warning restore 0649

    public void setControlScheme(Microgame.ControlScheme controlScheme)
    {
        //TODO re-enable command warnings?
        controlRenderer.sprite = stageController.controlSchemeSprites[(int)controlScheme];

        var text = TextHelper.getLocalizedTextNoWarnings("stage.control." + controlScheme.ToString().ToLower(), getDefaultControlString(controlScheme));

        if (controlText != null)
            controlText.text = text;
        if (controlTmpComponent != null)
            controlTmpComponent.text = text;
    }

    string getDefaultControlString(Microgame.ControlScheme controlScheme)
    {
        switch (controlScheme)
        {
            case (Microgame.ControlScheme.Key):
                return "USE DA KEYZ";
            case (Microgame.ControlScheme.Mouse):
                return "USE DA MOUSE";
            default:
                return "USE SOMETHING";
        }
    }
}
