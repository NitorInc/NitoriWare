using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonSoccerBunnyLook : MonoBehaviour {
    
    private float timer = 0f;
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
