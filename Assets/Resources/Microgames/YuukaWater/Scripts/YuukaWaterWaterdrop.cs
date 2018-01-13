

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NitorInc.Utility;

namespace NitorInc.YuukaWater {

    public class YuukaWaterWaterdrop : MonoBehaviour {

        public float scaleSpeed = 1.0f;
        Rigidbody2D rigid;

        private Vector2 yuukaVel;

        void Start() {
            rigid = GetComponent<Rigidbody2D>();
            TimerManager.NewTimer(2.0f, SelfDestruct, 0);
        }

        public void SetInitialForce(Vector2 vel, Vector2 yuukaVel)
        {
            this.yuukaVel = yuukaVel;
            if (rigid == null)
                rigid = GetComponent<Rigidbody2D>();
            rigid.velocity = vel + yuukaVel;

            Update();
        }

        void SelfDestruct() {
            if (this != null) {
                Destroy(gameObject);
            }
        }

        // Update is called once per frame
        void Update() {
            transform.up = (rigid.velocity - yuukaVel) * -1.0f;
            transform.localScale += transform.localScale * scaleSpeed * Time.deltaTime;
        }

        void OnTriggerEnter2D(Collider2D other) {
            if (other.GetComponentInParent<YuukaWaterWaterdrop>() == null) {
                Destroy(gameObject);
            }
        }
    }
}
