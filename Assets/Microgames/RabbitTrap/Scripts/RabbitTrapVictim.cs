using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RabbitTrapVictim : MonoBehaviour {

    private readonly string pauseTag = "MicrogameTag1";
    private readonly string speedTag = "MicrogameTag2";

    private Animator walkAnimation;
    

    [Header("Travel speed")]
    [SerializeField]
    private float speed;

    [Header("Animation speed mod")]
    [SerializeField]
    private float animationSpeedMod = 0.5F;

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

    [SerializeField]
    private float[] stopsAndWaitTime;
    
    private Vector2 trajectory;

    private float maxXPosition;
    private Vector2 maxPosition;
    private bool victimOutOfBounds;

    private bool isTrapable;

    private enum trapStates {TooEarly, OnTime ,TooLate};
    private trapStates trapState;

    private bool isVictory = false;

    private float pauseTimeLeft = 0;
    
    // Use this for initialization
    void Start () {
        walkAnimation = gameObject.GetComponentInChildren<Animator>();
        trapState = trapStates.TooEarly;
        maxXPosition = -8;
        maxPosition = new Vector2(maxXPosition, 0);
        trajectory = new Vector2(speed,0);
        victimOutOfBounds = false;        
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

        if (!victimOutOfBounds)
        {
            if (pauseTimeLeft>0)
            {
                this.pauseTimeLeft = this.pauseTimeLeft - Time.deltaTime;
            } else
            {
                moveVictim();
                
            }
            setAnimationSpeed();

            victimOutOfBounds = IsVictimOutOfBounds();
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

    void moveVictim()
    {
        Vector2 newPosition = GetNewPosition();
        this.transform.position = newPosition;
    }

    void setAnimationSpeed()
    {
        if (this.pauseTimeLeft > 0)
        {
            print("Pause animation");
            this.walkAnimation.enabled = false;
        } else
        {
            print("Resume animation");
            this.walkAnimation.enabled = true;
            this.walkAnimation.speed = this.speed * animationSpeedMod;
        }
        
        
    }

    Vector2 GetNewPosition()
    {
        return (Vector2)transform.position + (trajectory * Time.deltaTime * -speed);
    }

    bool IsVictimOutOfBounds()
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

    void PauseVictimMovement(float pauseTime)
    {
        this.pauseTimeLeft = pauseTime;
    }

    void ChangeVictimSpeed(float newSpeed)
    {
        this.speed = newSpeed;
    }

    void OnTriggerEnter2D(Collider2D trigger)
    {
        if (trigger.tag==this.pauseTag)
        {
            RabbitTrapPauseTrigger pauseTrigger = trigger.gameObject.GetComponent<RabbitTrapPauseTrigger>();
            if (!pauseTrigger.HasTriggered)
            {
                this.PauseVictimMovement(pauseTrigger.PauseTime);
                pauseTrigger.HasTriggered = true;
            }

        }

        if (trigger.tag == speedTag)
        {
            RabbitTrapSpeedChangeTrigger speedTrigger = trigger.gameObject.GetComponent<RabbitTrapSpeedChangeTrigger>();
            if (!speedTrigger.HasTriggered)
            {
                this.ChangeVictimSpeed(speedTrigger.NewSpeed);
                speedTrigger.HasTriggered = true;
            }
        }

    }

}
