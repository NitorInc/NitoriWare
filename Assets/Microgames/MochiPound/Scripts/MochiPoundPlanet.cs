using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NitorInc.MochiPound {

    public class MochiPoundPlanet : MonoBehaviour {

        public float bumpTime = 0.3f;
        public float bumpForce = 1.0f;
        public float angularRange = 30.0f;
        public float bumpXRange = 1.0f;
        public float bumpYRange = 1.0f;
        public float gravityScale = 1.0f;
        Rigidbody2D rigid;
        float bumpTimer = -1.0f;

        // Use this for initialization
        void Start() {
            bumpTimer = -1.0f;
            rigid = GetComponent<Rigidbody2D>();
        }

        public void Shake() {
            bumpTimer = 0.0f;
            rigid.gravityScale = gravityScale;
            rigid.velocity += new Vector2(Random.Range(-bumpXRange, bumpXRange), Random.Range(-bumpYRange, bumpYRange));
            rigid.angularVelocity = Random.Range(0.0f, angularRange);
        }

        // Update is called once per frame
        void Update() {
            if (bumpTimer >= 0.0f) {
            
                bumpTimer += Time.deltaTime;
            
                if (bumpTimer >= bumpTime) {
                    rigid.gravityScale = 0.0f;
                    rigid.velocity = Vector2.zero;
                    rigid.angularVelocity = 0.0f;
                    bumpTimer = -1.0f;
                }
            }
        }
    }
}