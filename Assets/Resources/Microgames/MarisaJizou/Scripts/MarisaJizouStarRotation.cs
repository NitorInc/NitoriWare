using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NitorInc.MarisaJizou {
    public class MarisaJizouStarRotation : MonoBehaviour {

        public float speed = 180.0f;
        // Use this for initialization
        void Start() {

        }

        // Update is called once per frame
        void Update() {
            transform.Rotate(0.0f, 0.0f, speed * Time.deltaTime);
        }
    }
}
