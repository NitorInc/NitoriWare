
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkRoom_GameHandler : MonoBehaviour {

    private bool hasFailed = false;

    /* Base methods */

    void Start () {
		
	}

	void Update () {
		
	}

    /* Getters and setters */

    public bool HasFailed { get { return hasFailed; } set { hasFailed = value; } }

}
