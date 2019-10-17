using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RumiaRescueTreeHitController : MonoBehaviour {

    [SerializeField]
    private Animator ani;

    private static bool hasHitted = false;

    private void Start() {
        hasHitted = false;
    }

    private void OnTriggerEnter2D(Collider2D collider2D) {
        if (hasHitted == true)
            return;
        hasHitted = true;

        collider2D.transform.parent.SendMessage("WhenRumiaHitTree");
        ani.SetTrigger("Hitted");

        MicrogameController.instance.setVictory(false);
    }
}
