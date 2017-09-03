﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NitoriLookMovement : MonoBehaviour
{

#pragma warning disable 0649	//Serialized Fields
    [SerializeField]
    private Vector2 mouseRotateVelocity;
#pragma warning restore 0649

	void Start()
	{
        Cursor.lockState = CursorLockMode.Locked;
	}
	
	void Update()
    {
        if (MicrogameController.instance.getVictoryDetermined())
        {
            enabled = false;
            return;
        }

        Vector3 eulerRotation = transform.rotation.eulerAngles;

        float x = eulerRotation.x;
        while (x > 90f)
        {
            x -= 360f;
        }
        x -= Input.GetAxis("Mouse Y") * mouseRotateVelocity.y;
        x = Mathf.Clamp(x, -80f, 15f);

        float y = eulerRotation.y;
		y += Input.GetAxis("Mouse X") * mouseRotateVelocity.x;
		
        transform.rotation = Quaternion.Euler(new Vector3(x, y, 0f));
    }
}
