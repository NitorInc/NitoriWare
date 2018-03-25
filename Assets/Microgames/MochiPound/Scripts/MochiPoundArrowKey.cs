using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NitorInc.MochiPound {
    public class MochiPoundArrowKey : MonoBehaviour {

        public Color disabledColor;
        public Animator animator;

        public string stillAnimationName = "ButtonStill";
        public string pulseAnimationName = "ButtonPulse";

        public GameObject xSpriteObj;
        public float xDuration = 0.12f;

        SpriteRenderer sr;
        SpriteRenderer Sr {
            get {
                if (sr == null)
                    sr = GetComponent<SpriteRenderer>();
                return sr;
            }
        }

        public void SetActive(bool active) {
            if (active) {
                animator.Play(pulseAnimationName);
                Sr.color = Color.white;
            } 
            else {
                animator.Play(stillAnimationName);
                Sr.color = disabledColor;
                if (MicrogameController.instance.getVictory())
                    Disable();
            }
        }

        public void OnMistake() {
            xSpriteObj.SetActive(true);
            Invoke("DisableX", xDuration);
        }

        void DisableX() {
            xSpriteObj.SetActive(false);
        }

        public void Disable()
        {
            gameObject.SetActive(false);
        }
    }
}
