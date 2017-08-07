using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NitoriLookTarget : MonoBehaviour
{

#pragma warning disable 0649	//Serialized Fields
#pragma warning restore 0649

	void Start()
	{
		
	}
	
	void Update()
	{
        transform.rotation = Camera.main.transform.rotation;
        Vector3 cameraEulers = Camera.main.transform.rotation.eulerAngles;
        //cameraEulers.Scale(new Vector3(.5f, 1f, 1f));
        //cameraEulers = new Vector3(cameraEulers.x * .5f, cameraEulers.y, cameraEulers.z);
        transform.rotation = Quaternion.Euler(cameraEulers);
	}
}
