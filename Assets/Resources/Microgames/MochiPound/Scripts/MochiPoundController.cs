using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NitorInc.MochiPound {

    public class MochiPoundController : MonoBehaviour {

        public int difficulty = 0;
        public int[] requiredHits = {25, 50, 75}; 
        public int RequiredHits {
            get {
                return requiredHits[difficulty];
            }
        }
        int hitCounter;
        public delegate void OnAction();
        public static event OnAction OnVictory;

        public Animator[] finishAnims;
        public float finishWaitTime = 0.3f;

        public void OnHit() {
            ++hitCounter;
            if (hitCounter >= RequiredHits) {
                MicrogameController.instance.setVictory(true, true);
                if (OnVictory != null) {
                    OnVictory();
                }
                Invoke("PlayFinishSequence", finishWaitTime);
            }
        }

        void PlayFinishSequence() {
            for (int i = 0; i < finishAnims.Length; i++) {
                finishAnims[i].enabled = true;
                finishAnims[i].Play("Finish", 0, 0.0f);
            }
        }

        void OnDestroy() {
            OnVictory = null;
        }
    }
}