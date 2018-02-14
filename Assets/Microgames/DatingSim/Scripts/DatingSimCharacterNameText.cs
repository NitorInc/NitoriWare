using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace NitorInc.DatingSim {
    public class DatingSimCharacterNameText : MonoBehaviour {

        TMP_Text textComp;
        DatingSimDialoguePreset preset;
        // Use this for initialization
        void Start() {
            textComp = GetComponent<TMP_Text>();
            preset = FindObjectOfType<DatingSimDialoguePreset>();
            DatingSimDialoguePreset.OnCharacterSelection += SetText;
        }

        void SetText(int index) {
            textComp.text = preset.GetFullName(index);
        }
    }
}
