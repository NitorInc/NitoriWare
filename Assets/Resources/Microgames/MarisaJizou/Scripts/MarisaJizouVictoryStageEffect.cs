using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NitorInc.Utility;

namespace NitorInc.MarisaJizou {
    public class MarisaJizouVictoryStageEffect : MonoBehaviour {

        public float delay = 0.2f;
        public GameObject animObject;

        Timer timer;

        // Use this for initialization
        void Start() {
            timer = TimerManager.NewTimer(delay, PlayEffects, 0, false, false);
        }

        void PlayEffects() {
            animObject.SetActive(true);
        }

        // Update is called once per frame
        void Update() {
            if (MicrogameController.instance.getVictory()) {
                timer.StartOnce();
            }
        }
    }
}
