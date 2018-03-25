using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NitorInc.MochiPound {
    public class MochiPoundArrowKey : MonoBehaviour {

        public Color disabledColor;
        public Animator animator;

        public string stillAnimationName = "ButtonStill";
        public string pulseAnimationName = "ButtonPulse";

        SpriteRenderer sr;

        bool isActive = false;
        private void Start() {
            sr = GetComponent<SpriteRenderer>();
        }
        public void SetActive(bool active) {
            if (active) {
                animator.Play(pulseAnimationName);
                sr.color = Color.white;
            } 
            else {
                animator.Play(stillAnimationName);
                sr.color = disabledColor;
                if (MicrogameController.instance.getVictory())
                    disable();
            }
            isActive = active;
        }

        public bool IsActive() {
            return isActive;
        }

        public void disable()
        {
            gameObject.SetActive(false);
        }
    }
}
