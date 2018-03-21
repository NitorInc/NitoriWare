using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NitorInc.MochiPound {

    public class MochiPoundRabbitController : MonoBehaviour {

        Animator anim;
        public float poundNormalizedTime = 0.55f;
        public float windupNormalizedTime = 0.8f;
        bool hasHit = false;
        public bool HasHit {
            get {
                return hasHit;
            }
        }
        bool hasWon = false;

        public Animator mochiAnim;
        CameraShake shake;
        public float xShake = 0.1f;
        public float shakeSpeed = 1.0f;
        MochiPoundPlanet[] planets;
        MochiPoundController ctrler;
        public Hit opposingSide;

        public MochiPoundArrowKey button;

        public string poundAnimName = "Pounding";
        int poundAnimNameHash;
        public string windupAnimName = "Windup";
        int windupAnimNameHash;
        public string idleAnimName = "Idle";
        int idleAnimNameHash;

        public float startAnimationSpeed = 2.5f;
        public float finishAnimationSpeed = 0.7f;

        // Use this for initialization
        void Start() {
            anim = GetComponent<Animator>();
            shake = FindObjectOfType<CameraShake>();
            planets = FindObjectsOfType<MochiPoundPlanet>();
            ctrler = FindObjectOfType<MochiPoundController>();
            poundAnimNameHash = Animator.StringToHash(poundAnimName);
            windupAnimNameHash = Animator.StringToHash(windupAnimName);
            idleAnimNameHash = Animator.StringToHash(idleAnimName);
            anim.speed = startAnimationSpeed;
        }

        // Update is called once per frame
        void Update() {
            if (!hasWon) {
                UpdateHit();
            }
        }

        void UpdateHit() {
            if (!hasHit) {
                var animState = anim.GetCurrentAnimatorStateInfo(0);
                if (animState.shortNameHash == poundAnimNameHash) {
                    if (animState.normalizedTime >= poundNormalizedTime && animState.normalizedTime < 1.0f) {
                        OnMochiHit();
                        hasHit = true;
                    }
                }
            }
        }

        public void ResetStatus() {
            hasHit = false;
        }

        public void ShowButton(bool active) {
            button.SetActive(active);
        }

        public bool IsAnimationFinished {
            get {
                var animState = anim.GetCurrentAnimatorStateInfo(0);
                if (animState.shortNameHash == idleAnimNameHash) {
                    return true;
                }
                if (animState.shortNameHash == windupAnimNameHash) {
                    if (animState.normalizedTime >= windupNormalizedTime) {
                        return true;
                    } else {
                        return false;
                    }
                }
                if (animState.shortNameHash == poundAnimNameHash) {
                    if (animState.normalizedTime >= poundNormalizedTime) {
                        return true;
                    }
                    else {
                        return false;
                    }
                }
                return false;
            }
        }

        public bool IsWindingUp() {
            var animState = anim.GetCurrentAnimatorStateInfo(0);
            if (animState.shortNameHash == windupAnimNameHash) {
                if (animState.normalizedTime < windupNormalizedTime) {
                    return true;
                } else {
                    return false;
                }
            }
            return false;
        }

        public void Pound() {
            //PlayPoundImmediate();
            PlayPoundAnim();
            ShowButton(false);
        }
        public void Disable() {
            hasHit = true;
            ShowButton(false);
        }


        public void Windup() {
            PlayWindup();
        }

        public void OnVictory() {
            anim.speed = finishAnimationSpeed;
            hasWon = true;
            PlayPoundAnim();
            ShowButton(false);
        }

        void OnMochiHit() {
            mochiAnim.Play("Bump", 0, 0.0f);
            shake.xShake = xShake;
            for (int i = 0; i < planets.Length; i++) {
                planets[i].Shake();
            }
            ctrler.OnHit();
        }

        void PlayWindup() {
            anim.Play(windupAnimNameHash, 0, 0.0f);
        }

        void PlayPoundAnim() {
            anim.Play(poundAnimNameHash, 0, 0.0f);
        }

        void PlayPoundImmediate() {
            anim.Play(poundAnimNameHash, 0, poundNormalizedTime);
        }

    }
}