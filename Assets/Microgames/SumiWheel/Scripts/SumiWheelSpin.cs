using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SumiWheelSpin : MonoBehaviour
{
    [SerializeField]
    private float spinSpeed;
    
	void Update ()
    {
        transform.Rotate(Vector3.forward * spinSpeed * Time.deltaTime);
	}
}
