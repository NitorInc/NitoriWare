using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RemiCover_RemiBehaviour : MonoBehaviour {

    public float walkingSpeed;                      // Speed of Remilia's movement (Walking)
    public float runningSpeed;                     // Speed of Remilia's movement (Running)
    public float leftLimit;                         // Minimum value of Remilia's X position that she can take
    public float rightLimit;                        // Maximum value of Remilia's X position that she can take

    // Probabilities for choosing, randomly, different movements for Remilia (Walking, Standing and Running)
    public int walkProbability;                     // Must be between 0 and 100.
    public int standProbability;                    // Must be between 0 and 100.
                                                    // Running probabilty will be the  remaining percentage

    // Actions
    private const int NONE = -1;                    // None selection
    private const int WALK = 0;                     // Walk movement selection
    private const int STAND = 1;                    // Standing movement selection 
    private const int RUN = 2;                      // Run movement selection
    private int lastMovementSelection = NONE;       // Last movement selected
    private int previousMovementSelection = NONE;   // Previous movement selected
    private bool isMoving = false;                  // Boolean to check if character is moving or not.

    public bool defaultMovementIsWalk;              // True --> Character will walk if standing movement is going to be selected twice.
                                                    // False --> Character will run if standing movement is going to be selected twice.
                                                    // This way, character won't be standing for all of the game.

    private float selectionTimer = 0;               // How long will the selected movement be performed? (Will be assigned Randomly).
    public float min_selectionTimer;                // Minimum value of selectionTimer (on Initialization)
    public float max_SelectionTimer;                // Maximum value of selectionTimer

    private int movementDirection = 0;              // To specify where Remilia is moving (Left by default).
    private const int LEFT = 0;                     // Left direction
    private const int RIGHT = 1;                    // Right direction
  
    private GameObject shadowObj = null;            // List of Gameobjects that in-game represents a Shadow
    private GameObject remiSprite = null;           // Sprite of Remi

    private bool facingRight = true;
    private bool stopMovement = false;              // To stop movement
    

	// Use this for initialization
	void Start () {
        Vector2 mousePosition = CameraHelper.getCursorPosition();
        this.transform.position = new Vector2(mousePosition.x, this.transform.position.y);
        this.remiSprite = transform.Find("RemiSprite").gameObject;
        this.shadowObj = GameObject.Find("Player/UmbrellaShadow");

        // At first, character will be standing.
        this.lastMovementSelection = STAND;
        this.selectionTimer = 1.0f;
    }
	
    
    void Update()
    {
        if (!stopMovement) { 
            moveCharacter();
            if (!checkIfUnderShadow()) { GameOver(); }
        }
    }


    // The things that happen when Player loses
    private void GameOver()
    {
        this.stopMovement = true;
        MicrogameController.instance.setVictory(false, true);
        changeSpriteColor(Color.red);
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

            if(previousMovementSelection == STAND)
            {
                chooseMovementDirection();
                switch (defaultMovementIsWalk)
                {
                    case true:
                        walkMovement();
                        lastMovementSelection = WALK;
                        break;

                    case false:
                        runMovement();
                        lastMovementSelection = RUN;
                        break;

                }
            }

            else
            {
                standing();
                lastMovementSelection = STAND;
            }

        }

        else
        {
            chooseMovementDirection();
            runMovement();
            lastMovementSelection = RUN;
        }

        selectionTimer = Random.Range(min_selectionTimer, max_SelectionTimer);
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

        selectionTimer = selectionTimer - Time.deltaTime;
        if (selectionTimer <= 0) {                        // Restart parameters 
            previousMovementSelection = lastMovementSelection;
            lastMovementSelection = NONE;
            isMoving = false;
        }
    }


    // Check if Walk movement has been chosen according to a number between 1 and 100.
    private bool walkHasBeenChosen(int number)
    {
        if((number >= 1 && number <= this.walkProbability))
        {
            return true;
        }
        return false;
    }


    // Check if Standing has ben chosen according to a number between 1 and 100
    private bool standHasBeenChosen(int number)
    {
        if(number > this.walkProbability && number <= standProbability + walkProbability)
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
        this.transform.position = this.transform.position + (move * this.walkingSpeed * Time.deltaTime);
        this.isMoving = true;
        changeDirectionOnLimit();

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
        this.transform.position = this.transform.position + (move * this.runningSpeed * Time.deltaTime);
        this.isMoving = true;
        changeDirectionOnLimit();
    }


    // Choose Randomly a direction which the character will follow (LEFT or RIGHT, 0 or 1). Also, if character was walking or running previously, then it won't change direction.
    private void chooseMovementDirection()
    {
        if (!isMoving) {
            if (!(previousMovementSelection == WALK || previousMovementSelection == RUN))
            {
                this.movementDirection = Random.Range(0, 2);
                if (this.movementDirection == RIGHT && facingRight == false){ flipHorizontally(); }
                else if (this.movementDirection == LEFT && facingRight == true) { flipHorizontally(); }
            }
        }
    }


    // Change direction of movement if character reach left or right limit
    private void changeDirectionOnLimit()
    {

        if (this.transform.position.x <= leftLimit){ this.movementDirection = RIGHT; }
        else if(this.transform.position.x >= rightLimit){ this.movementDirection = LEFT; }

        if (this.movementDirection == RIGHT && facingRight == false) { flipHorizontally(); }
        else if (this.movementDirection == LEFT && facingRight == true) { flipHorizontally(); }
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

    private void flipHorizontally()
    {
        if (facingRight)
        {
            facingRight = false;
            remiSprite.transform.localRotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            facingRight = true;
            remiSprite.transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
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

        Bounds shadowBounds = shadowObj.GetComponent<BoxCollider2D>().bounds;

        if (shadowBounds.Contains(new Vector2(left, shadowBounds.center.y)) && shadowBounds.Contains(new Vector2(right, shadowBounds.center.y)))
        {   
            return true;
        }

        return false;      
    }


    private void changeSpriteColor(Color color)
    {
        remiSprite.GetComponent<SpriteRenderer>().color = color;
    }

}
