using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MochiPoundController : MonoBehaviour {

    public Animator leftKineAnim;
    public Animator rightKineAnim;
    public Animator mochiAnim;

    public float screenShake = 5.0f;

    public float kineHitFrame = 50.0f;
    public float kineTotalFrames = 60.0f;
    float normalizedHitTime {
        get {
            return kineHitFrame / kineTotalFrames;
        }
    }

    public float kineSpeed = 1.0f;

    enum State { Start, LeftHitting, LeftDown, RightHitting, RightDown, Victory};
    State currState;

    int hitCounter = 0;
    public int hitGoal = 3;

    public delegate void OnMochiHit();
    public event OnMochiHit onHit;

	// Use this for initialization
	void Start () {
        currState = State.Start;
        hitCounter = 0;
        leftKineAnim.speed = kineSpeed;
        rightKineAnim.speed = kineSpeed;
        onHit += CountHit;
    }
	
	// Update is called once per frame
	void Update () {
        if (hitCounter >= hitGoal) {
            SetNextState(State.Victory);
        }
        ResetAnimator(leftKineAnim);
        ResetAnimator(rightKineAnim);
        ResetAnimator(mochiAnim);
        switch (currState) {
            case State.Start:
                if (Input.GetKeyDown(KeyCode.LeftArrow)) {
                    SetNextState(State.RightHitting);
                } else if (Input.GetKeyDown(KeyCode.RightArrow)) {
                    SetNextState(State.LeftHitting);
                }
                break;
            case State.LeftHitting:
                if (HasHit(leftKineAnim)) {
                    SetNextState(State.LeftDown);
                }
                break;
            case State.RightHitting:
                if (HasHit(rightKineAnim)) {
                    SetNextState(State.RightDown);
                }
                break;
            case State.LeftDown:
                if (Input.GetKeyDown(KeyCode.LeftArrow)) {
                    SetNextState(State.RightHitting);
                }
                break;
            case State.RightDown:
                if (Input.GetKeyDown(KeyCode.RightArrow)) {
                    SetNextState(State.LeftHitting);
                }
                break;
            case State.Victory:
                break;
        }
	}

    void PlayPoundAnim(bool isLeft) {
        if (isLeft)
            leftKineAnim.Play("Pound");
        else
            rightKineAnim.Play("Pound");
    }

    void PlayMochiAnim() {
        mochiAnim.Play("Bump");
    }

    void EnterState(State nextState) {
        switch (nextState) {
            case State.Start:
                break;
            case State.LeftHitting:
                PlayPoundAnim(true);
                break;
            case State.RightHitting:
                PlayPoundAnim(false);
                break;
            case State.LeftDown:
                onHit();
                break;
            case State.RightDown:
                onHit();
                break;
            case State.Victory:
                MicrogameController.instance.setVictory(true, true);
                break;
        }
    }

    void CountHit() {
        hitCounter += 1;
        PlayMochiAnim();
        CameraShake.instance.setScreenShake(screenShake);
    }

    void SetNextState(State nextState) {
        ExitState();
        EnterState(nextState);
        currState = nextState;
    }

    void ExitState() {
        switch (currState) {
            case State.Start:
                break;
            case State.LeftHitting:
                break;
            case State.RightHitting:
                break;
            case State.LeftDown:
                break;
            case State.RightDown:
                break;
            case State.Victory:
                break;
        }
    }

    void ResetAnimator(Animator anim) {
        var stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.normalizedTime >= 1.0f && !stateInfo.IsName("Idle")) {
            anim.Play("Idle");
        }
    }

    bool HasHit(Animator kineAnim) {
        bool result = false;
        var stateInfo = kineAnim.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.IsName("Pound") && stateInfo.normalizedTime >= normalizedHitTime) {
            result = true;
        }
        return result;
    }

    void OnDestroy() {
        onHit = null;
    }
}

namespace MochiUti {
    public class FSM {
        public class State {
            public virtual void OnEnter() { }
            public virtual void Update() { }
            public virtual void OnExit() { }
        }

        State initState;
        State currState;
        bool bActive;

        public FSM(State initState) {
            this.initState = initState;
            bActive = false;
        }

        public void Start() {
            initState.OnEnter();
            currState = initState;
            bActive = true;
        }

        public void Update() {
            if (bActive)
                currState.Update();
        }

        public void SetState(State state) {
            currState.OnExit();
            currState = state;
            currState.OnEnter();
        }
    }
}