using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NitorInc.FSM {

    public abstract class State {
        public State() {
            transitions = new List<Transition>();
        }
        private List<Transition> transitions;
        public State IsTransiting() {
            State next = this;
            for (int i = 0; i < transitions.Count; i++) {
                if (transitions[i].IsTransiting()) {
                    next = transitions[i].NextState;
                    break;
                }
            }
            return next;
        }
        public State AddTransition(Transition trans) {
            transitions.Add(trans);
            return this;
        }
        public abstract void OnEnter();
        public abstract void Update();
        public abstract void OnExit();
    }

    public class IdleState : State {
        public override void OnEnter() {
        }

        public override void OnExit() {
        }

        public override void Update() {
        }
    }

}