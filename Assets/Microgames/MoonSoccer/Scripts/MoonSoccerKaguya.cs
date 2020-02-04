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
	
    [SerializeField]
    private int delay = 0;
    
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
	
	// Used to count down the delay when Kaguya waits to move
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
	
	private float goalY;
	private bool directionOfGoal; // False for up, True for down
	
    private Animator animator;
    
    // Initialization 
    void Awake () {
		animator = GetComponentInChildren<Animator>();
        accelerationSpeed = maximumMoveSpeed / timeBeforeMaxSpeed;
        moveDistance = (BottomY * -1) + TopY;
        startX = transform.position.x;
		playerTransform = GameObject.Find("Mokou").GetComponent<Transform>();
    }
	
	void Start () {
		transform.position = new Vector3(transform.position.x, playerTransform.position.y, transform.position.z);
		SetGoal();
	}

	void FixedUpdate () 
    {
        if (MicrogameController.instance.getVictoryDetermined() != true && delayTimer == 0) { // When moving
			Move();
        } else if (delayTimer > 0) { // When waiting to move
			delayTimer -= 1;
		}
		
		// Make Kaguya stop when the ball is kicked
        if (Input.GetKey(KeyCode.Space)) {
			animator.SetBool("IsWaiting", true);
		}
		
		if (MicrogameController.instance.getVictoryDetermined() == true && MicrogameController.instance.getVictory() == false) {
			animator.SetBool("CaughtBall", true);
		}
    }
	
	// Set a new point to track based on the player's location
	void SetGoal() {
		goalY = playerTransform.position.y;
		// The timer before Kaguya can move again is only set if she's about to switch direction. This is less useful than expected, and might go unused.
		bool oldDirection = directionOfGoal; //todo move this
		directionOfGoal = (goalY <= transform.position.y);
		if (directionOfGoal != oldDirection) {
			accelerationGained = 0;
			delayTimer = delay;
		}
	}
	
	// Movement toward the set goal
	void Move() {
        float y = transform.position.y;
		float x = transform.position.x;
		if (!(Mathf.Abs(goalY-y) < 0.1)) { // Stop moving if the characters are nearly overlapping
			accelerationGained = Mathf.Abs(accelerationGained);
			if (animator.GetCurrentAnimatorStateInfo(0).IsName("MoonSoccerKaguyaWalk")) { // Whether Kaguya moves or not depends on her animation state. This is done to use the animation transition as a timer to tell when she should stop
				if (accelerationGained < maximumMoveSpeed) {
					accelerationGained += accelerationSpeed;
				}
			} else { // Lose acceleration after the ball has been kicked. This barely shows unless you use different numbers for acceleration
				if (accelerationGained > 0) {
					accelerationGained -= accelerationSpeed;
				}				
			}
			if (goalY < y) {
				y = y - accelerationGained * Time.deltaTime;
			} else {
				y = y + accelerationGained * Time.deltaTime;
			}
		} else { // If the character overlap, lose speed and set new goal
			SetGoal();
		}
		
		// Visual polish related to perspective
		moveDistance = (BottomY * -1) + TopY;
		x = -((y - BottomY) / moveDistance) * horizontalMovement;
		transform.position = new Vector3(startX + x, y, transform.position.z);
	}
}
