using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonSoccerGuideArrowUpdate : MonoBehaviour {

    // The speed of the arrow blinking
    [Header("Blinking Speed")]
    [SerializeField]
    private float blinkSpeed = 0.2f;
    // The time it takes before the arrow disappears on its own
    [Header("Blinking Duration")]
    [SerializeField]
    private float blinkDuration = 1f;

    private float blinkTimer = 0f;

	// Set a timer for the arrow to destroy itself
	void Start () {
		Invoke("DestroySelf", blinkDuration);
	}
	
	void Update () {
        // The arrow stops right away once the player has pressed one of the used buttons
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.Space))
            Destroy(gameObject);
        
		blinkTimer += Time.deltaTime;
        if (blinkTimer >= blinkSpeed)
        {
            blinkTimer = 0f;
            this.gameObject.GetComponent<SpriteRenderer> ().enabled = ! this.gameObject.GetComponent<SpriteRenderer> ().enabled;
        }
	}
    
    void DestroySelf () {
        Destroy(gameObject);
    }
    
}
