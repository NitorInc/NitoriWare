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
                    currState.OnExit();
                    currState = next;
                    currState.OnEnter();
                } else {
                    currState.Update();
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
}
