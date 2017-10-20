using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NitorInc.YuukaWater {

    public class YuukaWaterFlower : MonoBehaviour {

        Animator anim;
        Collider2D[] cols;
        string[] fullNames = { "PlantFull1", "PlantFull2", "PlantFull3"};
        int[] fullIds;
        public int flowerIndex;
        public float secondStageThreshold = 0.2f;
        YuukaWaterController ctrl;

        private void Start() {
            anim = GetComponent<Animator>();
            cols = GetComponentsInChildren<Collider2D>();
            fullIds = new int[fullNames.Length];
            for (int i = 0; i < fullNames.Length; i++) {
                fullIds[i] = Animator.StringToHash(fullNames[i]);
            }
            ctrl = FindObjectOfType<YuukaWaterController>();
        }

        enum State {
            stage1,
            stage2,
            stage3,
            finished
        }

        State currState;

        public float totalWaterRequired = 50;

        private void Update() {
            UpdateState();
        }

        void EnterState() {
            switch(currState) {
                case State.stage1:
                    break;
                case State.stage2:
                    anim.Play("PlantFlip1");
                    cols[0].enabled = false;
                    cols[1].enabled = true;
                    break;
                case State.stage3:
                    anim.Play("PlantFlip2");
                    cols[1].enabled = false;
                    ctrl.Notify();
                    break;
                case State.finished:
                    break;
            }
        }

        void UpdateState() {
            switch (currState) {
                case State.stage1:
                    break;
                case State.stage2:
                    break;
                case State.stage3:
                    var state = anim.GetCurrentAnimatorStateInfo(0);
                    if (state.normalizedTime >= 1.0f) {
                        //int index = Random.Range(0, fullIds.Length);
                        int index = flowerIndex;
                        anim.Play(fullIds[index]);
                        SetState(State.finished);
                    }
                    break;
                case State.finished:
                    break;
            }
        }

        void ExitState() {
            switch (currState) {
                case State.stage1:
                    break;
                case State.stage2:
                    break;
                case State.stage3:
                    break;
                case State.finished:
                    break;
            }
        }

        void SetState(State next) {
            if (currState != next) {
                ExitState();
                currState = next;
                EnterState();
            }
        }


        float SecondStage {
            get {
                return Mathf.Floor(totalWaterRequired * secondStageThreshold);
            }
        }
        float waterCounter = 0;

        void OnTriggerEnter2D(Collider2D other) {
            waterCounter += 1.0f;
            if (waterCounter >= totalWaterRequired) {
                SetState(State.stage3);
            }
            else if (waterCounter >= SecondStage) {
                SetState(State.stage2);
            }
            
        }
    }
}
