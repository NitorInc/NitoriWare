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
        if (other.gameObject.name.Contains("Meteor"))  
        {
            FlanGrab_Meteor_BehaviourScript otherScript = (FlanGrab_Meteor_BehaviourScript) other.gameObject.transform.GetComponent(typeof(FlanGrab_Meteor_BehaviourScript));
            otherScript.meteorHasLanded();
        }
    }


}
