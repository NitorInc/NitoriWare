using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KogasaScareSuwakoBehavior : MonoBehaviour
{
    [SerializeField]
    private Animator hopAnimator;
    [SerializeField]
    private Animator victimAnimator;
    [SerializeField]
    private AnimationCurve hopCurve;
    [SerializeField]
    private float hopDuration = 1f;
    [SerializeField]
    private float hopHeightMult = 1f;
    [SerializeField]
    private Vector2 hopXRange;
    [Header("Duration of Hop Idle animation should match Hop Ground Time")]
    [SerializeField]
    private float hopGroundTime = .5f;

    private bool hopping;
    private float hopStartTime;
    private float hopStartX;
    private float hopEndX;
    private float groundY;

    private enum HopState
    {
        Grounded = 0,
        Up = 1,
        Down = 2
    }

    void Start ()
    {
        groundY = transform.position.y;
        InvokeRepeating("startHop", hopGroundTime, hopDuration + hopGroundTime);
	}

    void startHop()
    {
        hopStartTime = Time.time;
        hopStartX = transform.position.x;
        hopEndX = MathHelper.randomRangeFromVector(hopXRange);
        //hopEndX = transform.position.x + 3f;
        victimAnimator.SetInteger("direction", (int)Mathf.Sign(hopEndX - hopStartX));
        hopping = true;
        setHopState(HopState.Up);
    }
	
	void Update ()
    {
        if (hopping)
        {
            float t = (Time.time - hopStartTime) / hopDuration;
            if (t >= 1f)
            {
                t = 1f;
                hopping = false;
                setHopState(HopState.Grounded);
            }
            else if (t >= .5f && getHopState() != HopState.Down)
            {
                setHopState(HopState.Down);
            }

            transform.position = new Vector3(
                Mathf.Lerp(hopStartX, hopEndX, t),
                groundY + (hopCurve.Evaluate(t) * hopHeightMult),
                transform.position.z);
        }
	}

    void setHopState(HopState state)
    {
        hopAnimator.SetInteger("hopState", (int)state);
    }

    HopState getHopState()
    {
        return (HopState)hopAnimator.GetInteger("hopState");
    }
}
