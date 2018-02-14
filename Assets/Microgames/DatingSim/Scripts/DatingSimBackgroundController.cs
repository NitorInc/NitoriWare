using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NitorInc.DatingSim {
    public class DatingSimBackgroundController : MonoBehaviour {

        SpriteRenderer sr;
        public Sprite[] bgSprites;
        // Use this for initialization
        void Start() {
            sr = GetComponent<SpriteRenderer>();
            DatingSimDialoguePreset.OnCharacterSelection += SetBackground;
        }

        void SetBackground(int index) {
            sr.sprite = bgSprites[index];
        }

    }
}
