using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DoorKnockDoor : MonoBehaviour {
    
    [SerializeField]
    private Collider2D collider;

	// Use this for initialization
	void Start() {
       		
	}
	
	// Update is called once per frame
	void Update() {
        if (Input.GetMouseButtonDown(0) && CameraHelper.isMouseOver(collider))
        {
           OnClick(); 
        } 
	}
    
    // When the object is clicked
    void OnClick() {
        Teleport();
    }
    
    // Move to a random location
    void Teleport() {
    
    }
}
