using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HatBilliardsColorMaterial : MonoBehaviour
{
    [SerializeField]

    private Color color;
    private Renderer renderer;
    
	void Start ()
    {
        renderer = GetComponent<Renderer>();
	}
	
	void Update ()
    {
        renderer.material.color = color;
	}
}
