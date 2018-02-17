using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BugSwat_Counter : MonoBehaviour {

    public int wriggleCount = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (wriggleCount < 1)
        {
            MicrogameController.instance.setVictory(true, true);
        }
	}
}
