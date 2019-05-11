﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodCutFishScript : MonoBehaviour {

    public GameObject mask;
    public float speed = 0.0f;
    public float deceleration = 0.0f;

	// Use this for initialization
	void Start () {
        mask = this.gameObject.transform.GetChild(0).GetChild(0).GetChild(0).gameObject;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
		if (speed > 0)
        {
            transform.position = new Vector3(transform.position.x + speed, transform.position.y, transform.position.z);
            speed -= deceleration;
            if (speed < 0)
            {
                speed = 0;
            }
        }
        else if (speed < 0)
        {
            transform.position = new Vector3(transform.position.x + speed, transform.position.y, transform.position.z);
            speed += deceleration;
            if (speed > 0)
            {
                speed = 0;
            }
        }
	}
}
