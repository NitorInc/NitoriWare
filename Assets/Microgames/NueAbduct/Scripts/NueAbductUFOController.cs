using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NitorInc.NueAbduct {
    public class NueAbductUFOController : MonoBehaviour {

        public Transform[] spawnPoints;

        [SerializeField]
        Transform suckPoint;
        public Transform SuckPoint {
            get {
                return suckPoint;
            }
        }

        [SerializeField]
        float gracePeriod = 0.2f;
        public float GracePeriod {
            get {
                return gracePeriod;
            }
        }
        [SerializeField]
        float suckTime = 0.5f;
        public float SuckTime {
            get {
                return suckTime;
            }
        }
        [SerializeField]
        float suckSpeed = 1.0f;
        public float SuckSpeed {
            get {
                return suckSpeed;
            }
        }
        [SerializeField]
        float suckAcc = 1.0f;
        public float SuckAcc
        {
            get
            {
                return suckAcc;
            }
        }

        public Animator[] anims;

        // Use this for initialization
        void Start() {

        }

        // Update is called once per frame
        void Update() {
            suckSpeed += suckAcc * Time.deltaTime;
        }

        public void PlaySuckAnimation() {
            for (int i = 0; i < anims.Length; i++) {
                anims[i].Play("Intensify");
            }
        }

        public void PlayIdleAnimation() {
            for (int i = 0; i < anims.Length; i++) {
                anims[i].Play("Idle");
            }
        }
    }
}