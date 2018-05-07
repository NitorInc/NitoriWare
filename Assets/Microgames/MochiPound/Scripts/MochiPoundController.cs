using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NitorInc.Utility;

namespace NitorInc.MochiPound {

    public class MochiPoundController : MonoBehaviour {
        public int difficulty = 0;
        public int[] requiredHits = {
            4,6,8
        };
        public int RequiredHit => requiredHits[difficulty];

        int hitCounter;

        public void CountHit() {
            hitCounter++;
            if (hitCounter == RequiredHit) {
                DisableInput();
                PrepareToStartFinalSequence();
                MicrogameController.instance.setVictory(true);
            }
        }

        public bool IsLastHit => hitCounter == RequiredHit;
        bool IsLastHitDetermined => hitCounter == RequiredHit - 1;

        public Animator[] finishAnims;
        public GameObject[] objectsToDisableOnFinish;
        public AudioClip finishPoundSound;
        public float finishPoundVolume = 2.5f;
        public float timeBeforeFinishAnim = 0.15f;
        public float finishAnimSpeed = 1.0f;

        public MochiPoundRabbitController[] rabbits;
        public float timeBetweenInputs = 0.2f;
        public float mistakePenalty = 0.1f;
        bool inputEnabled = true;
        KeyCode lastKey = KeyCode.RightArrow;

        Timer enableTimer;

        void PrepareToStartFinalSequence() {

            Invoke("PlayFinishSequence", timeBeforeFinishAnim);
        }

        void Start() {
            rabbits[0].ShowButton(true);
            rabbits[1].ShowButton(false);
            enableTimer = TimerManager.NewTimer(timeBetweenInputs, EnableInput, 0, false, false);
        }

        void Update() {
            if (inputEnabled) {
                UpdateRabbitInput();
            }
        }

        void UpdateRabbitInput() {
            if (Input.GetKeyDown(KeyCode.LeftArrow)) {
                if (lastKey == KeyCode.RightArrow) {
                    if (!IsLastHitDetermined) {
                        rabbits[0].Pound();
                        rabbits[1].Windup();
                        enableTimer.Restart(timeBetweenInputs);
                        lastKey = KeyCode.LeftArrow;
                    } else {
                        CountHit();
                        for (int i = 0; i < rabbits.Length; i++) {
                            rabbits[i].FinishPound();
                        }
                    }
                }
                else {
                    rabbits[0].OnMistake();
                    enableTimer.Restart(mistakePenalty);
                }
                DisableInput();
            } else if (Input.GetKeyDown(KeyCode.RightArrow)) {
                if (lastKey == KeyCode.LeftArrow) {
                    if (!IsLastHitDetermined) {
                        rabbits[1].Pound();
                        rabbits[0].Windup();
                        enableTimer.Restart(timeBetweenInputs);
                        lastKey = KeyCode.RightArrow;
                    } else {
                        CountHit();
                        for (int i = 0; i < rabbits.Length; i++) {
                            rabbits[i].FinishPound();
                        }
                    }
                }
                else {
                    rabbits[1].OnMistake();
                    enableTimer.Restart(mistakePenalty);
                }
                DisableInput();
            }
        }

        void EnableInput() {
            if (lastKey == KeyCode.LeftArrow) {
                EnableRight();
            } else {
                EnableLeft();
            }
            inputEnabled = true;
        }

        void EnableLeft() {
            rabbits[0].ShowButton(true);
            rabbits[1].ShowButton(false);
        }

        void EnableRight() {
            rabbits[1].ShowButton(true);
            rabbits[0].ShowButton(false);
        }

        void DisableInput() {
            inputEnabled = false;
            for (int i = 0; i < rabbits.Length; i++) {
                rabbits[i].ShowButton(false);
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