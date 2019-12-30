using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class BandConductorIKStayUpright : MonoBehaviour
{
    [SerializeField]
    private float angle = 90f;

	void Start ()
    {
		
	}
	
	void LateUpdate ()
    {
        transform.eulerAngles = Vector3.forward * angle;
	}
}
