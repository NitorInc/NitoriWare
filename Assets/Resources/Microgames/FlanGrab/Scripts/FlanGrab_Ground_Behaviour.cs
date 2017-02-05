using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlanGrab_Ground_Behaviour : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnCollisionEnter2D(Collision2D other)
    {
        var otherName = other.gameObject.name;
        if (otherName.Contains("Meteor1") || otherName.Contains("Meteor2") || otherName.Contains("Meteor3"))  
        {
            FlanGrab_Meteor_BehaviourScript otherScript = (FlanGrab_Meteor_BehaviourScript) other.gameObject.transform.GetComponent(typeof(FlanGrab_Meteor_BehaviourScript));
            otherScript.meteorHasLanded();
        }
    }


}
