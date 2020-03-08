using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonSoccerBallCatching : MonoBehaviour {
		
    
    // Check for collision againt the character which causes the minigame to fail and a ball animation to play out
    void OnTriggerEnter2D(Collider2D col) 
    {
		
        if (col.gameObject.name == "Ball" && !MicrogameController.instance.getVictoryDetermined())
        {
            // When the ball hits Kaguya it is destroyed
            if (gameObject.name == "Kaguya")
            { 
                Destroy(col.gameObject);
                MicrogameController.instance.setVictory(victory: false, final: true);
            // When the ball hits Tewi it plays the ball bounce animation
            } else if (gameObject.name == "Rabbit") {
                col.gameObject.GetComponentInChildren<Animator>().Play("MoonSoccerBallBounce");
                MicrogameController.instance.setVictory(victory: false, final: true);
            }
        }
    }
}

