using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueController : MonoBehaviour {

    private TMP_Text textComp;
    private AdvancingText textPlayer;
    DialoguePreset preset;

    void Start() {
        textComp = GetComponent<TMP_Text>();
        textPlayer = GetComponent<AdvancingText>();
        preset = FindObjectOfType<DialoguePreset>();
        DialoguePreset.OnCharacterSelection += SetDialogue;
    }

    void SetDialogue(int index) {
        textComp.text = preset.GetStartingDialogue(index);
        textComp.maxVisibleCharacters = 0;
        textPlayer.resetAdvance();
    }

    public void SetDialogue(string str) {
        textComp.text = str;
        textComp.maxVisibleCharacters = 0;
        textPlayer.resetAdvance();
    }
}
