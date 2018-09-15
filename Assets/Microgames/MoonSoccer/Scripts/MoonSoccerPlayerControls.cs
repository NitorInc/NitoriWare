using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonSoccerPlayerControls : MonoBehaviour {
    
    // Player object speed
    [Header("Movement Speed")]
    [SerializeField]
    private float moveSpeed = 1f;
    
    // Minimum height allowed
    [Header("Minimum Height")]
    [SerializeField]
    private float minHeight = 1f;
    
    // Maximum height allowed
    [Header("Maximum Height")]
    [SerializeField]
    private float maxHeight = 1f;
    
    // The lenght between the leftmost and rightmost point the sprite can reach horizontally
    [Header("X Movement Distance")]
    [SerializeField]
    private float xMovement = 1f; 
    
    // The total distance between the top and bottom boundaries of the vertical movement
    private float moveDistance = 0f;
    
    // The starting x value of the object transform
    private float startX = 0f;
    
    // Tells if the Space key has been pressed before
    private bool hasKicked = false;
        
    public MoonSoccerBall ballScript;
    
    // Initialization 
    void Start () {
        moveDistance = (minHeight * -1) + maxHeight;
        startX = transform.position.x;
    }
    
	// Update is called once per frame
	void Update () 
    {
        updateMovement();
        updateKick();
	}
    
    // Check player inputs and update object position. X value is updated based on the y position
    void updateMovement ()
    {
        float x = transform.position.x;
        float y = transform.position.y;
        if (Input.GetKey(KeyCode.DownArrow) && transform.position.y >= minHeight)
        {
            y = transform.position.y - moveSpeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.UpArrow) && transform.position.y <= maxHeight)
        {
            y = transform.position.y + moveSpeed * Time.deltaTime;
        }
        moveDistance = (minHeight * -1) + maxHeight;
        x = ((y - minHeight) / moveDistance) * xMovement;
        transform.position = new Vector2(startX + x, y);
    }
    
    // Activate the ball object when Spacebar is pressed
    void updateKick ()
    {
     if (Input.GetKey(KeyCode.Space) && hasKicked == false)
     {
         // TODO: Add kick animation
         ballScript.activate(transform.position);
         hasKicked = true;
     }
    }
}
