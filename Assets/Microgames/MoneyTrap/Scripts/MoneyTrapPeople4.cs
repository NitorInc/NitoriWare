using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyTrapPeople4 : MonoBehaviour {

    //Controls movement for people objects

    [Header("Reference to target (Jewel)")]
    [SerializeField]
    private GameObject target;

    [Header("How fast person moves horizontaly")]
    [SerializeField]
    private float speedX = 10f;

    [Header("How tall the person jumps")]
    [SerializeField]
    private float speedY = 10f;

    [Header("Proximity threshold to follow")]
    [SerializeField]
    private float proximityFollow = 1f;

    //[Header("Distance threshold to stop following")]
    //[SerializeField]
    //private float distanceLeave = 1f;

    [Header("How fast person falls")]
    [SerializeField]
    private float fallSpeed = 1f;

    [Header("Starting facing direction")]
    [SerializeField]
    private bool isFacingRight;

    [Header("Jumping gravity")]
    [SerializeField]
    private float gravity = 0.1f;

    //Possible states for the person
    enum State {Idle, Following, Falling};

    //Stores this person's state
    private State state;
    //Stores the direction of movement
    private Vector2 trajectory;
    //Stores the direction of the jump
    private Vector2 jumpTrajectory;
    //Stores initial height
    private float floor;
    //Store last jump height (initial trajectory)
    private float lastJumpTrajY;
    //Store jump speedup progress
    private float speedup = 0;

    // Use this for initialization
    void Start () {
        //the person starts free
        state = State.Idle;
        floor = transform.position.y;
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        //if collides with trap
        if (other.gameObject.name == "Trap")
        {
            state = State.Falling;
            trajectory.x = other.transform.position.x;
            trajectory.y = -10f;
            transform.parent = null;
        }
    }

    // Update is called once per frame
    void Update ()
    { 
        //if this person is still not trapped
        if (state != State.Falling)
        {
            

            //if person is following
            if (state == State.Following) {

                //get the direction towards player from this object's position and turn sprite accordingly
                trajectory = (target.transform.position - transform.position).normalized;
                if (target.transform.position.x > transform.position.x)
                {
                    SpriteRenderer sr = gameObject.GetComponent<SpriteRenderer>();
                    sr.flipX = true;
                }
                else if (target.transform.position.x < transform.position.x)
                {
                    SpriteRenderer sr = gameObject.GetComponent<SpriteRenderer>();
                    sr.flipX = false;
                }

                //has to make a new jump if continues following
                if (isGrounded())
                {
                    //if person isn't too far away from the jewel
                    if (Mathf.Abs(transform.position.x - target.transform.position.x) < proximityFollow)
                    {
                        //move towards player's x position at defined speed
                        Vector2 newPosition = transform.position;
                        if (speedup < 1)
                            speedup = speedup + 0.2f;
                        newPosition.x = newPosition.x + trajectory.x * speedup * speedX * Time.deltaTime;  // only make a jump from scratch if not in continuous jumping

                        newPosition.y = newPosition.y + speedY * Time.deltaTime;
                        transform.position = newPosition;

                        jumpTrajectory = trajectory;
                        lastJumpTrajY = trajectory.y;
                    }
                    else
                    {
                        state = State.Idle;

                        speedup = 0f;
                        //make a stopping jump
                        jumpTrajectory = new Vector2(0f, lastJumpTrajY * 0.6f);
                        Vector2 newPosition = transform.position;
                        newPosition.x = newPosition.x + trajectory.x * speedX * Time.deltaTime;
                        newPosition.y = newPosition.y + trajectory.y * speedY * Time.deltaTime;
                        transform.position = newPosition;
                    }
                    
                }
                else
                {
                    if (speedup < 1)
                        speedup = speedup + 0.2f;
                    //animate jump in air
                    jumpTrajectory.y = jumpTrajectory.y - gravity;
                    jumpTrajectory.x = jumpTrajectory.x + trajectory.x * 0.2f;  //change direction in air
                    
                    //move towards player's x position at defined speed
                    Vector2 newPosition = transform.position;
                    newPosition.x = newPosition.x + jumpTrajectory.x * speedup * speedX * Time.deltaTime;
                    newPosition.y = newPosition.y + jumpTrajectory.y * speedY * Time.deltaTime;
                    //avoid clipping underground
                    newPosition.y = Mathf.Max(newPosition.y, floor);
                    transform.position = newPosition;
                }
            }
            //if person is idle and in range to follow target
            else if(state == State.Idle)
            {
                if(Mathf.Abs(transform.position.x - target.transform.position.x) < proximityFollow)
                    state = State.Following;
                else if (!isGrounded())
                {
                    //animate jump in air
                    jumpTrajectory.y = jumpTrajectory.y - gravity;

                    //move towards player's x position at defined speed
                    Vector2 newPosition = transform.position;
                    newPosition.x = newPosition.x + jumpTrajectory.x * speedX * Time.deltaTime;
                    newPosition.y = newPosition.y + jumpTrajectory.y * speedY * Time.deltaTime;
                    //avoid clipping underground
                    newPosition.y = Mathf.Max(newPosition.y, floor);
                    transform.position = newPosition;
                }
            }
        }

        //FALLING MOVEMENT ANIMATION
        //if this person was trapped/is falling
        else
        {
            //if not yet in touhou hell
            if (transform.position.y > -7f)
            {
                //move downwards into touhou hell

                Vector2 newPosition = transform.position;

                //grind x acceleration to a halt
                newPosition.x += Mathf.Lerp(newPosition.x, trajectory.x, 0.5f) * Time.deltaTime;
                newPosition.y += Mathf.Lerp(newPosition.y, trajectory.y, 0.5f) * fallSpeed * Time.deltaTime;
                //newPosition.y += trajectory.y * Time.deltaTime;
                //= (Vector2)transform.position + (Vector2.Lerp(transform.position, trajectory, 0.5f) * Time.deltaTime);
                transform.position = newPosition;
            }
        }
    }

    private bool isGrounded()
    {
        return transform.position.y <= floor;
    }
}
