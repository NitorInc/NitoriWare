using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KaguyaMemory_Scaler : MonoBehaviour {

    private int scaleDirection = 1;
    private float scaleFactor = 1;
    private int timer1 = 30;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.localScale += new Vector3(scaleFactor, scaleFactor, 0);
        scaleFactor = 0.05f * scaleDirection;

        if (timer1 == 30)
        {
            scaleDirection *= -1;
            scaleFactor *= -1;
            timer1 = 0;
        }

        timer1++;
    }
}
