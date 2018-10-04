using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonSoccerBallCatching : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    
    // Collision with the ball
    void OnTriggerEnter2D(Collider2D col) {
        Destroy(col.gameObject);
        MicrogameController.instance.setVictory(victory: false, final: true);
    }
}

