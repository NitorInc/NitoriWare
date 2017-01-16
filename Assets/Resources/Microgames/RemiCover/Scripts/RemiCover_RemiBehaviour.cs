using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemiCover_RemiBehaviour : MonoBehaviour {

    public float movementSpeed;         // Speed of Remilia's movement (Walking)
    public float fasterMovementSpeed;   // Speed of Remilia's movement (Running)
    public float leftLimit;             // Minimum value of Remilia's X position that she can take
    public float rightLimit;            // Maximum value of Remilia's X position that she can take

    // Probabilities for choosing, randomly, different movements for Remilia (Walking, Standing and Running)
    private int walkProb = 45;          
    private int standProb = 45;         
    private int runProb = 10;

    private int lastSelection = 1;                  // Last movement selected (First movement is Standing)
    private const int WALK = 0;                     // Walk movement selection
    private const int STAND = 1;                    // Standing movement selection 
    private const int RUN = 2;                      // Run movement selection
    private int consecutiveMovementsCounter = 0;     // Counter for knowing if Remi's has been walking/running several times in a row


    private int selectionCounter = 21;   // Counter to vary the duration of the movements
    public int minSelectionCounter;     // Minimum value of selectionCounter
    public int maxSelectionCounter;     // Maximum value of selectionCounter

    private int movementDirection = 0;  // To specify where Remilia is moving (Left by default).
    private const int LEFT = 0;         // Left direction
    private const int RIGHT = 1;        // Right direction
  
    private GameObject[] shadowObjs = null;     // List of Gameobjects that in-game represents a Shadow
    private GameObject remiSprite = null;       // Sprite of Remi
    

	// Use this for initialization
	void Start () {
        Vector2 mousePosition = CameraHelper.getCursorPosition();
        this.transform.position = new Vector2(mousePosition.x, this.transform.position.y);
        this.remiSprite = transform.Find("RemiSprite").gameObject;
        this.shadowObjs = GameObject.FindGameObjectsWithTag("Shadow");
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void FixedUpdate()
    {

        // Move Remi
        moveRemilia();

        // Check If Remi is inside of umbrella:
        bool isUnderShadow = checkIfUnderShadow();

        // Do something if she is inside or not
        if (isUnderShadow)
        {
            changeSpriteColor(Color.white);
        }
        else
        {
            changeSpriteColor(Color.red);
        }
    }

    private void moveRemilia()
    {

        if (selectionCounter == 0) { 

            int rnd_number = Random.Range(1, 101);                                  // Random, number between 1 and 100 (For probabilities)

            if (rnd_number >= 1 && rnd_number <= walkProb){
                chooseDirection();
                walk_movement();
                lastSelection = WALK;
            }

            else if( rnd_number <= standProb + walkProb) {
                lastSelection = STAND;
                consecutiveMovementsCounter = 0;
                changeSpriteColor(Color.blue);
            }

            else
            {
                chooseDirection();
                run_movement();
                lastSelection = RUN;
            }

            selectionCounter = Random.Range(minSelectionCounter, maxSelectionCounter);
        }

        else
        {
            switch (lastSelection)
            {
                case WALK:
                    walk_movement();
                    break;

                case STAND:
                    changeSpriteColor(Color.blue);
                    break;

                case RUN:
                    run_movement();
                    break;
            }

            selectionCounter = selectionCounter - 1;
            if (selectionCounter == 0) { lastSelection = -1;  }
        }
    }

    private void walk_movement()
    {
        Bounds remiBounds = this.GetComponent<BoxCollider2D>().bounds;
        var move = new Vector3(0, 0, 0);
        switch(movementDirection)
        {
            case LEFT:
                move = new Vector3(-1, 0, 0);
                break;

            case RIGHT:
                move = new Vector3(1, 0, 0);
                break;
        }

        consecutiveMovementsCounter += 1;
        transform.position = transform.position + (move * movementSpeed * Time.deltaTime);
        changeDirection();

    }


    private void run_movement()
    {
        Bounds remiBounds = this.GetComponent<BoxCollider2D>().bounds;
        var move = new Vector3(0, 0, 0);
        switch (movementDirection)
        {
            case LEFT:
                move = new Vector3(-1, 0, 0);
                break;

            case RIGHT:
                move = new Vector3(1, 0, 0);
                break;
        }

        consecutiveMovementsCounter += 1;
        transform.position = transform.position + (move * fasterMovementSpeed * Time.deltaTime);
        changeDirection();

    }



    private void chooseDirection()
    {
       if (consecutiveMovementsCounter == 0)
        {
            this.movementDirection = Random.Range(0, 2);
        }
    }

    private void changeDirection()
    {
        if (transform.position.x <= leftLimit)
        {
            movementDirection = RIGHT;
        }

        else if( transform.position.x >= rightLimit)
        {
            movementDirection = LEFT;
        }
    }


    private void limitMovement()
    {

        if (transform.position.x < leftLimit) {
            transform.position = new Vector2(leftLimit, transform.position.y);
        }

        else if (transform.position.x > rightLimit) {
            transform.position = new Vector2(rightLimit, transform.position.y);
        }

    }


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
