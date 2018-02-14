using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NitorInc.DatingSim {
    public class DatingSimCharacterPortrait : MonoBehaviour {

        SpriteRenderer sr;

        // 0 = starting portrait
        // 1 = winning portrait
        // 2 = losing portrait
        [SerializeField]
        public List<ListWrapper> characterSprites;
        int currChar;
        // Use this for initialization
        void Start() {
            sr = GetComponent<SpriteRenderer>();
            DatingSimDialoguePreset.OnCharacterSelection += SelectCharacter;
            DatingSimOptionController.OnWinning += SetWinPortrait;
            DatingSimOptionController.OnLosing += SetLosePortrait;
        }

        void SelectCharacter(int index) {
            currChar = index;
            sr.sprite = characterSprites[index].list[0];
        }

        void SetWinPortrait() {
            sr.sprite = characterSprites[currChar].list[1];
        }

        void SetLosePortrait() {
            sr.sprite = characterSprites[currChar].list[2];
        }

        [System.Serializable]
        public class ListWrapper {
            public List<Sprite> list;
        }
    }
}