using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonSoccerBunnyLook : MonoBehaviour {
    
    // The ammount of time before the sprite turns around
    private float timer = 0f;
    
    // Reference to the object's spriteRenderer
    private SpriteRenderer spriteRenderer;

	void Start () {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
	}
	
	// Flip the sprite horizontally on a timer but only after the minigame victory state has been set
	void Update () {
        if (MicrogameController.instance.getVictoryDetermined() != false) { 
            timer += Time.deltaTime;
            if (timer > 1) {
                spriteRenderer.flipX = !spriteRenderer.flipX;
                timer = 0f;
            }
		}
	}
}
