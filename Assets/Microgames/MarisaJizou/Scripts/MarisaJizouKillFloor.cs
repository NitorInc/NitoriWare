using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NitorInc.MarisaJizou {

    public class MarisaJizouKillFloor : MonoBehaviour {

        MarisaJizouController controller;
        // Use this for initialization
        void Start() {
            controller = FindObjectOfType<MarisaJizouController>();
        }

        private void OnCollisionEnter2D(Collision2D collision) {
            controller.Notify(false);
        }
    }
}
