using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainTracks_VictoryPortraits : MonoBehaviour {

    float targety = -1.9f;
    float increment;
    int entryFrames = 30;

	// Use this for initialization
	void Start () {
        increment = (targety - transform.position.y) / entryFrames; 
	}
	
	// Update is called once per frame
	void Update () {
		if (MicrogameController.instance.getVictory() && transform.position.y < targety)
        {
            float newy = transform.position.y + increment;
            transform.position = new Vector3(transform.position.x, newy, transform.position.z);
        }
	}
}
