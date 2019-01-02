using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MagFestSettingsAccess : MonoBehaviour
{
    
	void Start ()
    {
		
	}
	
	void Update ()
    {
        if (Input.GetKey(KeyCode.Alpha2) && Input.GetKey(KeyCode.Alpha8) && Input.GetKeyDown(KeyCode.Alpha4))
            GetComponent<Button>().onClick.Invoke();
		
	}
}
