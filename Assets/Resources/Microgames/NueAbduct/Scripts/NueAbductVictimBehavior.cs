﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NitorInc.Utility;

namespace NitorInc.NueAbduct {

    public class NueAbductVictimBehavior : MonoBehaviour {


        Vector2 targetPos;
        Animator anim;
        Vibrate vib;

        public NueAbductUFOController ufo;

        public float wanderRadius = 0.3f;
        public float wanderSpeed = 0.5f;
        public float decisionTimeMin = 0.5f;
        public float decisionTimeMax = 1.5f;

        public enum State {
            Idle,
            Wander,
            Sucking,
            Sucked
        }

        public State currState;

        Timer wanderTimer;
        Timer graceTimer;
        Timer suckTimer;

        // Use this for initialization
        void Start() {
            anim = GetComponentInChildren<Animator>();
            vib = GetComponentInChildren<Vibrate>();
            targetPos = transform.position;
            wanderTimer = TimerManager.NewTimer(1.0f, Wander, 0, false, false);
            graceTimer = TimerManager.NewTimer(ufo.GracePeriod, SuckFail, 0, false, false);
            suckTimer = TimerManager.NewTimer(ufo.SuckTime, SuckSucceed, 0, false, false);
            SetState(State.Wander);
        }

        void Wander() {
            targetPos = Random.insideUnitCircle * wanderRadius + new Vector2(transform.position.x, transform.position.y);
            wanderTimer.SetTime(Random.Range(decisionTimeMin, decisionTimeMax));
            wanderTimer.Start();
        }

        // Update is called once per frame
        void Update() {
            UpdateState();
        }

        void SetState(State nextState) {
            if (nextState != currState) {
                ExitState();
                EnterState(nextState);
            }
        }

        void EnterState(State nextState) {
            switch (nextState) {
                case State.Idle:
                    break;
                case State.Wander:
                    wanderTimer.Restart();
                    break;
                case State.Sucked:
                    anim.Play("Sucked");
                    break;
                case State.Sucking:
                    suckTimer.Restart();
                    vib.enabled = true;
                    ufo.PlaySuckAnimation();
                    break;
            }
            currState = nextState;
        }

        void UpdateState() {
            switch (currState) {
                case State.Idle:
                    break;
                case State.Wander:
                    transform.position = Vector2.MoveTowards(transform.position, targetPos, wanderSpeed * Time.deltaTime);
                    break;
                case State.Sucked:
                    transform.position = Vector2.MoveTowards(transform.position, ufo.SuckPoint.position, ufo.SuckSpeed * Time.deltaTime);
                    break;
                case State.Sucking:
                    break;
            }
        }

        void ExitState() {
            switch (currState) {
                case State.Idle:
                    break;
                case State.Wander:
                    wanderTimer.Stop();
                    break;
                case State.Sucked:
                    break;
                case State.Sucking:
                    vib.enabled = false;
                    suckTimer.Stop();
                    graceTimer.Stop();
                    ufo.PlayIdleAnimation();
                    break;
            }
        }

        void SuckFail() {
            SetState(State.Wander);
        }

        void SuckSucceed() {
            SetState(State.Sucked);
        }

        void SuckedAnimation() {
            anim.Play("Sucked");
        }

        void OnTriggerEnter2D(Collider2D other) {
            if (other.GetComponentInChildren<NueAbductVictimBehavior>() == null) {
                if (currState == State.Sucked) {
                    Destroy(gameObject);
                } else if (!other.name.Contains("Succ")) {
                    SetState(State.Sucking);
                }
            }
        }

        void OnTriggerExit2D(Collider2D other) {
            if (other.GetComponentInChildren<NueAbductVictimBehavior>() == null) {
                if (currState == State.Sucking) {
                    if (!other.name.Contains("Succ")) {
                        graceTimer.Restart();
                    }
                }
            }
        }

        private void OnDestroy() {
            wanderTimer.Stop();
            graceTimer.Stop();
            suckTimer.Stop();
        }
    }
}