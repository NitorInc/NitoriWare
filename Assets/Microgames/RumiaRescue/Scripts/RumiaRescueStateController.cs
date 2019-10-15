using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RumiaRescueStateController : MonoBehaviour {

    private bool isRescued;
    private Transform thisTransform;
    public bool IsRescued {
        get {
            return isRescued;
        }
    }

    void Awake() {
        isRescued = false;
    }

    void Start() {
        thisTransform = transform;
    }

    public bool CanRescueThisOne(Vector3 rescuerPosition,float rescueDistance) {
        print(gameObject.name + " " + (rescuerPosition - thisTransform.position).magnitude);
        if((rescuerPosition - thisTransform.position).sqrMagnitude <= rescueDistance * rescueDistance) {
            isRescued = true;
        } else {
            isRescued = false;
        }
        return isRescued;
    }

	
}
