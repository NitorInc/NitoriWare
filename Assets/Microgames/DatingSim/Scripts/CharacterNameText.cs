using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CharacterNameText : MonoBehaviour {

    TMP_Text textComp;
    DialoguePreset preset;
	// Use this for initialization
	void Start () {
        textComp = GetComponent<TMP_Text>();
        preset = FindObjectOfType<DialoguePreset>();
        DialoguePreset.OnCharacterSelection += SetText;
	}

    void SetText(int index) {
        textComp.text = preset.GetFullName(index);
    }
}
