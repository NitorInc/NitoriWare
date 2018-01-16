using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YuukaWaterPlantOffset : MonoBehaviour
{
    public SineWave sine;
    
	void Start ()
    {
        sine.xOffset = Random.Range(0f, 1f);
	}
}
