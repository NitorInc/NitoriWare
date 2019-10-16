using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RumiaRescuePositionModifier : MonoBehaviour {

    [SerializeField]
    private float factorY2Z = 0.1f;

    private Transform thisTransform;

    void Start() {
        thisTransform = transform;
    }

	void LateUpdate() {
        Vector3 beforePosition = thisTransform.position;
        beforePosition.z = factorY2Z * beforePosition.y;
        thisTransform.position = beforePosition;
    }
}
