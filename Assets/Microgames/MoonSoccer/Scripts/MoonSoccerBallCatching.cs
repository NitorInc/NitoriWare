using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonSoccerBallCatching : MonoBehaviour {
    
    // Check for collision with the ball object and causes failure if it does
    void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.name == "Ball")
        {
            Destroy(col.gameObject);
            MicrogameController.instance.setVictory(victory: false, final: true);
        }
    }
}

