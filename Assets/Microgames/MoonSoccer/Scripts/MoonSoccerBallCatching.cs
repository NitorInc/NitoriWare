using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonSoccerBallCatching : MonoBehaviour {
    
    // Check for collision with the ball object and causes failure if it does
    void OnTriggerEnter2D(Collider2D col) 
    {
        if (col.gameObject.name == "Ball")
        {
            if (gameObject.name == "Kaguya")
            // When the ball hits Kaguya
            { 
                Destroy(col.gameObject);
                MicrogameController.instance.setVictory(victory: false, final: true);
            // When the ball hits tewi
            } else if (gameObject.name == "Rabbit") { 
                // TODO: actual little animation there
                MicrogameController.instance.setVictory(victory: false, final: true);
            // When the ball hits the two defender characters
            } else {
                col.gameObject.GetComponent<MoonSoccerBall>().moveSpeed = -16;
                col.gameObject.GetComponent<MoonSoccerBall>().vMoveSpeed = 16;
                MicrogameController.instance.setVictory(victory: false, final: true);
            }
        }
    }
}

