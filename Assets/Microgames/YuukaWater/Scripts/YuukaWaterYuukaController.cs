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
        public ParticleSystem heartParticles;
        public float idleAnimSpeed = 0.3f;
        public float moveAnimSpeed = 1.0f;
        public float leftXBound;
        public float rightXBound;

        float vel = 0f;

        bool movementEnabled = true;

        // Use this for initialization
        void Start() {
            launcher = GetComponentInChildren<YuukaWaterWaterLauncher>();
            moveAnimSpeed = yuukaAnim.speed;
            YuukaWaterController.OnVictory += YuukaSmile;
        }

        // Update is called once per frame
        void Update() {
            float goalVel = 0f;

            if (movementEnabled)
            {
                if (Input.GetKey(KeyCode.LeftArrow))
                    goalVel -= moveSpeed;
                if (Input.GetKey(KeyCode.RightArrow))
                    goalVel += moveSpeed;
            }
            
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

            if (Mathf.Abs(vel) > 0.0f) {
                yuukaAnim.SetFloat("velX", moveAnimSpeed);
            } else {
                yuukaAnim.SetFloat("velX", idleAnimSpeed);
            }
            
            launcher.UpdateYuukaVel(vel);
            transform.Translate(new Vector3(vel, 0.0f, 0.0f) * Time.deltaTime, Space.World);
            if (transform.position.x < leftXBound)
            {
                transform.Translate(Vector2.right * (leftXBound - transform.position.x));
                vel = 0f;
            }
            else if (transform.position.x > rightXBound)
            {
                transform.Translate(Vector2.right * (rightXBound - transform.position.x));
                vel = 0f;
            }
        }

        void YuukaSmile() {
            heartParticles.Play();

            Invoke("disableMovement", .25f);
        }

        void disableMovement()
        {
            movementEnabled = false;
            yuukaAnim.SetTrigger("victory");
            yuukaSprite.sprite = yuukaSmile;
        }
    }

}