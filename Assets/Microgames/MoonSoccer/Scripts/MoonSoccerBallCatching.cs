using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonSoccerBallCatching : MonoBehaviour {
		
	private MoonSoccerBall ballScript;
    
    // Check for collision againt the character which causes the minigame to fail and a ball animation to play out
    void OnTriggerEnter2D(Collider2D col) 
    {
		
        if (col.gameObject.name == "Ball" && !MicrogameController.instance.getVictoryDetermined())
        {
			ballScript = col.gameObject.GetComponent<MoonSoccerBall>();
            // When the ball hits Kaguya it is destroyed
            if (gameObject.name == "Kaguya")
            { 
                Destroy(col.gameObject);
                MicrogameController.instance.setVictory(victory: false, final: true);
            // When the ball hits Tewi it plays the ball bounce animation
            } else if (gameObject.name == "Rabbit") { 
                ballScript.moveSpeed = -3;
                col.gameObject.GetComponentInChildren<Animator>().Play("MoonSoccerBallBounce");
                MicrogameController.instance.setVictory(victory: false, final: true);
            // When the ball hits the two defender characters it flies off into space
            } else {
                ballScript.moveSpeed = -18;
                ballScript.vMoveSpeed = 14;
                MicrogameController.instance.setVictory(victory: false, final: true);
            }
        }
    }
}

