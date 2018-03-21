using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NitorInc.Utility;

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

        public float poundAnimTime = 0.25f;
        public float poundHitTime = 0.15f;
        public float windupAnimTime = 0.25f;
        Timer poundTimer;
        Timer windupTimer;

        public KeyCode inputKey = KeyCode.LeftArrow;
        public MochiPoundRabbitController opposingRabbit;

        public MochiPoundArrowKey button;

        public string poundAnimName = "Pounding";
        int poundAnimNameHash;
        public string windupAnimName = "Windup";
        int windupAnimNameHash;
        public string idleAnimName = "Idle";
        int idleAnimNameHash;

        bool readyToPound = true;

        MochiPoundController ctrl;

        // Use this for initialization
        void Start() {
            anim = GetComponent<Animator>();
            shake = FindObjectOfType<CameraShake>();
            planets = FindObjectsOfType<MochiPoundPlanet>();
            poundTimer = TimerManager.NewTimer(poundAnimTime, OnPoundFinish, 0, false, false);
            windupTimer = TimerManager.NewTimer(windupAnimTime, OnWindupFinish, 0, false, false);
            ctrl = FindObjectOfType<MochiPoundController>();
            
            if (inputKey == KeyCode.RightArrow) {
                readyToPound = false;
            }
        }

        public void PrepareToFinish() {
            poundTimer.Stop();
            windupTimer.Stop();
            readyToPound = false;
            Invoke("FinishPound", ctrl.finishPoundAnimTime);
        }

        void FinishPound() {
            anim.Play(poundAnimName, 0, 0.0f);
            ctrl.PrepareToStartFinalSequence();
        }

        public void SetReady(bool ready = true) {
            readyToPound = true;
        }

        public bool IsPounding() {
            return poundTimer.IsRunning();
        }

        public bool IsWindingUp() {
            return windupTimer.IsRunning();
        }

        void OnPoundFinish() {
        }

        void OnWindupFinish() {
            readyToPound = true;
        }

        // Update is called once per frame
        void Update() {
            if (!ctrl.IsLastHit) {
                ShowButton(readyToPound);
                if (Input.GetKeyDown(inputKey)) {
                    if (readyToPound) {
                        Pound();
                    }
                }
            }
            else {
                ShowButton(false);
            }
        }

        public void ShowButton(bool active) {
            button.SetActive(active);
        }

        public void Pound() {
            readyToPound = false;
            anim.Play(poundAnimName);
            TimerManager.NewTimer(poundHitTime, OnMochiHit, 0);
            poundTimer.Restart(poundAnimTime);
        }

        public void Windup() {
            anim.Play(windupAnimName);
            windupTimer.Restart(windupAnimTime);
        }

        void OnMochiHit() {
            ctrl.CountHit();
            if (!ctrl.IsLastHit) {
                opposingRabbit.Windup();
            } else {
                PrepareToFinish();
                opposingRabbit.PrepareToFinish();
            }
            mochiAnim.Play("Bump", 0, 0.0f);
            shake.xShake = xShake;
            for (int i = 0; i < planets.Length; i++) {
                planets[i].Shake();
            }
        }

    }
}