using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemiCover_Remilia_MovementBehaviour : MonoBehaviour {

    public float walkingSpeed;                      // Speed of walking movement.
    public float runningSpeed;                      // Speed of running movement.
    private float currentSpeed;                     // currentSpeed (Only useful for Running movement acceleration)
    public float accelerationFactor;                // How much will CurrentSpeed increase until reaching runningSpeed?
    public float leftLimit, rightLimit;             // Minimum and Maximum x-position of movement.

    // Probabilities for randomly choosing different movements for Remilia
    public int walkProbability;
    public int standProbability;
    public int runningProbabilty;
    public int teleportProbability;
    private int totalProbabilityValue;
    public float teleportImmunityDelay;

    // Movement actions
    private enum movements { NONE, WALK, STAND, RUN, TELEPORT }     // Movements available
    private int currentMovementSelection = (int)movements.NONE;     // Last movement selected
    private int previousMovementSelection = (int)movements.NONE;    // Previous movement selected
    private float movementTimer = 0;                                // How long will the selected movement be performed? (Will be assigned Randomly).
    public float initialMovementDuration;                           // Duration of the first movement performed, by default is STAND movement.
    public float minimumMovementDuration;                           // Minimum value that the movement timer can take (when is initialized).
    public float maximumMovementDuration;                           // Maximum value that the movement timer can take (when is initialized).
    public float minTeleportDistance;                               // Minimun distance that must be travelled during teleport movement

    // Movement Directions
    private enum movementDirections { LEFT, RIGHT }                             // To specify if gameObject is facing left or right (Left by default).
    private int currentMovementDirection = (int)movementDirections.LEFT;        // Current movement direction
    private bool facingRight = true;                                            // To check if gameObject is facing to the right.

    // Other Components
    private Animator animator;
    private RemiCover_Remi_HealthBehaviour healthScript;

    public AudioClip teleportSFX_reappear;
    public AudioClip teleportSFX_disappear;

    void Start()
    {
        totalProbabilityValue = standProbability + walkProbability + runningProbabilty + teleportProbability;
        animator = GetComponent<Animator>();
        healthScript = GetComponent<RemiCover_Remi_HealthBehaviour>();
        setInitialPosition();
        setInitialMovement((int)movements.STAND, initialMovementDuration);
    }

    // Update is called once per frame
    void Update()
    {
        if (!MicrogameController.instance.getVictoryDetermined() && MicrogameController.instance.session.BeatsRemaining >= .5f)
            moveCharacter();
        else if (MicrogameController.instance.session.BeatsRemaining < .5f) { 
            Debug.Log(animator.GetInteger("MovementAnimation"));
            if (animator.GetInteger("MovementAnimation") != 4) // If not Teleport
                animator.SetInteger("MovementAnimation", (int)movements.STAND);

        }
    }



    // Perform the current movement selected. If no movement is selected, then choose randomly a movement to perform.
    private void moveCharacter()
    {
        if (currentMovementSelection == (int)movements.NONE)
            chooseMovement();
        performMovement();
    }

    // Choose a movement to perform. Standing and Teleport movement won't be chosen twice in a row.
    private void chooseMovement()
    {
        int rnd_number = randomNumberForMovementSelection();
        updateCurrentMovementSelection(rnd_number);
        updateMovementDirection();
        newMovementTimer();
    }

    // Perform the currently selected movement
    private void performMovement()
    {

        updateMovementAnimation();

        switch (currentMovementSelection)
        {
            case (int)movements.WALK:
                performWalkMovement();
                break;

            case (int)movements.STAND:
                performStandingMovement();
                break;

            case (int)movements.TELEPORT:
                performTeleportMovement();
                break;

            case (int)movements.RUN:
                performRunMovement();
                break;
        }

        updateMovementTimer();

    }

    /// Perform Walking Movement
    private void performWalkMovement()
    {
        var move = obtainMovementVector3();
        transform.position = transform.position + (move * walkingSpeed * Time.deltaTime);
        currentSpeed = walkingSpeed;
        changeDirectionOnLimit();

    }

    // Perform Standing Movement
    private void performStandingMovement()
    {
        currentSpeed = 0;
    }

    // Perform Running movement
    private void performRunMovement()
    {
        var move = obtainMovementVector3();

        if (currentSpeed == 0)
            currentSpeed = walkingSpeed;
        else if (currentSpeed < runningSpeed)
            currentSpeed += accelerationFactor;
        transform.position = transform.position + (move * currentSpeed * Time.deltaTime);

        changeDirectionOnLimit();
    }

    // Perform Teleport movement (The movement is performed by the animator)
    public void performTeleportMovement()
    {
        if (healthScript.isActiveAndEnabled)
            healthScript.setInmunnity(true);
    }

    private void playReappearTeleportSound()
    {
        MicrogameController.instance.playSFX(teleportSFX_reappear);
    }

    private void playDisappearTeleportSound()
    {
        MicrogameController.instance.playSFX(teleportSFX_disappear);
    }

    // Change the position of the character randomly. If the new position is near the previous position, then the new position is moved a little.
    // Used by animator to change gameobject position.
    private void changePosition()
    {

        bool teleportToTheLeftPossibility = checkIfLeftTeleportIsPossible();
        bool teleportToTheRightPossibility = checkIfRightTeleportIsPossible();
        float newXPosition;

        if (teleportToTheLeftPossibility && teleportToTheRightPossibility)
        {
            bool teleportToTheLeft = randomBoolean();
            if (teleportToTheLeft)
                newXPosition = randomPositionAfterLeftTeleport();
            else
                newXPosition = randomPositionAfterRightTeleport();
        }

        else if (teleportToTheLeftPossibility)
            newXPosition = randomPositionAfterLeftTeleport();

        else if (teleportToTheRightPossibility)
            newXPosition = randomPositionAfterRightTeleport();

        else
            newXPosition = getFurthestPosition();

        transform.position = new Vector2(newXPosition, transform.position.y);
    }

    // Gets called by animtaor when teleport movement has ended
    public void endTeleportMovement()
    {
        if (healthScript.isActiveAndEnabled)
            Invoke("stopImmunity", teleportImmunityDelay);

        resetMovementSelectionParameters();
        currentSpeed = 0;


    }

    void stopImmunity()
    {
        healthScript.setInmunnity(false);
    }

    // Check if gameobject needs to be fliped in order to face the current movement direction.
    private bool DoesItNeedsToFlip()
    {
        return (currentMovementDirection == (int)movementDirections.RIGHT && facingRight == false) || (currentMovementDirection == (int)movementDirections.LEFT && facingRight == true);
    }

    // Change movement direction if object reachs the left or right limits.
    private void changeDirectionOnLimit()
    {
        if (transform.position.x <= leftLimit)
            currentMovementDirection = (int)movementDirections.RIGHT;
        else if (transform.position.x >= rightLimit)
            currentMovementDirection = (int)movementDirections.LEFT;
        if (DoesItNeedsToFlip()) flipHorizontally();
    }

    // Generate a movement vector that depends on the direction of the current movement.
    private Vector3 obtainMovementVector3()
    {
        var move = new Vector3(0, 0, 0);
        switch (currentMovementDirection)
        {
            case (int)movementDirections.LEFT:
                move = new Vector3(-1, 0, 0);
                break;

            case (int)movementDirections.RIGHT:
                move = new Vector3(1, 0, 0);
                break;
        }
        return move;
    }

    // Flip gameobject Horizontally
    private void flipHorizontally()
    {
        transform.Rotate(new Vector3(0, 180, 0));
        facingRight = !facingRight;
    }

    // Limit character´s movement according to left and right limit
    private void limitMovement()
    {
        transform.position = new Vector2(Mathf.Clamp(leftLimit, transform.position.x, rightLimit), transform.position.y);
    }

    // Generate a random movement direction selection ( 0 -> LEFT, 1 -> RIGHT )
    private int randomDirection()
    {
        return Random.Range(0, 2);
    }

    // Generate a random Boolean value
    private bool randomBoolean()
    {
        return Random.value >= 0.5;
    }

    // Generate a random number for movement selection
    private int randomNumberForMovementSelection()
    {
        int rnd_number;

        if (previousMovementSelection == (int)movements.STAND)                               // Standing won't be chosen twice in a row
            rnd_number = Random.Range(0, totalProbabilityValue - standProbability + 1);          
        else if (previousMovementSelection == (int)movements.TELEPORT)                       // Teleport won't be chosen twice in a row.
            rnd_number = Random.Range(0, totalProbabilityValue - teleportProbability + 1);       
        else
            rnd_number = Random.Range(0, totalProbabilityValue + 1);

        return rnd_number;
    }

    // Generate a random number for movement duration
    private float randomNumberForMovementDuration()
    {
        return Random.Range(minimumMovementDuration, maximumMovementDuration);
    }

    // Generate a random number for x-position after left teleport movement
    private float randomPositionAfterLeftTeleport()
    {
        return Random.Range(leftLimit, transform.position.x - minTeleportDistance);
    }

    // Generate a random number for x-position after right teleport movement
    private float randomPositionAfterRightTeleport()
    {
        return Random.Range(transform.position.x + minTeleportDistance, rightLimit);
    }

    // Get the furthest position possible from the current object position
    private float getFurthestPosition()
    {
        float leftLimitDistance = Mathf.Abs(transform.position.x - leftLimit);
        float rightLimitDistance = Mathf.Abs(transform.position.x - rightLimit);
        if (leftLimitDistance < rightLimitDistance)
            return transform.position.x + rightLimitDistance;
        else
            return transform.position.x - leftLimitDistance;
    }

    // Set the initial position of gameObject according to the position of the cursor
    private void setInitialPosition()
    {
        Vector2 mousePosition = CameraHelper.getCursorPosition();
        transform.position = new Vector2(mousePosition.x, transform.position.y);
    }

    // Set the initial movement and the time for which is going to make that movement
    private void setInitialMovement(int movement, float timer)
    {
        currentMovementSelection = movement;
        movementTimer = timer;
        previousMovementSelection = (int)movements.NONE;
    }

    // Check if Walk movement has been selected. 
    private bool hasWalkingBeenSelected(int number)
    {
        return (number > 0 && number <= walkProbability);
    }

    // Check if Running movement has been selected.
    private bool hasRunningBeenSelected(int number)
    {
        return (number > walkProbability && number <= (walkProbability + runningProbabilty));
    }

    // Check if Teleport movement has been selected.
    private bool hasTeleportBeenSelected(int number)
    {
        if (previousMovementSelection == (int)movements.TELEPORT)  // Teleport won't be chosen twice in a row.
            return false;
        else
            return (number > (walkProbability + runningProbabilty) && number <= (walkProbability + runningProbabilty + teleportProbability));
    }

    // Check if Standing movement has been selected.    
    private bool hasStandingBeenSelected(int number)
    {
        if (previousMovementSelection == (int)movements.STAND)  // Standing won't be chosen twice in a row.
            return false;
        else
            if (previousMovementSelection == (int)movements.STAND)
                return (number > (walkProbability + runningProbabilty) && number <= totalProbabilityValue);
            else
                return (number > (walkProbability + runningProbabilty + teleportProbability) && number <= totalProbabilityValue);
    }

    // Check if gameobject can teleport to the left
    private bool checkIfLeftTeleportIsPossible()
    {
        return (transform.position.x - minTeleportDistance >= leftLimit);
    }

    // Check if gameobject can teleport to the right
    private bool checkIfRightTeleportIsPossible()
    {
        return (transform.position.x + minTeleportDistance <= rightLimit);
    }

    // Choose the new current movement according to a number between ]0, totalProbabilityValue]
    private void updateCurrentMovementSelection(int rnd_number)
    {
        if (hasWalkingBeenSelected(rnd_number))
            currentMovementSelection = (int)movements.WALK;
        else if (hasRunningBeenSelected(rnd_number))
            currentMovementSelection = (int)movements.RUN;
        else if (hasTeleportBeenSelected(rnd_number))
            currentMovementSelection = (int)movements.TELEPORT;
        else if (hasStandingBeenSelected(rnd_number))
            currentMovementSelection = (int)movements.STAND;
        else
            currentMovementSelection = (int)movements.NONE;
    }

    // Choose randomly a direction (Left or Right) which the gameobject will follow. A new direction won't be chosen if the character was walking or running previously
    private void updateMovementDirection()
    {
        if (previousMovementSelection != (int)movements.WALK && previousMovementSelection != (int)movements.RUN)
        {
            if (currentMovementSelection == (int)movements.WALK || currentMovementSelection == (int)movements.RUN)
            {
                currentMovementDirection = randomDirection();
                if (DoesItNeedsToFlip())
                    flipHorizontally();
            }
        }
    }

    // Change movement animation according to current movement selection
    private void updateMovementAnimation()
    {
        // Movement animation:
        // 1 -> WALK
        // 2 -> IDLE
        // 3 -> RUN
        // 4 -> TELEPORT
        if (currentMovementSelection == (int)movements.NONE)
            animator.SetInteger("MovementAnimation", 2);
        else
            animator.SetInteger("MovementAnimation", currentMovementSelection);
    }

    // Update movement timer
    private void updateMovementTimer()
    {
        if (currentMovementSelection != (int)movements.TELEPORT) // Teleport is not affected by timer.
        {
            movementTimer = movementTimer - Time.deltaTime;
            if (movementTimer <= 0)
            {
                resetMovementSelectionParameters();
            }
        }
    }

    // Choose randomly a new movement timer.
    private void newMovementTimer()
    {
        movementTimer = randomNumberForMovementDuration();
    }

    // Update previous movement selection, set current movement to None and reset Movement Timer
    public void resetMovementSelectionParameters()
    {
        previousMovementSelection = currentMovementSelection;
        currentMovementSelection = (int)movements.NONE;
        animator.SetInteger("MovementAnimation", 2); // Standing animation
        movementTimer = 0;
    }


}
