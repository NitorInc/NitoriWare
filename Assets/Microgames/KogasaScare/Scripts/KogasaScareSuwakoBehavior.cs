using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KogasaScareSuwakoBehavior : MonoBehaviour
{
    [SerializeField]
    private Animator hopAnimator;
    [SerializeField]
    private AnimationCurve hopCurve;
    [SerializeField]
    private float hopDuration = 1f;
    [SerializeField]
    private float hopGroundTime = .5f;
    [SerializeField]
    private float hopHeightMult = 1f;
    [SerializeField]
    private Vector2 hopXRange;

    private bool hopping;
    private float hopStartTime;
    private float hopStartX;
    private float hopEndX;
    private float groundY;
    
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
        hopping = true;
        hopAnimator.SetInteger("HopState", 1);
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
                hopAnimator.SetInteger("HopState", 2);
            }
            transform.position = new Vector3(
                Mathf.Lerp(hopStartX, hopEndX, t),
                groundY + (hopCurve.Evaluate(t) * hopHeightMult),
                transform.position.z);
        }
	}
}
