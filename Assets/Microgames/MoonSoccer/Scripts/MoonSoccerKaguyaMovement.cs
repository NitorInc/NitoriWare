using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonSoccerKaguyaMovement : MonoBehaviour {
    
    // The speed of her vertical movement
    [Header("Movement Speed")]
    [SerializeField]
    private float moveSpeed = 1f;

    // Minimum height the object can reach before changing direction
    [Header("Minimum Height")]
    [SerializeField]
    private float minHeight = 1f;
    
    // Maximum height the object can reach before changing direction
    [Header("Maximum Height")]
    [SerializeField]
    private float maxHeight = 1f;
    
    // The lenght between the leftmost and rightmost point the sprite can reach horizontally
    [Header("X Movement Distance")]
    [SerializeField]
    private float xMovement = 1f;   
    
    // Bool to track the current movement direction
    private bool downward = true;
    
    // The total distance between the top and bottom boundaries of the vertical movement
    private float moveDistance = 0f;
    
    // The starting x value of the object transform
    private float startX = 0f;
    
    // Initialization
    void Start () {
        moveDistance = (minHeight * -1) + maxHeight;
        startX = transform.position.x;
    }

    
	// Update Kaguya's position by moving her vertically according to moveSpeed. X value is updated based on the y position
	void Update () {
        if (MicrogameController.instance.getVictoryDetermined() != true) {
            float x = transform.position.x;
            float y = transform.position.y;
            if (downward == true)
            {
                if (transform.position.y >= minHeight)
                    y = transform.position.y - moveSpeed * Time.deltaTime;
                else
                    downward = false;
            }
            else
            {
                if (transform.position.y <= maxHeight)
                    y = transform.position.y + moveSpeed * Time.deltaTime;
                else
                    downward = true;
            }
            x = -((y - minHeight) / moveDistance) * xMovement;
            transform.position = new Vector2(startX + x, y);
        }
	}
}
