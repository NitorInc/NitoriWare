using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonSoccerKickAnimation : MonoBehaviour {

    public MoonSoccerBall ballScript;

    public void activateBall () 
    {
            ballScript.activate(transform.position);
	}
}
