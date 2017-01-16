using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemiCover_RemiBehaviour : MonoBehaviour {

    public float movementSpeed;                     // Speed of Remilia's movement (Walking)
    public float fasterMovementSpeed;               // Speed of Remilia's movement (Running)
    public float leftLimit;                         // Minimum value of Remilia's X position that she can take
    public float rightLimit;                        // Maximum value of Remilia's X position that she can take

    // Probabilities for choosing, randomly, different movements for Remilia (Walking, Standing and Running)
    public int walkProb;          
    public int standProb;         
    // Running probabilty will be (100 - walkProb - standProb)

    private const int NONE = -1;                    // None selection
    private const int WALK = 0;                     // Walk movement selection
    private const int STAND = 1;                    // Standing movement selection 
    private const int RUN = 2;                      // Run movement selection
    private int lastMovementSelection = NONE;       // Last movement selected
    private int previousMovementSelection = NONE;   // Previous movement selected
    private bool isMoving = false;                  // Boolean to check if character is moving or not.

    private int selectionCounter = 0;               // Counter to vary the duration of the movements
    public int minSelectionCounter;                 // Minimum value of selectionCounter
    public int maxSelectionCounter;                 // Maximum value of selectionCounter

    private int movementDirection = 0;              // To specify where Remilia is moving (Left by default).
    private const int LEFT = 0;                     // Left direction
    private const int RIGHT = 1;                    // Right direction
  
    private GameObject[] shadowObjs = null;         // List of Gameobjects that in-game represents a Shadow
    private GameObject remiSprite = null;           // Sprite of Remi
    

	// Use this for initialization
	void Start () {
        Vector2 mousePosition = CameraHelper.getCursorPosition();
        this.transform.position = new Vector2(mousePosition.x, this.transform.position.y);
        this.remiSprite = transform.Find("RemiSprite").gameObject;
        this.shadowObjs = GameObject.FindGameObjectsWithTag("Shadow");

        // First character's movement will be standing
        this.lastMovementSelection = NONE;
        this.selectionCounter = 25; 
    }
	

	// Update is called once per frame
	void Update () {
		
	}


    void FixedUpdate()
    {
        moveCharacter();
        bool isUnderShadow = checkIfUnderShadow();

        if (isUnderShadow)
        {
            changeSpriteColor(Color.white);
        }
        else
        {
            // Game Over
            changeSpriteColor(Color.red);
        }
    }


    // Move character
    private void moveCharacter()
    {
        if (lastMovementSelection == NONE) {
            chooseMovement();
        }
 
        else {
            continueMovement();
        }
    }


    // Select one of the three options of movement (Walk, Stand or Run)
    private void chooseMovement()
    {
        int rnd_number = Random.Range(1, 101);                                  // Random, number between 1 and 100 (For probabilities)

        if (walkHasBeenChosen(rnd_number))
        {
            chooseMovementDirection();
            walkMovement();
            lastMovementSelection = WALK;
        }

        else if (standHasBeenChosen(rnd_number))
        {
            standing();
            lastMovementSelection = STAND;
        }

        else
        {
            chooseMovementDirection();
            runMovement();
            lastMovementSelection = RUN;
        }

        selectionCounter = Random.Range(minSelectionCounter, maxSelectionCounter);  // How much will the character move?
    }


    // Continue movement that has been chosen previously.
    private void continueMovement()
    {
        switch (lastMovementSelection)
        {
            case WALK:
                walkMovement();
                break;

            case STAND:
                standing();
                break;

            case RUN:
                runMovement();
                break;
        }

        selectionCounter -= 1;
        if (selectionCounter == 0) {                        // Restart parameters 
            previousMovementSelection = lastMovementSelection;
            lastMovementSelection = NONE;
            isMoving = false;
        }
    }


    // Check if Walk movement has been chosen according to a number between 1 and 100
    private bool walkHasBeenChosen(int number)
    {
        if(number >= 1 && number <= this.walkProb)
        {
            return true;
        }
        return false;
    }


    // Check if Standing has ben chosen according to a number between 1 and 100
    private bool standHasBeenChosen(int number)
    {
        if(number > this.walkProb && number <= standProb + walkProb)
        {
            return true;
        }
        return false;
    }


    // Make character walk
    private void walkMovement()
    {
        var move = obtainMovementVector3();
        Bounds remiBounds = this.GetComponent<BoxCollider2D>().bounds;
        this.transform.position = this.transform.position + (move * this.movementSpeed * Time.deltaTime);
        this.isMoving = true;
        changeDirection();

    }


    // Make character stand
    private void standing()
    {
        this.isMoving = false;
    }
    

    // Make caharacter run
    private void runMovement()
    {
        var move = obtainMovementVector3();
        Bounds remiBounds = this.GetComponent<BoxCollider2D>().bounds;
        this.transform.position = this.transform.position + (move * this.fasterMovementSpeed * Time.deltaTime);
        this.isMoving = true;
        changeDirection();
    }


    // Choose Randomly a direction which the character will follow (LEFT or RIGHT, 0 or 1). Also, if character was walking or running previously, then it won't change direction.
    private void chooseMovementDirection()
    {
        if (!isMoving) {
            if (!(previousMovementSelection == WALK || previousMovementSelection == RUN))
            {
                this.movementDirection = Random.Range(0, 2);
            }
        }
    }


    // Change direction of movement if character reach left or right limit
    private void changeDirection()
    {
        if (this.transform.position.x <= leftLimit){ this.movementDirection = RIGHT; }
        else if(this.transform.position.x >= rightLimit){ this.movementDirection = LEFT; }
    }


    // Obtain movement vector according to direction
    private Vector3 obtainMovementVector3()
    {
        var move = new Vector3(0, 0, 0);           
        switch (this.movementDirection)
        {
            case LEFT:
                move = new Vector3(-1, 0, 0);
                break;

            case RIGHT:
                move = new Vector3(1, 0, 0);
                break;
        }
        return move;
    }


    // Limit character movement to left and right limit
    private void limitMovement()
    {
        this.transform.position = new Vector2(Mathf.Clamp(leftLimit, this.transform.position.x, rightLimit), this.transform.position.y);
    }


    // Check if character is under shadow or not
    private bool checkIfUnderShadow()
    {
        Bounds remiBounds = this.GetComponent<BoxCollider2D>().bounds;
        float left = remiBounds.center.x - remiBounds.extents.x;
        float right = remiBounds.center.x + remiBounds.extents.x;

        bool isRemiInside = false;
        foreach (GameObject shadow in this.shadowObjs)
        {
            Bounds shadowBounds = shadow.GetComponent<BoxCollider2D>().bounds;
            if (shadowBounds.Contains(new Vector2(left, shadowBounds.center.y)) && shadowBounds.Contains(new Vector2(right, shadowBounds.center.y)))
            {
                isRemiInside = true;
                break;
            }
        }
        return isRemiInside;
    }


    private void changeSpriteColor(Color color)
    {
        remiSprite.GetComponent<SpriteRenderer>().color = color;
    }

}
