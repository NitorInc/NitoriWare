using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class BoundsFinder : MonoBehaviour
{
    
	void Start ()
    {
		
	}
	
	void Update ()
    {
        var mesh = GetComponent<MeshRenderer>();
        Debug.Log(mesh.bounds);
	}
}
