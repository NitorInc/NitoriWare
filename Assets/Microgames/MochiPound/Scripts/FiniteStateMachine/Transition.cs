using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NitorInc.FSM {

    public abstract class Transition {
        public Transition(State next) {
            NextState = next;
        }
        public State NextState { get; }
        public abstract bool IsTransiting();
    }

    public class AnimatorTimeTransition : Transition {

        Animator anim;
        int hash;
        float time;
        public AnimatorTimeTransition(State nextState, Animator anim, string name, float time) : base(nextState) {
            this.anim = anim;
            hash = Animator.StringToHash(name);
            this.time = time;
        }

        public override bool IsTransiting() {
            bool result = false;
            var animState = anim.GetCurrentAnimatorStateInfo(0);
            if (animState.shortNameHash == hash) {
                if (animState.normalizedTime >= time) {
                    result = true;
                }
            }
            return result;
        }
    }

    public class KeyPressTransition : Transition {

        KeyCode key;
        public KeyPressTransition(State nextState, KeyCode key) : base(nextState) {
            this.key = key;
        }

        public override bool IsTransiting() {
            return Input.GetKeyDown(key);
        }
    }


}