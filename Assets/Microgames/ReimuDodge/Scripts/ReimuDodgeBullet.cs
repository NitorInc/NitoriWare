using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReimuDodgeBullet : MonoBehaviour {
    //A Unity in-editor variable
    [Header("the thing to fly towards")]
    [SerializeField]
    private GameObject target;

    //Stores the direction of travel for the bullet
    private Vector2 trajectory;

	// Use this for initialization
	void Start ()
    {
        //Calculate a trajectory towards the target
        trajectory = (target.transform.position - transform.position).normalized;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
