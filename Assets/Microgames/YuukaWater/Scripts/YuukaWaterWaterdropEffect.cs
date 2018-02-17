using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NitorInc.Utility;

namespace NitorInc.YuukaWater {


    public class YuukaWaterWaterdropEffect : MonoBehaviour {

        public float scaleSpeed = 1.0f;
        Rigidbody2D rigid;

        void Start() {
            rigid = GetComponent<Rigidbody2D>();
            TimerManager.NewTimer(2.0f, SelfDestruct, 0);
        }

        public void SetInitialForce(Vector2 vel) {
            if (rigid == null)
                rigid = GetComponent<Rigidbody2D>();
            rigid.velocity = vel;
            transform.up = vel * -1.0f;
        }

        void SelfDestruct() {
            if (this != null) {
                Destroy(gameObject);
            }
        }

        // Update is called once per frame
        void Update() {
            transform.up = rigid.velocity * -1.0f;
            transform.localScale += transform.localScale * scaleSpeed * Time.deltaTime;
        }
    }
}