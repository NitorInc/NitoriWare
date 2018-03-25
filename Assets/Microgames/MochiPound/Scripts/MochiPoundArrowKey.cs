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
                sr.color = Color.white;
            } 
            else {
                animator.Play(stillAnimationName);
                sr.color = disabledColor;
                if (MicrogameController.instance.getVictory())
                    Disable();
            }
        }

        public void Disable()
        {
            gameObject.SetActive(false);
        }
    }
}
