using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NitorInc.MochiPound {

    public class MochiPoundController : MonoBehaviour {
        public int difficulty = 0;
        public int[] requiredHits = {
            4,6,8
        };
        public int RequiredHit {
            get {
                return requiredHits[difficulty];
            }
        }

        int hitCounter;

        public void CountHit() {
            hitCounter++;
        }

        public bool IsLastHit {
            get {
                return hitCounter == RequiredHit;
            }
        }

        public float finishPoundAnimTime = 0.2f;
        public float finishSequenceAnimTime = 0.2f;
        public Animator[] finishAnims;
        public GameObject[] objectsToDisableOnFinish;
        public void PrepareToStartFinalSequence() {
            Invoke("PlayFinishSequence", finishSequenceAnimTime);
        }
        void PlayFinishSequence() {
            for (int i = 0; i < finishAnims.Length; i++) {
                finishAnims[i].enabled = true;
                finishAnims[i].Play("Finish", 0, 0.0f);
            }
            for (int i = 0; i < objectsToDisableOnFinish.Length; i++) {
                objectsToDisableOnFinish[i].SetActive(false);
            }
        }
    }
}