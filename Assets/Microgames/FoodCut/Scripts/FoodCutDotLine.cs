using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodCutDotLine : MonoBehaviour {

    [Header("Minimum x position of dotted line")]
    [SerializeField]
    private float minX = -3f;

    [Header("Maximum x position of dotted line")]
    [SerializeField]
    private float maxX = 3f;

    // Use this for initialization
    void Start()
    { 
        transform.position = new Vector2(Random.Range(minX, maxX), transform.position.y);
    }

    // Update is called once per frame
    void Update()
    {
    }

}