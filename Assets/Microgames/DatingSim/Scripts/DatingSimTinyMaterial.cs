using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NitorInc.DatingSim {
    public class DatingSimTinyMaterial : MonoBehaviour {

        public Material[] mats;
        // Use this for initialization
        void Start() {
            DatingSimDialoguePreset.OnCharacterSelection += ChangeMaterial;
        }

        void ChangeMaterial(int index) {
            GetComponent<Renderer>().material = new Material(mats[index]);
        }

        // Update is called once per frame
        void Update() {

        }
    }
}
