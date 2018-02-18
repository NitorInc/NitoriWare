using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NitorInc.MarisaJizou {

    public class MarisaJizouMarisaKasaEffect : MonoBehaviour {

        MarisaJizouMarisaStarEffect[] stars;
        // Use this for initialization
        void Start() {
            stars = GetComponentsInChildren<MarisaJizouMarisaStarEffect>();
        }

        public void DropEffect() {
            for (int i = 0; i < stars.Length; i++) {
                stars[i].Activate();
            }
        }
    }
}
