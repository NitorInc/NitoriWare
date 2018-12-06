using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonSoccerBunnyLook : MonoBehaviour {
    
    // The ammount of time before the sprite turns around
    private float timer = 0f;
    
    // Reference to the object's spriteRenderer
    private SpriteRenderer spriteRenderer;

	// Use this for initialization
	void Start () {
    
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
		
	}
	
	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;
		if (timer > 1) {
			spriteRenderer.flipX = !spriteRenderer.flipX;
			timer = 0f;
		}
		
	}
}
