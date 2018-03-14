

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NitorInc.Utility;

namespace NitorInc.YuukaWater {

    public class YuukaWaterWaterdrop : MonoBehaviour {

        public float scaleSpeed = 1.0f;
        public GameObject sprayEffect;
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
        
        private void OnCollisionEnter2D(Collision2D collision) {
            if (collision.gameObject.GetComponentInParent<YuukaWaterWaterdrop>() == null) {
                if (collision.contacts.Length > 0)
                    Instantiate(sprayEffect, collision.contacts[0].point, Quaternion.identity);
                Destroy(gameObject);
            }
        }
    }
}
