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
        public List<GameObject> kasaStack;

        bool hasTurned = false;

        int hatsCarried = 3;
        int dropCounter = 0;

        public float upMagnitude = 1.7f;

        public SpriteRenderer marisaSprite;
        Vector3 direction = Vector3.right;

        public delegate void OnAction();
        public static event OnAction onTurning;

        public AudioClip[] hatDropClip;

        // Use this for initialization
        void Start() {
            hatsCarried = FindObjectOfType<MarisaJizouController>().hatsCarried;
            var total = kasaStack.Count;
            for (int i = 0; i < total - hatsCarried; i++) {
                kasaStack[0].gameObject.SetActive(false);
                kasaStack.RemoveAt(0);
            }
        }

        // Update is called once per frame
        void Update() {
            if (!MicrogameController.instance.getVictoryDetermined()) {
                
                transform.Translate(direction * moveSpeed * Time.deltaTime);

                if (transform.position.x <= leftBound || transform.position.x >= rightBound) {
                    if (!hasTurned) {
                        marisaSprite.flipX = !marisaSprite.flipX;
                        direction *= -1.0f;
                        hasTurned = true;
                        if (onTurning != null)
                            onTurning();
                    }
                } else {
                    hasTurned = false;
                }

                if (dropCounter < hatsCarried) {
                    if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.DownArrow)) {
                        Instantiate(kasaProto, kasaStack[dropCounter].transform.position, Quaternion.identity);
                        kasaStack[dropCounter].SetActive(false);
                        MicrogameController.instance.playSFX(hatDropClip[dropCounter], MicrogameController.instance.getSFXSource().panStereo);
                        dropCounter++;
                    }
                }

            } else {
                transform.Translate(direction * finishSpeed * Time.deltaTime);
            }
        }

        private void OnDestroy() {
            onTurning = null;
        }
    }
}