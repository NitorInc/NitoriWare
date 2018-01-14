using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NitorInc.Utility;


namespace NitorInc.YuukaWater {

    public class YuukaWaterWaterLauncher : MonoBehaviour {

        public GameObject[] waterDrops;
        public float spawnRadius = 0.15f;
        public Transform leftSpawnPoint;
        public Transform rightSpawnPoint;
        float direction = 1.0f;
        public float waterInterval = 0.1f;
        public float dropVelMin = 0.15f;
        public float dropVelMax = 2.0f;
        public float yuukaVelMult = .5f;
        float yuukaVel = 0.0f;

        Timer spawnTimer;

        // Use this for initialization
        void Start() {
            spawnTimer = TimerManager.NewTimer(waterInterval, Spawn, 0, false, false);
            spawnTimer.Start();

            YuukaWaterController.OnVictory += TurnOff;
        }

        private void Update() {
            if (Input.GetKeyDown(KeyCode.Q)) {
                TurnOff();
            }
            if (Input.GetKeyDown(KeyCode.E)) {
                TurnOn();
            }
        }

        void Spawn() {
            Vector3 fuzz = Random.insideUnitCircle * spawnRadius;
            Vector3 pos = direction > 0.0f ? rightSpawnPoint.position : leftSpawnPoint.position + fuzz;
            var drop = Instantiate(waterDrops[Random.Range(0, waterDrops.Length)], pos, Quaternion.identity).GetComponent<YuukaWaterWaterdrop>();
            drop.transform.parent = transform;
            Vector2 vel = new Vector2(Random.Range(dropVelMin, dropVelMax), 0.0f) * direction;
            drop.SetInitialForce(vel, Vector2.right * yuukaVel);
            spawnTimer.Restart();
        }

        public void TurnOn() {
            spawnTimer.Restart();
        }
        public void TurnOff() {
            spawnTimer.Stop();
        }

        public void SetDirection (float dir) {
            direction = dir;
        }

        public void UpdateYuukaVel (float vel) {
            yuukaVel = vel * yuukaVelMult;
        }
    }
}
