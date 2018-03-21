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

        public GameObject victorySequence;
        public float sequenceStartingDelay = 0.25f;
        public GameObject[] objsToDisableOnVictory;
        

        ClownTorchTorchObject torch;

        bool hasWon = false;
        public bool HasWon {
            get {
                return hasWon;
            }
        }

        void Start() {
            var torches = FindObjectsOfType<ClownTorchTorchObject>();
            for (int i = 0; i < torches.Length; i++) {
                if (torches[i].GetComponent<ClownTorchTag>().type == ClownTorchTag.Type.ClownTorch) {
                    torch = torches[i];
                }
            }
        }

        void Update() {
            if (torch.IsLit()) {
                SetVictory();
            }
        }

        public void SetVictory() {
            if (!hasWon) {
                for (int i = 0; i < objsToDisableOnVictory.Length; i++) {
                    objsToDisableOnVictory[i].SetActive(false);
                }
                MicrogameController.instance.setVictory(true);
                victorySequence.SetActive(true);
                hasWon = true;
            }
        }
    }
}
