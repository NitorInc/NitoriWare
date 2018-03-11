using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NitorInc.ClownTorch {
    public class ClownTorchWater : MonoBehaviour {

        // Use this for initialization
        void Start() {

        }

        // Update is called once per frame
        void Update() {

        }

        private void OnTriggerEnter2D(Collider2D collision) {
            var tag = collision.GetComponentInParent<ClownTorchTag>();
            if (tag.type == ClownTorchTag.Type.PlayerTorch) {
                var obj = tag.GetComponent<ClownTorchTorchObject>();
                obj.TurnOff();
            }
        }
    }
}