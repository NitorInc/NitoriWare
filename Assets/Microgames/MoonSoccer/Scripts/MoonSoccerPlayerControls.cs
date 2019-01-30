using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonSoccerPlayerControls : MonoBehaviour {
    
    // Whether the space key has been pressed yet or not
    private bool hasKicked = false;
        
    public MoonSoccerBall ballScript;
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKey(KeyCode.Space) && hasKicked == false)
        {
            // TODO: Add kick animation
            ballScript.activate(transform.position);
            hasKicked = true;
        }
    }
}