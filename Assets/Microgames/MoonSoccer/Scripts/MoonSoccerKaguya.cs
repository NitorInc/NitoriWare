using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonSoccerKaguya : MonoBehaviour {
    
   
    [Header("Movement Speed")]
    [SerializeField]
    private float maximumMoveSpeed = 1f;
    
    // The acceleration of the character
    [SerializeField]
    private float timeBeforeMaxSpeed = 1f;
    
    // The lenght between the leftmost and rightmost point the sprite can reach horizontally
    [Header("Horizontal Movement Length")]
    [SerializeField]
    private float xMovement = 1f;
    
    // The change in scale that happens when going all the way up the screen, in percentage
    [Header("Scale Change From Perspective")]
    [SerializeField]
    private float scaleChange = 5f;
    
    [Header("Vertical Movement Range")]
    [SerializeField]
    private float TopY = 1.5f;
    [SerializeField]
    private float BottomY = -2.9f;
    
    // The current acceleration
    private float accelerationGained = 0f;
    
    // The total distance between the top and bottom boundaries of the vertical movement
    private float moveDistance = 0f;
    
    // The starting x value of the object transform
    private float startX = 0f;
    
    // Whether the space key has been pressed yet or not
    private bool hasKicked = false;
    
    // Acceleration gained per update
    private float accelerationSpeed = 0f;
    
    // The scale of the sprite at the very start
    private Vector3 startScale;
	
	private Transform playerTransform;
    
    // Initialization 
    void Start () {
        accelerationSpeed = maximumMoveSpeed / timeBeforeMaxSpeed;
        moveDistance = (BottomY * -1) + TopY;
        startX = transform.position.x;
        startScale = transform.localScale;
		playerTransform = GameObject.Find("Mokou").GetComponent<Transform>();
    }
    
	// Update is called once per frame
	void Update () 
    {
        if (MicrogameController.instance.getVictoryDetermined() != true || hasKicked == false) {
            float x = transform.position.x;
            float y = transform.position.y;
			if (playerTransform.position.y == transform.position.y ) {
				accelerationGained = 0;
			}
            else if (playerTransform.position.y < transform.position.y && transform.position.y >= BottomY)
            {
                //if (accelerationGained >= 0)
                //    accelerationGained = 0;
                if (accelerationGained >= maximumMoveSpeed * -1)
                    accelerationGained -= accelerationSpeed;
            }
            else if (playerTransform.position.y > transform.position.y && transform.position.y <= TopY)
            {
                //if (accelerationGained <= 0)
                //    accelerationGained = 0;
                if (accelerationGained <= maximumMoveSpeed)
                    accelerationGained += accelerationSpeed;
            }
            else
                accelerationGained = 0;
            y = transform.position.y + accelerationGained * Time.deltaTime;
            moveDistance = (BottomY * -1) + TopY;
            x = -((y - BottomY) / moveDistance) * xMovement;
            transform.position = new Vector3(startX + x, y, transform.position.z);
            // Scale the character's size based on how high they are on screen
            float vDistance = 1 - ((y - BottomY) / moveDistance);
            transform.localScale = new Vector3((startScale.x / 100f) * (100-scaleChange + vDistance*scaleChange), 
                                               (startScale.y / 100f) * (100-scaleChange + vDistance*scaleChange), 
                                               startScale.z);
        }
    }
}
