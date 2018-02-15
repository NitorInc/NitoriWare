using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DatingSimDialogueController : MonoBehaviour
{

    public float introTextDelay;

    private TMP_Text textComp;
    private AdvancingText textPlayer;

    void Start()
    {
        textComp = GetComponent<TMP_Text>();
        textPlayer = GetComponent<AdvancingText>();

        SetDialogue(DatingSimHelper.getSelectedCharacter().getLocalizedIntroDialogue());
        textPlayer.enabled = false;
        Invoke("EnableTextPlayer", introTextDelay);
    }

    void EnableTextPlayer()
    {
        textPlayer.enabled = true;
    }

    public void SetDialogue(string str)
    {
        textComp.text = str;
        textComp.maxVisibleCharacters = 0;
        textPlayer.resetAdvance();
    }
}
