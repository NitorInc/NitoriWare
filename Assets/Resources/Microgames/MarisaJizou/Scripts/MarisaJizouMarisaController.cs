using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NitorInc.Utility;

namespace NitorInc.MarisaJizou {

    public class MarisaJizouMarisaController : MonoBehaviour {

        public float leftBound;
        public float rightBound;
        float speed;
        public float moveSpeed = 5.0f;
        public float dropMoveSpeed = 1.75f;
        public float finishSpeed = 10.0f;

        public Transform kasaSnapPoint;
        public GameObject kasaProto;
        public GameObject kasaDummy;
        public List<GameObject> kasaStack;

        bool hasTurned = false;

        public int dropLimit = 3;
        int dropCounter = 0;
        public float dropInterval = 0.24f;

        public float upMagnitude = 1.7f;

        public Animator tiltAnim;

        Timer dropTimer;

        // Use this for initialization
        void Start() {
            dropTimer = TimerManager.NewTimer(dropInterval, ResetKasa, 0, false, false);
            speed = moveSpeed;
        }

        void ResetKasa() {
            speed = moveSpeed;
            if (dropCounter < dropLimit) {
                kasaDummy.SetActive(true);
                kasaStack[dropCounter - 1].SetActive(false);
            }
        }

        // Update is called once per frame
        void Update() {
            if (!MicrogameController.instance.getVictoryDetermined()) {

                transform.Translate(Vector3.right * speed * Time.deltaTime);

                if (transform.position.x <= leftBound || transform.position.x >= rightBound) {
                    if (!hasTurned) {
                        transform.Rotate(Vector3.up, 180.0f);
                        hasTurned = true;
                    }
                } else {
                    hasTurned = false;
                }
            } else {
                var dir = transform.position.z <= 0.0f ? Vector3.right : Vector3.left;
                dir.y = upMagnitude;
                transform.Translate(dir * finishSpeed * Time.deltaTime);
            }

            if (kasaDummy.activeSelf) {
                if (dropCounter < dropLimit) {
                    if (Input.GetMouseButtonDown(0)) {
                        Instantiate(kasaProto, kasaSnapPoint.position, Quaternion.identity);
                        dropCounter++;
                        kasaDummy.SetActive(false);
                        dropTimer.Start();
                        speed = dropMoveSpeed;
                        tiltAnim.Play("Tilt", 0, 0.0f);
                    }
                }
            }
        }

        private void OnDestroy() {
            dropTimer.Stop();
        }
    }
}