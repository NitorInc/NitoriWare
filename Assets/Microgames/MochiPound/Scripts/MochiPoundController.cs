using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NitorInc.MochiPound {

    public enum Hit {
        Left = 0,
        Right
    }

    public class MochiPoundController : MonoBehaviour {

        public int difficulty = 0;
        public int[] requiredHits = {25, 50, 75}; 
        public int RequiredHits {
            get {
                return requiredHits[difficulty];
            }
        }
        public bool IsLastHit {
            get {
                return (RequiredHits - hitCounter) == 1;
            }
        }
        int hitCounter = 0;

        public Animator[] finishAnims;
        // how long in real time the last pound would take
        public float lastPoundElapsedTime = 0.25f;
        // how long in real time the finish pound (pound from both sides) pound would take
        public float finishWaitTime = 0.55f;

        bool hasWon = false;
        Hit lastHit = Hit.Left;

        public MochiPoundRabbitController[] rabbits;

        void Start() {
            hitCounter = 0;
            rabbits[(int)Hit.Left].Disable();
        }

        void Update() {
            if (!hasWon) {
                if (BothAnimationFinished()) {
                    RefreshRabbit(OpposedTo(lastHit));
                    switch (lastHit) {
                        case Hit.Left:
                            if (Input.GetKeyDown(KeyCode.LeftArrow)) {
                                rabbits[(int)Hit.Right].Pound();
                                ++hitCounter;
                                if (hitCounter < RequiredHits)
                                    rabbits[(int)Hit.Left].Windup();
                                lastHit = Hit.Right;
                            }
                            break;
                        case Hit.Right:
                            if (Input.GetKeyDown(KeyCode.RightArrow)) {
                                rabbits[(int)Hit.Left].Pound();
                                ++hitCounter;
                                if (hitCounter < RequiredHits)
                                    rabbits[(int)Hit.Right].Windup();
                                lastHit = Hit.Left;
                            }
                            break;
                    }
                }
            }
        }

        public bool BothAnimationFinished() {
            return rabbits[0].IsAnimationFinished && rabbits[1].IsAnimationFinished;
        }

        public void RefreshRabbit(Hit side) {
            rabbits[(int)side].ResetStatus();
            rabbits[(int)side].ShowButton(true);
        }

        public Hit OpposedTo(Hit side) {
            if (side == Hit.Left) {
                return Hit.Right;
            }
            else {
                return Hit.Left;
            }
        }

        public void OnHit() {
            if (hitCounter >= RequiredHits) {
                MicrogameController.instance.setVictory(true, true);
                hasWon = true;
                for (int i = 0; i < rabbits.Length; i++) {
                    rabbits[i].Disable();
                }
                Invoke("PlayFinishPound", lastPoundElapsedTime);
            }
        }

        void PlayFinishPound() {
            for (int i = 0; i < rabbits.Length; i++) {
                rabbits[i].OnVictory();
            }
            Invoke("PlayFinishSequence", finishWaitTime);
        }

        void PlayFinishSequence() {
            for (int i = 0; i < finishAnims.Length; i++) {
                finishAnims[i].enabled = true;
                finishAnims[i].Play("Finish", 0, 0.0f);
            }
        }

    }
}