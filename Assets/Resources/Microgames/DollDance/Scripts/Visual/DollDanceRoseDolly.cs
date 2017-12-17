using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DollDanceRoseDolly : MonoBehaviour
{
    public float moveSpeed;
    
	void Start ()
    {
		
	}
	
	void Update ()
    {
        transform.position += Vector3.up * moveSpeed * Time.deltaTime;
	}
}
