using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonSoccerBallCatching : MonoBehaviour {
		
	private MoonSoccerBall ballScript;
    
    // Check for collision with the ball object and causes failure if it does
    void OnTriggerEnter2D(Collider2D col) 
    {
		
        if (col.gameObject.name == "Ball" && !MicrogameController.instance.getVictoryDetermined())
        {
			ballScript = col.gameObject.GetComponent<MoonSoccerBall>();
            if (gameObject.name == "Kaguya")
            // When the ball hits Kaguya
            { 
                Destroy(col.gameObject);
                MicrogameController.instance.setVictory(victory: false, final: true);
            // When the ball hits tewi
            } else if (gameObject.name == "Rabbit") { 
                ballScript.moveSpeed = -3;
                col.gameObject.GetComponentInChildren<Animator>().Play("MoonSoccerBallBounce");
                MicrogameController.instance.setVictory(victory: false, final: true);
            // When the ball hits the two defender characters
            } else {
                ballScript.moveSpeed = -16;
                ballScript.vMoveSpeed = 16;
                MicrogameController.instance.setVictory(victory: false, final: true);
            }
        }
    }
}

