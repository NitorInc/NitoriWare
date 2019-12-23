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
    private float sizeChange = 5f; // Might not be useful with new assets?
    
	// The vertical boundaries of the character's movement
    [Header("Vertical Movement Range")]
    [SerializeField]
    private float TopY = 1.5f;
    [SerializeField]
    private float BottomY = -2.9f;
	
    // The ammount of time waited after reaching a location before tracking the player again
    [Header("Tracking Delay")]
    [SerializeField]
    private int delay = 0;
	private int delayTimer = 0;
   
   
    // The current acceleration
    private float accelerationGained = 0f;
    
    // Acceleration gained per update
    private float accelerationSpeed = 0f;
    
    // The distance between the top and bottom movement boundaries. Needed for perspective
    private float moveDistance = 0f;
    
    // The starting x value of the object
    private float startX = 0f;
	
	// The player's transform used for tracking
	private Transform playerTransform;
	
	// A queue that will contain the player's position over the last X frames.
	private Queue<float> movementQueue = new Queue<float>();
	
	private float goalY;
	private bool directionOfGoal; // False for up, True for down
	
    private Animator animator;
    
    // Initialization 
    void Start () {
		animator = GetComponentInChildren<Animator>();
        accelerationSpeed = maximumMoveSpeed / timeBeforeMaxSpeed;
        moveDistance = (BottomY * -1) + TopY;
        startX = transform.position.x;
		transform.position = new Vector3(transform.position.x, Random.Range(BottomY, TopY), transform.position.z);
		playerTransform = GameObject.Find("Mokou").GetComponent<Transform>();
		SetGoal();
    }

	void FixedUpdate () 
    {
		// Player tracking
        if (MicrogameController.instance.getVictoryDetermined() != true && delayTimer == 0) {
            float y = transform.position.y;
			float x = transform.position.x;
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
				y = y + accelerationGained * Time.deltaTime;
			} else {
				SetGoal();
				accelerationGained = 0;
			}
			
			// Visual polish related to movement
            moveDistance = (BottomY * -1) + TopY;
            x = -((y - BottomY) / moveDistance) * horizontalMovement;
            transform.position = new Vector3(startX + x, y, transform.position.z);
        } else if (delayTimer > 0) {
			delayTimer -= 1;
		}
		
		animator.SetBool("IsWalking", accelerationGained != 0);
    }
	
	void SetGoal() {
		goalY = playerTransform.position.y;
		bool oldDirection = directionOfGoal;
		directionOfGoal = (goalY <= transform.position.y);
		if (directionOfGoal != oldDirection)
			delayTimer = delay;
	}
}
