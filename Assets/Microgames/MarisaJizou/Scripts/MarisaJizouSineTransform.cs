using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NitorInc.MarisaJizou {
    public class MarisaJizouSineTransform : MonoBehaviour {

        float rad;
        public float speed = 1.0f;
        public float amplitude = 1.0f;
        public Transform dummyParent;

        // Update is called once per frame
        void Update() {
            Vector3 pos = transform.position;
            rad += Mathf.PI * speed * Time.deltaTime;
            pos.y = dummyParent.position.y + Mathf.Sin(rad) * amplitude;
            transform.position = pos;
        }
    }
}
