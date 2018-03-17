using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NitorInc.ClownTorch {
    public class ClownTorchSpawnInRect : MonoBehaviour {

        public GameObject objToSpawn;
        public Rect range;

        // Use this for initialization
        void Start() {
            Vector3 target = Vector3.zero;
            target.x = Random.Range(range.xMin, range.xMax);
            target.y = Random.Range(range.yMin, range.yMax);
            Instantiate(objToSpawn, target, Quaternion.identity, transform);
        }
    }
}
