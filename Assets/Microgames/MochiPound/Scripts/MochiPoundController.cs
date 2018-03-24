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

        public Animator[] finishAnims;
        public GameObject[] objectsToDisableOnFinish;
        public AudioClip finishPoundSound;
        public float finishPoundVolume = 2.5f;
        bool hasCalled = false;
        public float finishAnimSpeed = 1.0f;

        public void PrepareToStartFinalSequence(float poundAnimTime) {
            if (!hasCalled) {
                hasCalled = true;
                Invoke("PlayFinishSequence", poundAnimTime);
            }
        }

        void PlayFinishSequence() {
            MicrogameController.instance.playSFX(finishPoundSound, MicrogameController.instance.getSFXSource().panStereo, 1.0f, finishPoundVolume);
            for (int i = 0; i < finishAnims.Length; i++) {
                finishAnims[i].speed *= finishAnimSpeed;
                finishAnims[i].enabled = true;
                finishAnims[i].Play("Finish", 0, 0.0f);
            }
            for (int i = 0; i < objectsToDisableOnFinish.Length; i++) {
                objectsToDisableOnFinish[i].SetActive(false);
            }
        }
    }
}