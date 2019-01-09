using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RabbitTrapVictim : MonoBehaviour {

    [Header("Travel speed")]
    [SerializeField]
    private float speed;

    [Header("Trap hitbox")]
    [SerializeField]
    private GameObject trap;

    [Header("Too soon stop location")]
    [SerializeField]
    private float tooSoonStopLocation;

    [Header("On Time X stop location")]
    [SerializeField]
    private float onTimeXStopLocation;

    [Header("On Time Y stop location")]
    [SerializeField]
    private float onTimeYStopLocation;

    private Vector2 trajectory;

    private float maxXPosition;
    private Vector2 maxPosition;
    private bool stopMovement;

    private bool isTrapable;

    private enum trapStates {TooEarly, OnTime ,TooLate};
    private trapStates trapState;

    private bool isVictory = false;
    
    // Use this for initialization
    void Start () {
        print("Start");
        trapState = trapStates.TooEarly;
        maxXPosition = -8;
        maxPosition = new Vector2(maxXPosition, 0);
        trajectory = new Vector2(speed,0);
        stopMovement = false;        
}
	
	// Update is called once per frame
	void Update () {

        SetTrapable();

        if (Input.GetMouseButtonDown(0))
        {
            if (isTrapable)
            {
                SetVictory();
            } else
            {
                SetLose();
            }
        }        

        if (!stopMovement)
        {
            Vector2 newPosition = GetNewPosition();
            this.transform.position = newPosition;

            stopMovement = IsStopMovement();
        } else
        {
            if (this.isVictory)
            {
                trajectory = new Vector2(0,speed);
                Vector2 newPosition = GetNewPosition();
                this.transform.position = newPosition;
            }
        }
    }

    Vector2 GetNewPosition()
    {
        return (Vector2)transform.position + (trajectory * Time.deltaTime * -speed);
    }

    bool IsStopMovement()
    {
        if (this.transform.position.x<maxXPosition)
        {
            return true;
        } else
        {
            return false;
        }
    }

    void SetTrapable()
    {
        if (IsVictimTrapable())
        {
            isTrapable = true;
            trapState = trapStates.OnTime;
        }
        else
        {
            isTrapable = false;
            if (trapState==trapStates.OnTime)
            {
                trapState = trapStates.TooLate;
            }
        }
    }

    bool IsVictimTrapable()
    {
        if (this.GetComponent<Collider2D>().IsTouching(trap.GetComponent<Collider2D>()))
        {
            return true;
        } else
        {
            return false;
        }
    }

    void SetVictory()
    {
        this.isVictory = true;
        print("Victory!");
        maxXPosition = onTimeXStopLocation;
    }

    void SetLose()
    {
        if (trapState == trapStates.TooEarly)
        {
            SetLoseEarly();
        } else
        {
            SetLoseLate();
        }
    }

    void SetLoseEarly()
    {
        print("Too early!");
        maxXPosition = tooSoonStopLocation;        
    }

    void SetLoseLate()
    {
        print("Too late!");
    }

}
