using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NitorInc.MarisaJizou {

    public class MarisaJizouMarisaStarEffect : MonoBehaviour {

        public Vector3 impulse;
        public float gravity = 5.0f;
        Rigidbody2D rigid;
        TrailRenderer tr;
        SpriteRenderer sr;
        // Use this for initialization
        void Start() {
            rigid = GetComponent<Rigidbody2D>();
            tr = GetComponent<TrailRenderer>();
            sr = GetComponent<SpriteRenderer>();
            MarisaJizouController.onVictory += Activate;
        }

        public void Activate() {
            if (!tr.enabled) {
                tr.enabled = true;
                rigid.gravityScale = 5.0f;
                rigid.AddForce(impulse, ForceMode2D.Impulse);
                Invoke("SelfDestory", 4.0f);
                transform.parent = null;
            }
        }
        
        void SelfDestory() {
            MarisaJizouController.onVictory -= Activate;
            Destroy(gameObject);
        }
    }
}
