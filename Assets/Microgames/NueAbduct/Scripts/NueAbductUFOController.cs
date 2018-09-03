using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NitorInc.NueAbduct {
    public class NueAbductUFOController : MonoBehaviour {

        public Transform[] spawnPoints;

        [SerializeField]
        Transform suckPoint;
        public Transform SuckPoint => suckPoint;

        [SerializeField]
        float gracePeriod = 0.2f;
        public float GracePeriod => gracePeriod;

        [SerializeField]
        float suckTime = 0.5f;
        public float SuckTime => suckTime;

        [SerializeField]
        float suckSpeed = 1.0f;
        public float SuckSpeed => suckSpeed;

        [SerializeField]
        float suckAcc = 1.0f;
        public float SuckAcc => suckAcc;

        public Animator[] anims;

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