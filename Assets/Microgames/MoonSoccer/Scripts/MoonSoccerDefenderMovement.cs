using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonSoccerDefenderMovement : MonoBehaviour {
    
    
    private enum VerticalMovementRange
    {
        None,
        TopHalf,
        BottomHalf,
        FullScreen
    }
    
    [Header("Vertical Movement")]
    // The speed of the vertical movement
    [SerializeField]
    private float moveSpeed = 1f;

    // Minimum height the object can reach before changing direction
    [SerializeField]
    private VerticalMovementRange movementRange;
    
    // The lenght between the leftmost and rightmost point the sprite can reach horizontally
    [Header("Horizontal Movement Length")]
    [SerializeField]
    private float xMovement = 1f;
    
    // The direction the object will start moving in
    private bool downward = true;
    
    // The total distance between the top and bottom boundaries of the vertical movement
    private float moveDistance = 0f;
    
    // The starting x value of the object transform
    private float startX = 0f;
    
    
    private float minHeight = 0f;
    private float maxHeight = 0f;
    
    // Initialization
    void Start () {
        // Determine the min and max heights based on the selected movement range
        switch  (movementRange)
        {
            case VerticalMovementRange.None:
            {
                minHeight = 0f;
                maxHeight = 0f;
                break;
            }
            case VerticalMovementRange.TopHalf:
            {
                minHeight = -0.5f;
                maxHeight = 1.5f;
                break;
            }
            case VerticalMovementRange.BottomHalf:
            {
                minHeight = -2.9f;
                maxHeight = -0.5f;
                break;
            }
            case VerticalMovementRange.FullScreen:
            {
                minHeight = -2.9f;
                maxHeight = 1.5f;
                break;
            }
        }
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
            transform.position = new Vector3(startX + x, y, transform.position.z);
        }
	}
}
