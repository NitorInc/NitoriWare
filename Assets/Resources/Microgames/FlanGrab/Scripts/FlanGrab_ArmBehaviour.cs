﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlanGrab_ArmBehaviour : MonoBehaviour {

    private Animator anim;

	// Use this for initialization
	void Start () {
        anim = this.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		
        if (Input.GetMouseButtonDown(0))
        {
            anim.SetTrigger("HandClutch");
        }

        else if (Input.GetMouseButtonUp(0))
        {
            anim.SetTrigger("OpenHand");
        }
	}
}
