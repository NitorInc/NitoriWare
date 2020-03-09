using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkRoomEnding : MonoBehaviour
{
    
	void Start ()
    {
		
	}
	
	void Update ()
    {
        var cam = MainCameraSingleton.instance;
        if (cam.transform.position.x >= transform.position.x)
        {
            cam.transform.parent = null;
            cam.transform.position = new Vector3(transform.position.x, cam.transform.position.y, cam.transform.position.z);
            MicrogameController.instance.setVictory(true);
            DarkRoom_RenkoBehavior.instance.Win();
        }
		
	}
}
