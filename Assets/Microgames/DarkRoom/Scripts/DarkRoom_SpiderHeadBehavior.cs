using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkRoom_SpiderHeadBehavior : MonoBehaviour {

    [Header("Adjustables")]
    [SerializeField] private float highY;
    [SerializeField] private float lowY;
    [SerializeField] private float retreatSpeed;
    [SerializeField] private float lowerSpeed;
    [Header("Alarms | counts down by 1 per frame.")]
    [SerializeField] private float lowerDelay;

    [Header("GameObjects")]
    [SerializeField] private GameObject light;
    [SerializeField]
    private Animator rigAnimator;

    private Transform transformThread;

    private float lowerDelayTimer;

	/* Base methods */

	void Start () {
        // Initialization
        transformThread = transform.parent.Find("Thread");
	}
	
	void Update () {

        // Handle lowering
        if (lowerDelayTimer - 60 * Time.deltaTime > 0f)
            lowerDelayTimer -= 60 * Time.deltaTime;
        else
            Lower();
    }

    /* My methods */

    private void Retreat() {
        // Retreat up
        if (transform.position.y + retreatSpeed * Time.deltaTime >= highY) {
            transform.position          = new Vector3(transform.position.x, highY, transform.position.z);
            transformThread.localScale  = new Vector3(transformThread.localScale.x, 1, transformThread.localScale.z);
            rigAnimator.SetInteger("Direction", 0);
        } else {
            transform.position          += new Vector3(0f, retreatSpeed, 0f) * Time.deltaTime;
            transformThread.localScale  += new Vector3(0f, -retreatSpeed, 0f) * Time.deltaTime;
            rigAnimator.SetInteger("Direction", 1);
        }
    }

    private void Lower() {
        if ((light.transform.position - transform.position).magnitude < light.transform.localScale.x)
            return;

        // Lower.. down
        if (transform.position.y - retreatSpeed * Time.deltaTime <= lowY) {
            transform.position          = new Vector3(transform.position.x, lowY, transform.position.z);
            transformThread.localScale  = new Vector3(transformThread.localScale.x, highY - lowY + 1, transformThread.localScale.z);
            rigAnimator.SetInteger("Direction", 0);
        } else {
            transform.position          += new Vector3(0f, -lowerSpeed, 0f) * Time.deltaTime;
            transformThread.localScale  += new Vector3(0f, lowerSpeed, 0f) * Time.deltaTime;
            rigAnimator.SetInteger("Direction", -1);
        }
    }
    
    /* Collision handling */

    private void OnTriggerStay2D(Collider2D otherCollider) {
        GameObject other = otherCollider.gameObject;

        // WITH: Light
        if (other.name == "Light") {
            Retreat();
            lowerDelayTimer = lowerDelay;
        }

    }


    private void OnTriggerExit2D(Collider2D otherCollider)
    {
        GameObject other = otherCollider.gameObject;

        // WITH: Light
        if (other.name == "Light")
        {
            rigAnimator.SetInteger("Direction", 0);
            lowerDelayTimer = lowerDelay;
        }

    }

}
