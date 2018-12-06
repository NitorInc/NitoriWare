using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonSoccerLayoutPick : MonoBehaviour {
    
    public int layout = 0;

	// Determine the layout randomly before other objects start
	void Awake () {
		layout = Random.Range(0, 3);
	}
}
