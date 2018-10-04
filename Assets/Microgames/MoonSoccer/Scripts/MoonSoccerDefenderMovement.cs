using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonSoccerDefenderMovement : MonoBehaviour {
    
    // The speed of the vertical movement
    [Header("Vertical Movement Speed")]
    [SerializeField]
    private float moveSpeed = 1f;

    // Minimum height the object can reach before changing direction
    [Header("Vertical Movement Range")]
    [SerializeField]
    private float minHeight = 1f;
    
    // Maximum height the object can reach before changing direction
    [SerializeField]
    private float maxHeight = 1f;
    
    // The lenght between the leftmost and rightmost point the sprite can reach horizontally
    [Header("Horizontal Movement Length")]
    [SerializeField]
    private float xMovement = 1f;
    
    // The direction the object will start moving in
    [Header("Initial Direction")]
    [SerializeField]
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

    
	// Update the object's position by moving it vertically according to moveSpeed. X value is updated based on the y position
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
