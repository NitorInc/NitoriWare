using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonSoccerZOrdering : MonoBehaviour {
    
	// Update is called once per frame
	void Update () {
		if (MicrogameController.instance.getVictoryDetermined())
		{
			// Every character is set to be under the ball once the minigame ends, to make sure some ball catching animations dont look weird
			GetComponent<SpriteRenderer>().sortingOrder = -1;
		} else {
			// The sorting order is determined based on the position of the sprite's lowest point (the character's feet)
			GetComponent<SpriteRenderer>().sortingOrder = Mathf.RoundToInt(transform.position.y - (GetComponent<SpriteRenderer>().bounds.size.y/2) * -100);
		}
	}
}
