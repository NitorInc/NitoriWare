using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuukotoSweep_Sweeping : MonoBehaviour {


    private Animator animator;

	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        manageSweepMovement();
    }

    void manageSweepMovement()
    {
        animator.SetBool("Sweep", Input.GetKey(KeyCode.Space));
    }
}
