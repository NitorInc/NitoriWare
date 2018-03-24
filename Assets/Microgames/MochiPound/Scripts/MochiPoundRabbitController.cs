using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NitorInc.Utility;

namespace NitorInc.MochiPound {

    public class MochiPoundRabbitController : MonoBehaviour {

        Animator anim;

        public Animator mochiAnim;
        CameraShake shake;
        public float xShake = 0.1f;
        public float shakeSpeed = 1.0f;
        MochiPoundPlanet[] planets;
        MochiPoundController ctrler;

        public float rabbitAnimSpeed;
        // total elapsed time of Pound clip
        public float poundAnimTime = 0.25f;
        float PoundAnimTime {
            get {
                return poundAnimTime / rabbitAnimSpeed;
            }
        }
        // time needed for hammer to hit
        public float poundHitTime = 0.15f;
        float PoundHitTime {
            get {
                return poundHitTime / rabbitAnimSpeed;
            }
        }
        // total elapsed time of Windup clip
        public float windupAnimTime = 0.25f;
        float WindupAnimTime {
            get {
                return windupAnimTime / rabbitAnimSpeed;
            }
        }
        Timer poundTimer;
        Timer windupTimer;

        public KeyCode inputKey = KeyCode.LeftArrow;
        public MochiPoundRabbitController opposingRabbit;

        public MochiPoundArrowKey button;

        public AudioClip poundClip;

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
            ApplyAnimationSpeed();
            shake = FindObjectOfType<CameraShake>();
            planets = FindObjectsOfType<MochiPoundPlanet>();
            poundTimer = TimerManager.NewTimer(PoundAnimTime, OnPoundFinish, 0, false, false);
            windupTimer = TimerManager.NewTimer(WindupAnimTime, OnWindupFinish, 0, false, false);
            ctrl = FindObjectOfType<MochiPoundController>();
            
            if (inputKey == KeyCode.RightArrow) {
                readyToPound = false;
            }
        }

        void ApplyAnimationSpeed() {
            anim.speed *= rabbitAnimSpeed;
            mochiAnim.speed *= rabbitAnimSpeed;
        }

        public void SetAnimationSpeed(float speed) {
            anim.speed = speed;
            mochiAnim.speed = speed;
        }

        public void PrepareToFinish() {
            poundTimer.Stop();
            windupTimer.Stop();
            readyToPound = false;
            FinishPound();
        }

        void FinishPound() {
            SetAnimationSpeed(ctrl.finishAnimSpeed);
            anim.Play(poundAnimName, 0, 0.0f);
            ctrl.PrepareToStartFinalSequence(PoundHitTime);
        }

        public void PlayPoundSound() {
            MicrogameController.instance.playSFX(poundClip, MicrogameController.instance.getSFXSource().panStereo);
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
            else
            {
                MicrogameController.instance.setVictory(true);
                ShowButton(false);
            }
        }

        public void ShowButton(bool active) {
            button.SetActive(active);
        }

        public void Pound() {
            readyToPound = false;
            anim.Play(poundAnimName);
            TimerManager.NewTimer(PoundHitTime, OnMochiHit, 0);
            poundTimer.Restart(PoundAnimTime);
        }

        public void Windup() {
            anim.Play(windupAnimName);
            windupTimer.Restart(WindupAnimTime);
        }

        void OnMochiHit() {
            ctrl.CountHit();
            if (!ctrl.IsLastHit) {
                opposingRabbit.Windup();
                mochiAnim.Play("Bump", 0, 0.0f);
                shake.xShake = xShake;
                for (int i = 0; i < planets.Length; i++) {
                    planets[i].Shake();
                }
                PlayPoundSound();
            } else {
                PrepareToFinish();
                opposingRabbit.PrepareToFinish();
            }

        }
        
    }
}