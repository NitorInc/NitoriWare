using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkRoom_ChimeraBehavior : MonoBehaviour {

    [Header("Adjustables")]
    [SerializeField] private float walkSpeed;
    [Header("Alarms | counts down by 1 per frame.")]
    [SerializeField] private float healthMax;
    [SerializeField] private float fleeDistance = 8f;
    [SerializeField]
    private float fleeDelay = .5f;
    [SerializeField]
    private AudioClip scaredClip;
    [SerializeField]
    private float hurtableDistance = 9f;
    [SerializeField]
    private float eatDistance = -2f;

    [Header("GameObjects")]
    [SerializeField] private GameObject renko;

    private Animator myAnimator;

    private float health;
    private float fleeDelayTimer;

    private bool isFleeing = false;
    bool dead = false;
    private Vector2 fleeEndPosition;

	/* Base methods */

	void Start () {
        // Initialization
        myAnimator = GetComponentInChildren<Animator>();
        health = healthMax;
    }
	
	void Update () {

        // Handle movement
        if (isFleeing)
        {
            Flee();
        } else
        {
            Walk();
        }
        

	}

    /* My methods */

    private void Walk() {
        // Walk forward
        transform.position += new Vector3(walkSpeed, 0f, 0f) * Time.deltaTime;
        if (!dead
            && transform.position.x > renko.transform.position.x + eatDistance)
        {
            myAnimator.SetTrigger("Eat");
            walkSpeed /= 2f;
            DarkRoom_RenkoBehavior.instance.Fail();
            dead = true;
        }
    }

    private void Flee() {
        // Flee backwards
        if (fleeDelayTimer > 0f)
            fleeDelayTimer -= Time.deltaTime;
        else if (transform.moveTowards2D(fleeEndPosition, 32))
            isFleeing = false;
    }

    /* Collision handling */

    private void OnTriggerStay2D(Collider2D otherCollider) {
        GameObject other = otherCollider.gameObject;

        // WITH: Light
        if (other.name == "Light") {
            if (myAnimator.GetCurrentAnimatorStateInfo(0).IsName("animChimeraWalk")) {
                // Decrease health
                if (health - 60 * Time.deltaTime > 0f) {
                    if (transform.position.x > renko.transform.position.x - hurtableDistance)
                        health -= 60 * Time.deltaTime;
                } else {
                    health = healthMax;
                    myAnimator.SetTrigger("isShined");
                    fleeEndPosition = transform.position - new Vector3(fleeDistance, 0f, 0f);
                    isFleeing = true;
                    fleeDelayTimer = fleeDelay;
                    MicrogameController.instance.playSFX(scaredClip, AudioHelper.getAudioPan(transform.position.x));
                }
            }

        }

    }

}
