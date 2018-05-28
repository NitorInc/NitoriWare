using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlanGrab_ArmBehaviour : MonoBehaviour {

    private Animator anim;

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		if (MicrogameController.instance.getVictoryDetermined())
		{
			if (MicrogameController.instance.getVictory())
				anim.SetTrigger("HandClutch");
			else
				anim.SetTrigger("OpenHand");
			enabled = false;
			return;
		}

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
