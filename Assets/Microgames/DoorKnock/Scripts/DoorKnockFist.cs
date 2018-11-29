using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorKnockFist : MonoBehaviour {
    private bool knocking = false;
    private Animator anim;
    void Start() {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetMouseButtonDown(0) && !MicrogameController.instance.getVictory()) {
            anim.SetTrigger("Knock");
        }
    } 
}
