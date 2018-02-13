using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script can force a camera to render nothing but black upon calling disableMask()
//It returns to its normal rendering when reenableMask() is called
//Simulates pausing the game with a microgame camera

public class DisableCameraMask : MonoBehaviour
{
    
    private Camera camera;
    private int cameraMask;
    private Color cameraColor;
    
	void Start ()
    {
        camera = GetComponent<Camera>();
        cameraMask = camera.cullingMask;
        cameraColor = camera.backgroundColor;
	}

    public void disableMask()
    {
        cameraMask = camera.cullingMask;
        cameraColor = camera.backgroundColor;
        camera.cullingMask = 0;
        camera.backgroundColor = Color.black;
    }

    public void reenableMask()
    {
        camera.cullingMask = cameraMask;
        camera.backgroundColor = cameraColor;
    }
}
