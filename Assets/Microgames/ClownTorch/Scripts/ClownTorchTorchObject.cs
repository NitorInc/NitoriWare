using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NitorInc.ClownTorch {
    public class ClownTorchTorchObject : MonoBehaviour {

        public GameObject fireEff;
        public ParticleSystem smokeEff;
        public ParticleSystem extinguishEff;
        float requiredTime = 0.5f;
        float timer = 0.0f;
        bool isOnFire = false;

        ClownTorchTorchManager manager;
        // Use this for initialization
        void Start() {
            manager = ClownTorchTorchManager.instance;
            var tag = GetComponent<ClownTorchTag>().type;
            switch (tag) {
                case ClownTorchTag.Type.ClownTorch:
                    requiredTime = manager.ClownTorchRequiredTime;
                    break;
                case ClownTorchTag.Type.PlayerTorch:
                    requiredTime = manager.PlayerTorchRequiredTime;
                    break;
            }
            
        }

        // Update is called once per frame
        void Update() {
            if (isOnFire) {
                timer += Time.deltaTime;
            }

            if (!IsLit()) {
                if (timer >= requiredTime) {
                    fireEff.SetActive(true);
                    smokeEff.Stop();
                    manager.PlayIgniteClip();
                }
            }
        }

        public bool IsLit() {
            return fireEff.activeSelf;
        }

        public void TurnOn() {
            fireEff.SetActive(true);
        }

        public void TurnOff() {
            if (IsLit()) {
                fireEff.SetActive(false);
                timer = 0.0f;
                extinguishEff.Play();
            }
        }

        private void OnTriggerEnter2D(Collider2D col) {
            checkCollision(col, true);
        }

        private void OnTriggerStay2D(Collider2D col)
        {
            checkCollision(col, false);
        }

        private void checkCollision(Collider2D col, bool entering){
            var tag = col.GetComponentInParent<ClownTorchTag>().type;
            switch (tag)
            {
                case ClownTorchTag.Type.Pyre:
                    if (GetComponent<ClownTorchTag>().type != ClownTorchTag.Type.ClownTorch)
                    {
                        smokeEff.Play();
                        isOnFire = true;
                    }
                    break;
                case ClownTorchTag.Type.Water:
                    if (GetComponent<ClownTorchTag>().type != ClownTorchTag.Type.ClownTorch)
                    {
                        isOnFire = false;
                        TurnOff();
                    }
                    break;
                case ClownTorchTag.Type.PlayerTorch:
                    var obj = col.GetComponentInParent<ClownTorchTorchObject>();
                    if (entering && obj.IsLit() && !IsLit())
                    {
                        smokeEff.Play();
                        isOnFire = true;
                    }
                    break;
                case ClownTorchTag.Type.ClownTorch:
                    break;
            }
        }

        private void OnTriggerExit2D(Collider2D col) {
            smokeEff.Stop();
            isOnFire = false;
        }
    }
}