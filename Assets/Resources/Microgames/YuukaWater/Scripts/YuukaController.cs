using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NitorInc.YuukaWater {

    public class YuukaController : MonoBehaviour {

        public float moveSpeed = 1.0f;
        public Animator yuukaAnim;
        public YuukaWaterLauncher launcher;
        public SpriteRenderer yuukaSprite;
        public Sprite yuukaSmile;
        public float idleAnimSpeed = 0.3f;
        public float moveAnimSpeed = 1.0f;

        float lastDirection = 1.0f;

        // Use this for initialization
        void Start() {
            launcher = GetComponentInChildren<YuukaWaterLauncher>();
            moveAnimSpeed = yuukaAnim.speed;
            YuukaWaterController.OnVictory += YuukaSmile;
        }

        // Update is called once per frame
        void Update() {
            var x = Input.GetAxis("Horizontal");
            /*
            if (x > 0.0f) {
                if (lastDirection < 0.0f) {
                    yuukaAnim.Play("RotateReversed");
                    lastDirection = 1.0f;
                    launcher.SetDirection(lastDirection);
                }
            } else if (x < 0.0f) {
                if (lastDirection > 0.0f) {
                    yuukaAnim.Play("Rotate");
                    lastDirection = -1.0f;
                    launcher.SetDirection(lastDirection);
                }
            }
            */
            if (Mathf.Abs(x) > 0.0f) {
                yuukaAnim.SetFloat("velX", moveAnimSpeed);
            } else {
                yuukaAnim.SetFloat("velX", idleAnimSpeed);
            }

            float vel = x * moveSpeed;
            //launcher.UpdateYuukaVel(vel);
            transform.Translate(new Vector3(vel, 0.0f, 0.0f) * Time.deltaTime, Space.World);
        }

        void YuukaSmile() {
            yuukaSprite.sprite = yuukaSmile;
        }
    }

}