using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RumiaRescueStateController : MonoBehaviour {

    private bool isRescued;
    private Transform thisTransform;
    private Animator ani;
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
        ani = GetComponent<Animator>();
    }

    public bool CanRescueThisOne(Vector3 rescuerPosition,float rescueDistance) {
        if((rescuerPosition - thisTransform.position).sqrMagnitude <= rescueDistance * rescueDistance) {
            isRescued = true;
        } else {
            isRescued = false;
        }
        ani.SetBool("IsRescued", isRescued);
        return isRescued;
    }

	
}
