using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NitorInc.ClownTorch {
    public class ClownTorchTorchManager : MonoBehaviour {

        [SerializeField]
        private float clownTorchRequiredTime = 0.5f;
        public float ClownTorchRequiredTime {
            get {
                return clownTorchRequiredTime;
            }
        }
        [SerializeField]
        private float playerTorchRequiredTime = 0.5f;
        public float PlayerTorchRequiredTime {
            get {
                return playerTorchRequiredTime;
            }
        }
    }
}
