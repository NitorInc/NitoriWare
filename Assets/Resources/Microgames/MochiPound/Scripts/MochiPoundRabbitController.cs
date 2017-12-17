using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NitorInc.MochiPound {

    public class MochiPoundRabbitController : MonoBehaviour {

        Animator anim;
        public float poundNormalizedTime = 0.55f;
        public bool isLeft;
        KeyCode key = KeyCode.RightArrow;
        KeyCode opposingKey = KeyCode.LeftArrow;
        bool hasPounded = false;
        bool hasHit = true;
        bool hasWon = false;

        public Animator mochiAnim;
        CameraShake shake;
        public float xShake = 0.1f;
        public float shakeSpeed = 1.0f;
        MochiPoundPlanet[] planets;
        MochiPoundController ctrler;

        public string poundAnimName = "Pounding";
        int poundAnimNameHash;

        // Use this for initialization
        void Start() {
            anim = GetComponent<Animator>();
            shake = FindObjectOfType<CameraShake>();
            planets = FindObjectsOfType<MochiPoundPlanet>();
            ctrler = FindObjectOfType<MochiPoundController>();
            poundAnimNameHash = Animator.StringToHash(poundAnimName);
            if (isLeft) {
                key = KeyCode.LeftArrow;
                opposingKey = KeyCode.RightArrow;
            }

            MochiPoundController.OnVictory += OnVictory;
        }

        // Update is called once per frame
        void Update() {
            if (!hasWon) {
                if (Input.GetKeyDown(key) && !hasPounded) {
                    PlayPoundAnim();
                    hasHit = false;
                    hasPounded = true;
                }

                if (hasPounded && !hasHit) {
                    var animState = anim.GetCurrentAnimatorStateInfo(0);
                    if (animState.shortNameHash == poundAnimNameHash) {
                        if (animState.normalizedTime >= poundNormalizedTime) {
                            OnMochiHit();
                            hasHit = true;
                        }
                    }
                }

                if (Input.GetKeyDown(opposingKey)) {
                    hasPounded = false;
                    if (!hasHit) {
                        anim.Play(poundAnimNameHash, 0, poundNormalizedTime);
                        OnMochiHit();
                        hasHit = true;
                    }
                }
            }
        }

        void OnVictory() {
            hasWon = true;
            PlayPoundAnim();
        }

        void OnMochiHit() {
            mochiAnim.Play("Bump", 0, 0.0f);
            shake.xShake = xShake;
            for (int i = 0; i < planets.Length; i++) {
                planets[i].Shake();
            }
            ctrler.OnHit();
        }

        void PlayPoundAnim() {
            anim.Play(poundAnimNameHash, 0, 0.0f);
        }

    }
}