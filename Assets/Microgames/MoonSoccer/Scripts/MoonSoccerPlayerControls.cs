using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonSoccerPlayerControls : MonoBehaviour {
    
    // Player object speed
    [Header("Final Movement Speed")]
    [SerializeField]
    private float finalMoveSpeed = 1f;
    
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
    
    private float acceleration = 0f;
    
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
        if (MicrogameController.instance.getVictoryDetermined() != true) {
            float x = transform.position.x;
            float y = transform.position.y;
            float accelerationSpeed = 0.3f;
            if (Input.GetKey(KeyCode.DownArrow) && transform.position.y >= minHeight)
            {
                if (acceleration >= 0)
                    acceleration = 0;
                if (acceleration >= finalMoveSpeed * -1)
                    acceleration -= accelerationSpeed;
            }
            else if (Input.GetKey(KeyCode.UpArrow) && transform.position.y <= maxHeight)
            {
                if (acceleration <= 0)
                    acceleration = 0;
                if (acceleration <= finalMoveSpeed)
                    acceleration += accelerationSpeed;
            }
            else
                acceleration = 0;
            y = transform.position.y + acceleration * Time.deltaTime;
            moveDistance = (minHeight * -1) + maxHeight;
            x = ((y - minHeight) / moveDistance) * xMovement;
            transform.position = new Vector2(startX + x, y);
        }
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
