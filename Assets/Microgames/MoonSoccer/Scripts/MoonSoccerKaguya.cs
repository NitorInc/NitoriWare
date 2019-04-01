using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonSoccerKaguya : MonoBehaviour {
    
	// The max speed of the character
    [Header("Movement Speed")]
    [SerializeField]
    private float maximumMoveSpeed = 1f;
    
    // The time it takes for the character to accelerate to max speed
    [SerializeField]
    private float timeBeforeMaxSpeed = 1f;
    
	
	
    [Header("Perspective Related Variables")]
    // The lenght between the leftmost and rightmost point the sprite can reach horizontally
    [SerializeField]
    private float horizontalMovement = 1f;
	
    // The change in sprite size that happens when going all the way up the screen, in percentage
    [SerializeField]
    private float sizeChange = 5f;
    
	// The vertical boundaries of the character's movement
    [Header("Vertical Movement Range")]
    [SerializeField]
    private float TopY = 1.5f;
    [SerializeField]
    private float BottomY = -2.9f;
	
    // The ammount of frames before Kaguya starts moving. Determines the size of the Queue used to track movement
    [Header("Delay Before Movement")]
    [SerializeField]
    private float delay = 0;
   
   
    // The current acceleration
    private float accelerationGained = 0f;
    
    // Acceleration gained per update
    private float accelerationSpeed = 0f;
    
    // The distance between the top and bottom movement boundaries. Needed for perspective
    private float moveDistance = 0f;
    
    // The starting x value of the object
    private float startX = 0f;
    
    // The the starting scale of the sprite
    private Vector3 startScale;
	
	// The player's transform used for tracking
	private Transform playerTransform;
	
	// A queue that will contain the player's position over the last X frames.
	private Queue<float> movementQueue = new Queue<float>();
	
	private float goalY;
    
    // Initialization 
    void Start () {
        accelerationSpeed = maximumMoveSpeed / timeBeforeMaxSpeed;
        moveDistance = (BottomY * -1) + TopY;
        startX = transform.position.x;
		transform.position = new Vector3(transform.position.x, Random.Range(BottomY, TopY), transform.position.z);
        startScale = transform.localScale;
		playerTransform = GameObject.Find("Mokou").GetComponent<Transform>();
		// Fill the tracking queue with Kaguya's starting position to add a delay before she moves
        for (int i = 0; i < delay; i++) {
			movementQueue.Enqueue(transform.position.y);
		}
    }
    
	// Update is called once per frame
	void FixedUpdate () 
    {
        if (MicrogameController.instance.getVictoryDetermined() != true) {
            float y = transform.position.y;
			float x = transform.position.x;
            goalY = movementQueue.Dequeue();
			movementQueue.Enqueue(playerTransform.position.y);
			if (!(Mathf.Abs(goalY-y) < 0.1)) { // Avoid moving if the characters are lined up closely enough
				if (goalY < y && y >= BottomY)
				{
					if (accelerationGained >= 0)
						accelerationGained = 0;
					if (accelerationGained >= maximumMoveSpeed * -1)
						accelerationGained -= accelerationSpeed;
				}
				else if (goalY > y && y <= TopY)
				{
					if (accelerationGained <= 0)
						accelerationGained = 0;
					if (accelerationGained <= maximumMoveSpeed)
						accelerationGained += accelerationSpeed;
				}
			}
			else
				accelerationGained = 0;
				y = y + accelerationGained * Time.deltaTime;
            moveDistance = (BottomY * -1) + TopY;
            x = -((y - BottomY) / moveDistance) * horizontalMovement;
            transform.position = new Vector3(startX + x, y, transform.position.z);
            // Scale the character's size based on how high they are on screen
            float vDistance = 1 - ((y - BottomY) / moveDistance);
            transform.localScale = new Vector3((startScale.x / 100f) * (100-sizeChange + vDistance*sizeChange), 
                                               (startScale.y / 100f) * (100-sizeChange + vDistance*sizeChange), 
                                               startScale.z);
        }
    }
}
