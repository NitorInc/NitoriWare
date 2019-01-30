using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonSoccerMovement : MonoBehaviour {
	
	
    [Header("Movement Speed")]
    [SerializeField]
	private float moveSpeed = 2f;
	
    [SerializeField]
	private bool startMovingDown = false;
    
    // The upper and lower bound of the vertical movement 
    [Header("Vertical Ranges")]
    [SerializeField]
    private float TopY = 1.5f;
    [SerializeField]
    private float BottomY = -2.9f;
	
    // The lenght between the leftmost and rightmost point the sprite can reach horizontally
    [SerializeField]
    private float hMovementWidth = 1f;
    
    // The change in scale that happens when going all the way up the screen, in percentage
    [Header("Scale Change for Perspective")]
    [SerializeField]
    private float scalePercentage = 5f;
	
    // The starting x value of the object transform
    private float startX = 0f;
    
    // The upper and lower bounds of the object's vertical movement
    private float minHeight = 0f;
    private float maxHeight = 0f;
    
    private bool downward;
    
    // The total distance between the top and bottom boundaries of the vertical movement
    private float moveDistance = 0f;
    
    // The scale of the sprite at the very start
    private Vector3 startScale;
    
    // Initialization
    void Start () {
        moveDistance = (BottomY * -1) + TopY;
        startX = transform.position.x;
        startScale = transform.localScale;
		downward = startMovingDown;
    }

    
	// Update the object's position by moving it vertically according to moveSpeed. X value is updated based on the y position
	void Update () {
        if (MicrogameController.instance.getVictoryDetermined() != true) {
            float x = transform.position.x;
            float y = transform.position.y;
            if (downward == true)
            {
                if (transform.position.y >= BottomY)
                    y = transform.position.y - moveSpeed * Time.deltaTime;
                else
                    downward = false;
            }
            else
            {
                if (transform.position.y <= TopY)
                    y = transform.position.y + moveSpeed * Time.deltaTime;
                else
                    downward = true;
            }
            // Horizontal Movement based on how high they are on screen
            x = -((y - BottomY) / moveDistance) * hMovementWidth;
            transform.position = new Vector3(startX + x, y, transform.position.z);
            // Scale the character's size based on how high they are on screen
            float vDistance = 1 - ((y - BottomY) / moveDistance);
            transform.localScale = new Vector3((startScale.x / 100f) * (100-scalePercentage + vDistance*scalePercentage), 
                                               (startScale.y / 100f) * (100-scalePercentage + vDistance*scalePercentage), 
                                               startScale.z);
        }
	}
}