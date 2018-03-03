using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelSpin : MonoBehaviour
{

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        //Determine difficulty, if easy, half speed, if normal, regular speed, if hard, double speed
        //Rotation
        transform.Rotate(Vector3.forward / 2);
    }
}
