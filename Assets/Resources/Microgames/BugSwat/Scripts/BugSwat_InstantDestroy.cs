using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BugSwat_InstantDestroy : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Invoke("massDestruction", 0.1f);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void massDestruction()
    {
        Destroy(gameObject);
    }
}
