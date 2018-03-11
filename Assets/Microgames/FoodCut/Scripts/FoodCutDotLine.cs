using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodCutDotLine : MonoBehaviour {
    Collider2D dotLineCollider;

    // Use this for initialization
    void Start()
    {
        dotLineCollider = GetComponent<Collider2D>();
        dotLineCollider.isTrigger = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Temporarily turns trigger on when Space is Pressed
        if (Input.GetKeyDown(KeyCode.Space))
        {
            dotLineCollider.isTrigger = true;
            Debug.Log("Trigger On: " + dotLineCollider.isTrigger);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        print("Trigger works!");
    }
}