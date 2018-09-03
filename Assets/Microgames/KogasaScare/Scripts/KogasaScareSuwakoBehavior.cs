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
    private AudioClip hopClip;
    [SerializeField]
    private float hopDuration = 1f;
    [Header("Duration of Hop Idle animation should match Hop Ground Time")]
    [SerializeField]
    private float hopGroundTime = .5f;
    [SerializeField]
    private float hopHeightMult = 1f;
    [SerializeField]
    private Vector2 hopXRange;
    [SerializeField]
    private float fallGravity = 6f;
    [SerializeField]
    private float fallSpeed = 6f;
    [SerializeField]
    private GameObject genericSpriteObject;

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
        genericSpriteObject.SetActive(false);
        groundY = transform.position.y;
        InvokeRepeating("startHop", hopGroundTime, hopDuration + hopGroundTime);
	}

    void startHop()
    {
        hopStartTime = Time.time;
        hopStartX = transform.position.x;
        hopEndX = MathHelper.randomRangeFromVector(hopXRange);
        victimAnimator.SetInteger("direction", (int)Mathf.Sign(hopEndX - hopStartX));
        hopping = true;
        setHopState(HopState.Up);
        MicrogameController.instance.playSFX(hopClip, AudioHelper.getAudioPan(transform.position.x));
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

    void onScare(bool successful)
    {
        enabled = false;
        CancelInvoke();

        var suwaBody = GetComponent<Rigidbody2D>();
        suwaBody.bodyType = RigidbodyType2D.Dynamic;
        suwaBody.gravityScale = fallGravity;
        suwaBody.velocity = Vector2.down * fallSpeed;

        hopAnimator.gameObject.SetActive(false);
        genericSpriteObject.SetActive(true);
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
