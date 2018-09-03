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
        float PoundAnimTime => poundAnimTime / rabbitAnimSpeed;

        // time needed for hammer to hit
        public float poundHitTime = 0.15f;
        float PoundHitTime => poundHitTime / rabbitAnimSpeed;

        // total elapsed time of Windup clip
        public float windupAnimTime = 0.25f;
        float WindupAnimTime => windupAnimTime / rabbitAnimSpeed;

        public MochiPoundArrowKey button;

        public AudioClip poundClip;
        public AudioClip mistakeClip;

        public string poundAnimName = "Pounding";
        public string windupAnimName = "Windup";
        public string idleAnimName = "Idle";
        public string shakeAnimName = "Shake";

        MochiPoundController ctrl;

        // Use this for initialization
        void Start() {
            anim = GetComponent<Animator>();
            ApplyAnimationSpeed();
            shake = FindObjectOfType<CameraShake>();
            planets = FindObjectsOfType<MochiPoundPlanet>();
            ctrl = FindObjectOfType<MochiPoundController>();
        }

        void ApplyAnimationSpeed() {
            anim.speed *= rabbitAnimSpeed;
            mochiAnim.speed *= rabbitAnimSpeed;
        }

        public void SetAnimationSpeed(float speed) {
            anim.speed = speed;
            mochiAnim.speed = speed;
        }

        public void FinishPound() {
            SetAnimationSpeed(ctrl.finishAnimSpeed);
            anim.Play(poundAnimName, 0, 0.0f);
        }

        public void PlayPoundSound() {
            MicrogameController.instance.playSFX(poundClip, MicrogameController.instance.getSFXSource().panStereo);
        }

        public void ShowButton(bool active) {
            button.SetActive(active);
        }

        public void Pound() {
            anim.Play(poundAnimName, 0, 0.0f);
            Invoke("OnMochiHit", PoundHitTime);
        }

        public void Windup() {
            anim.Play(windupAnimName, 0, 0.0f);
        }

        void OnMochiHit() {
            PlanetBump();
            MochiBump();
            ctrl.CountHit();
        }

        public void OnMistake() {
            MicrogameController.instance.playSFX(mistakeClip, MicrogameController.instance.getSFXSource().panStereo);
            button.OnMistake();
            anim.Play(shakeAnimName, 0, 0.0f);
        }

        void PlanetBump() {
            shake.xShake = xShake;
            for (int i = 0; i < planets.Length; i++) {
                planets[i].Shake();
            }
        }

        void MochiBump() {
            mochiAnim.Play("Bump", 0, 0.0f);
            PlayPoundSound();
        }
        
    }
}