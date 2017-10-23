using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NitorInc.FSM {

    public class FSM {

        public FSM(State initState) {
            currState = initState;
        }

        public bool enabled = true;

        State currState;

        public void Update() {
            if (enabled) {
                State next = currState.IsTransiting();
                if (next != currState) {
                    currState.Update();
                } else {
                    currState.OnExit();
                    currState = next;
                    currState.OnEnter();
                }
            }
        }

        public void SetState(State next) {
            if (next != currState) {
                currState.OnExit();
                currState = next;
                currState.OnEnter();
            }
        }
    }

    public abstract class State {
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

    public abstract class Transition {
        public Transition(State nextState) {
            this.nextState = nextState;
        }
        private State nextState;
        public State NextState {
            get {
                return nextState;
            }
        }
        public abstract bool IsTransiting();
    }
}
