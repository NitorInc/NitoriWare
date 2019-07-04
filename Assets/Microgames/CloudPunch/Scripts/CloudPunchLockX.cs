using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudPunchLockX : MonoBehaviour
{
    private float initialX;

	void Start ()
    {
        initialX = transform.position.x;
	}
	
	void LateUpdate()
    {
        transform.position = new Vector3(initialX, transform.position.y, transform.position.z);
	}
}
