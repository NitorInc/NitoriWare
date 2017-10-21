using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NitorInc.Utility;

namespace NitorInc.MarisaJizou {

    public class MarisaJizouMarisaController : MonoBehaviour {

        public float leftBound;
        public float rightBound;
        public float moveSpeed = 5.0f;
        public float finishSpeed = 10.0f;

        public Transform kasaSnapPoint;
        public GameObject kasaProto;
        public GameObject kasaDummy;
        public List<GameObject> kasaStack;

        bool hasTurned = false;

        public int dropLimit = 3;
        int dropCounter = 0;
        public float dropInterval = 0.24f;

        Timer dropTimer;

        // Use this for initialization
        void Start() {
            dropTimer = TimerManager.NewTimer(dropInterval, ResetKasa, 0, false, false);
        }

        void ResetKasa() {
            if (dropCounter < dropLimit) {
                kasaDummy.SetActive(true);
                kasaStack[dropCounter - 1].SetActive(false);
            }
        }

        // Update is called once per frame
        void Update() {
            if (!MicrogameController.instance.getVictoryDetermined()) {

                transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);

                if (transform.position.x <= leftBound || transform.position.x >= rightBound) {
                    if (!hasTurned) {
                        transform.Rotate(Vector3.up, 180.0f);
                        hasTurned = true;
                    }
                } else {
                    hasTurned = false;
                }
            } else {
                transform.Translate(Vector3.right * finishSpeed * Time.deltaTime);
            }

            if (kasaDummy.activeSelf) {
                if (dropCounter < dropLimit) {
                    if (Input.GetMouseButtonDown(0)) {
                        Instantiate(kasaProto, kasaSnapPoint.position, Quaternion.identity);
                        dropCounter++;
                        kasaDummy.SetActive(false);
                        dropTimer.Start();
                    }
                }
            }
        }

        private void OnDestroy() {
            dropTimer.Stop();
        }
    }
}