using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NitoriLookDisableRandomChild : MonoBehaviour
{
    
	void Awake()
    {
        var randomChildIndex = Random.Range(0, transform.childCount);
        Destroy(transform.GetChild(randomChildIndex).gameObject);
	}
}
