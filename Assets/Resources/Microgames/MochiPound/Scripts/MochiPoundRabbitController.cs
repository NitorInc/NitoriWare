using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NitorInc.MochiPound {

    using FSM;

    public class MochiPoundRabbitController : MonoBehaviour {

        Animator anim;
        public float poundNormalizedTime = 0.55f;
        FSM fsm;
        public bool isLeft;
        State readyState;
        State poundingState;
        public Animator mochiAnim;
        CameraShake shake;
        public float xShake = 0.1f;
        public float shakeSpeed = 1.0f;
        MochiPoundPlanet[] planets;
        // Use this for initialization
        void Start() {
            anim = GetComponent<Animator>();
            shake = FindObjectOfType<CameraShake>();
            planets = FindObjectsOfType<MochiPoundPlanet>();
            poundingState = new PoundingState(this);
            readyState = new IdleState().AddTransition(new KeyPressTransition(poundingState, isLeft ? KeyCode.RightArrow : KeyCode.LeftArrow));
            poundingState.AddTransition(new AnimatorTimeTransition(readyState, anim, "Pounding", poundNormalizedTime));
            fsm = new FSM(readyState);
        }

        // Update is called once per frame
        void Update() {
            fsm.Update();
        }

        void OnMochiHit() {
            mochiAnim.Play("Bump", 0, 0.0f);
            shake.xShake = xShake;
            for (int i = 0; i < planets.Length; i++) {
                planets[i].Shake();
            }
        }

        void PlayPoundAnim() {
            anim.Play("Pounding", 0, 0.0f);
        }

        public class PoundingState : State {

            MochiPoundRabbitController agent;
            public PoundingState(MochiPoundRabbitController agent) : base() {
                this.agent = agent;
            }

            public override void OnEnter() {
                agent.PlayPoundAnim();
            }

            public override void OnExit() {
                agent.OnMochiHit();
            }

            public override void Update() {
            }
        }
    }
}