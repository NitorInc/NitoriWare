using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NitorInc.Utility;

namespace NitorInc.ClownTorch {
    public class ClownTorchWander : MonoBehaviour {

        public Rect rect;
        public float moveSpeed;
        public float winMoveSpeed;
        public float waitTime = 0.5f;

        Vector2 target;

        Animator anim;

        // Use this for initialization
        void Start() {
            target = transform.position;
            anim = GetComponent<Animator>();
            TimerManager.NewTimer(waitTime, Idle, 0, true, true);
        }

        void Wander() {
            if (!MicrogameController.instance.getVictory())
            {
                target.x = Random.Range(rect.xMin, rect.xMax);
                target.y = Random.Range(rect.yMin, rect.yMax);
                anim.Play("Walk");
                TimerManager.NewTimer(waitTime, Idle, 0, true, true);
            }
        }

        void Idle() {
            if (!MicrogameController.instance.getVictory()) {
                anim.Play("Idle");
                TimerManager.NewTimer(waitTime, Wander, 0, true, true);
            }
            else
                anim.Play("Idle");
        }

        // Update is called once per frame
        void Update() {
            if (MicrogameController.instance.getVictory()) {
                anim.Play("Idle");
                //anim.Play("Walk");
                //transform.position += Vector3.right * winMoveSpeed * Time.deltaTime;
            } else {
                transform.position = Vector2.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
                Vector2 pos = transform.position;
                if (pos == target) {
                    anim.Play("Idle");
                }
            }
        }
    }
}