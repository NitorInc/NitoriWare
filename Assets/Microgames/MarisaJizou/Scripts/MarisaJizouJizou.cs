using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NitorInc.MarisaJizou {

    public class MarisaJizouJizou : MonoBehaviour {

        MarisaJizouController controller;
        bool hasTouched = false;

        public GameObject happyExp;
        public GameObject sadExp;
        public GameObject hat;
        public GameObject onHitEffectProto;
        public Transform onHitEffectTrans;
        Collider2D col;

        public void Register(MarisaJizouController controller) {
            this.controller = controller;
            col = GetComponentInChildren<Collider2D>();
        }

        public void HatLanding() {
            if (!hasTouched) {
                controller.Notify(true);
                hasTouched = true;
                happyExp.SetActive(true);
                sadExp.SetActive(false);
                hat.SetActive(true);
                var go = Instantiate(onHitEffectProto) as GameObject;
                go.transform.position = onHitEffectTrans.position;
                go.SetActive(true);
                col.enabled = false;
            }
        }
    }
}