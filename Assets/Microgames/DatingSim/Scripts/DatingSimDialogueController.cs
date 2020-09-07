using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DatingSimDialogueController : MonoBehaviour
{
    public float introTextDelay;
    [Tooltip("If set to >0 will slow down or speed up text advance to complete it in this time")]
    public float introTextForceCompletionTime;

    private TMP_Text textComp;
    private AdvancingText textPlayer;
    private float defaultTextSpeed;


    private void Awake()
    {
        textComp = GetComponent<TMP_Text>();
        textPlayer = GetComponent<AdvancingText>();
        defaultTextSpeed = textPlayer.getAdvanceSpeed();
    }

    private void OnTextLocalized()
    {
        SetDialogue(textComp.text);
        OnFontLocalized();

        textPlayer.enabled = false;
        Invoke("EnableTextPlayer", introTextDelay);
    }

    void OnFontLocalized()
    {
        if (introTextForceCompletionTime > 0f)
        {
            float newSpeed = textPlayer.getTotalVisibleChars() / introTextForceCompletionTime;
            textPlayer.setAdvanceSpeed(newSpeed);
        }

    }

    void EnableTextPlayer()
    {
        textPlayer.enabled = true;
    }

    public void resetDialogueSpeed()
    {
        textPlayer.setAdvanceSpeed(defaultTextSpeed);
    }

    public void SetDialogue(string str)
    {
        textComp.text = str;
        textComp.maxVisibleCharacters = 0;
        textPlayer.resetAdvance();
    }
}
