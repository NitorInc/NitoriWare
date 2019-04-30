using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonSoccerZOrdering : MonoBehaviour {
    
	// Update is called once per frame
	void Update () {
		GetComponent<SpriteRenderer>().sortingOrder = Mathf.RoundToInt(transform.position.y - (GetComponent<SpriteRenderer>().bounds.size.y/2) * -100);
	}
}
