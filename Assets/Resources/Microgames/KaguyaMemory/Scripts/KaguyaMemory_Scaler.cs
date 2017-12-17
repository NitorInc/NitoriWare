using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KaguyaMemory_Scaler : MonoBehaviour {

    private int scaleDirection = 1;
    private float scaleFactor = 1;

	// Use this for initialization
	void Start () {
        Invoke("scaleFunction", 0f);
    }
	
    void scaleFunction()
    {
        scaleDirection *= -1;
        scaleFactor *= -1;
        Invoke("scaleFunction", 0.5f);
    }

	// Update is called once per frame
	void Update () {
        transform.localScale += new Vector3(scaleFactor, scaleFactor, 0);
        scaleFactor = 0.05f * scaleDirection;
    }
}
