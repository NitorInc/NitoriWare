using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FoodRoast {
    public class FoodRoast_VictoryController : MonoBehaviour {
        private static FoodRoast_VictoryController _instance = null;
        public static FoodRoast_VictoryController Instance {
            get {
                // Unefficient, but we can't use script execution order.
                if (_instance == null) {
                    _instance = GameObject.Find("Aki").GetComponent<FoodRoast_VictoryController>();
                }
                return _instance;
            }
        }

        private void Awake() {
            _instance = this;
        }

        private Animator rigAnimator;
        private GameObject failure;
        private GameObject victory;

        void Start() {
            rigAnimator = GetComponent<Animator>();
            failure = transform.Find("Failure").gameObject;
            victory = transform.Find("Victory").gameObject;
        }

        public void setVictory(bool victory){
            rigAnimator.speed = 0f;
            
            if (victory) {
                this.victory.SetActive(true);
                MicrogameController.instance.setVictory(true);
            } else {
                this.failure.SetActive(true);
                MicrogameController.instance.setVictory(false);
            }
        }

    }
}