using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NitorInc.MochiPound {

    public enum Hit {
        Left = 0,
        Right,
        Both
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
        public float finishWaitTime = 0.3f;

        bool hasWon = false;
        Hit lastHit = Hit.Both;

        public MochiPoundRabbitController[] rabbits;

        void Start() {
            hitCounter = 0;
        }

        void Update() {
            if (!hasWon) {
                switch (lastHit) {
                    case Hit.Both:
                        if (Input.GetKeyDown(KeyCode.RightArrow)) {
                            rabbits[(int)Hit.Left].Pound();
                            rabbits[(int)Hit.Left].ShowButton(false);
                            ++hitCounter;
                            lastHit = Hit.Left;
                        } else if (Input.GetKeyDown(KeyCode.LeftArrow)) {
                            rabbits[(int)Hit.Right].Pound();
                            rabbits[(int)Hit.Right].ShowButton(false);
                            ++hitCounter;
                            lastHit = Hit.Right;
                        }
                        break;
                    case Hit.Left:
                        if (Input.GetKeyDown(KeyCode.LeftArrow)) {
                            rabbits[(int)Hit.Right].Pound();
                            rabbits[(int)Hit.Right].ShowButton(false);
                            rabbits[(int)Hit.Left].ResetStatus();
                            rabbits[(int)Hit.Left].ShowButton(true);
                            ++hitCounter;
                            lastHit = Hit.Right;
                        }
                        break;
                    case Hit.Right:
                        if (Input.GetKeyDown(KeyCode.RightArrow)) {
                            rabbits[(int)Hit.Left].Pound();
                            rabbits[(int)Hit.Left].ShowButton(false);
                            rabbits[(int)Hit.Right].ResetStatus();
                            rabbits[(int)Hit.Right].ShowButton(true);
                            ++hitCounter;
                            lastHit = Hit.Left;
                        }
                        break;
                }
            }
        }

        public void OnHit() {
            if (hitCounter >= RequiredHits) {
                MicrogameController.instance.setVictory(true, true);
                hasWon = true;
                for (int i = 0; i < rabbits.Length; i++) {
                    rabbits[i].OnVictory();
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

    }
}