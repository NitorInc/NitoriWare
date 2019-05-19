using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodCutKnifeStretch : MonoBehaviour {

    public GameObject knifeParent;
    
    [Header("Minimum X position of movement")]
    public float minX = 0f;
    [Header("Maximum X position of movement")]
    public float maxX = 0f;
    
    static float t = 0.0f;
    private SpriteRenderer spriteRenderer;

    // Use this for initialization
    void Start (){
        spriteRenderer = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {

        spriteRenderer.enabled = (transform.position.x >= minX && transform.position.x <= maxX);

	}
}
