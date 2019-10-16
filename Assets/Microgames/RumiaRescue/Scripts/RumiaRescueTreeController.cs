using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RumiaRescueTreeController : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collider2D) {
        print(collider2D.name);
        MicrogameController.instance.setVictory(false);
    }
}
