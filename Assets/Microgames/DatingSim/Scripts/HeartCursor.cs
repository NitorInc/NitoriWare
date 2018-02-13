using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartCursor : MonoBehaviour {

    Animator anim;
	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
        OptionController.OnLosing += PlayOnFail;
	}

    void PlayOnFail() {
        anim.Play("Break", 0, 0.0f);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
