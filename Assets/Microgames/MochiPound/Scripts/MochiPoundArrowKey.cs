using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NitorInc.MochiPound {
    public class MochiPoundArrowKey : MonoBehaviour {

        public Color disabledColor;
        SpriteRenderer sr;
        private void Start() {
            sr = GetComponent<SpriteRenderer>();
        }
        public void SetActive(bool active) {
            if (active) {
                sr.color = Color.white;
            } 
            else {
                sr.color = disabledColor;
            }
        }
    }
}
