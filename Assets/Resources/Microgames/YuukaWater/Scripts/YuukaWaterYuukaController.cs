using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NitorInc.YuukaWater {

    public class YuukaWaterYuukaController : MonoBehaviour {

        public float moveSpeed = 1.0f;
        public float moveAcc = 1.0f;

        public Animator yuukaAnim;
        public YuukaWaterWaterLauncher launcher;
        public SpriteRenderer yuukaSprite;
        public Sprite yuukaSmile;
        public float idleAnimSpeed = 0.3f;
        public float moveAnimSpeed = 1.0f;

        float lastDirection = 1.0f;
        float vel = 0f;

        // Use this for initialization
        void Start() {
            launcher = GetComponentInChildren<YuukaWaterWaterLauncher>();
            moveAnimSpeed = yuukaAnim.speed;
            YuukaWaterController.OnVictory += YuukaSmile;
        }

        // Update is called once per frame
        void Update() {
            //var x = Input.GetAxis("Horizontal");
            float goalVel = 0f;
            if (Input.GetKey(KeyCode.LeftArrow))
                goalVel -= moveSpeed;
            if (Input.GetKey(KeyCode.RightArrow))
                goalVel += moveSpeed;
            
            //Accelerate to goal velocity
            if (vel != goalVel)
            {
                float advance = moveAcc * Time.deltaTime;
                float velDiff = goalVel - vel;
                if (Mathf.Abs(velDiff) <= advance)
                    vel = goalVel;
                else
                    vel += advance * Mathf.Sign(velDiff);
            }

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
            if (Mathf.Abs(vel) > 0.0f) {
                yuukaAnim.SetFloat("velX", moveAnimSpeed);
            } else {
                yuukaAnim.SetFloat("velX", idleAnimSpeed);
            }
            
            launcher.UpdateYuukaVel(vel);
            transform.Translate(new Vector3(vel, 0.0f, 0.0f) * Time.deltaTime, Space.World);
        }

        void YuukaSmile() {
            yuukaSprite.sprite = yuukaSmile;
        }
    }

}