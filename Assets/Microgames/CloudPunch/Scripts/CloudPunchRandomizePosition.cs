using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudPunchRandomizePosition : MonoBehaviour
{
    [SerializeField]
    private float unitDistance;
    [SerializeField]
    private int spawnUnitRange;
    
	void Start ()
    {
        transform.position += Vector3.up * Random.Range(-spawnUnitRange, spawnUnitRange + 1) * unitDistance;
	}
}
