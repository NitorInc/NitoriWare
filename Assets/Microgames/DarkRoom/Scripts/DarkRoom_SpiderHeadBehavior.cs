using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkRoom_SpiderHeadBehavior : MonoBehaviour {

    [Header("Adjustables")]
    [SerializeField] private float highY;
    [SerializeField] private float lowY;
    [SerializeField] private float retreatSpeed;
    [SerializeField] private float lowerSpeed;
    [SerializeField] private float lowerAcc;
    [Header("Alarms | counts down by 1 per frame.")]
    [SerializeField] private float lowerDelay;
    [SerializeField] private float raiseDelay;

    [Header("GameObjects")]
    [SerializeField] private GameObject light;
    [SerializeField]
    private Animator rigAnimator;

    private Transform transformThread;
    [SerializeField]
    private AudioClip raiseClip;
    private float currentLowerSpeed;

    private float lowerDelayTimer;
    private float raiseDelayTimer;
    private AudioSource sfxSource;
    private bool inLight;

	/* Base methods */

	void Start () {
        // Initialization
        transformThread = transform.parent.Find("Thread");
        sfxSource = GetComponent<AudioSource>();
	}
	
	void Update () {

        if (inLight)
        {
            raiseDelayTimer -= 60f * Time.deltaTime;
            raiseDelayTimer = Mathf.Max(raiseDelayTimer, 0f);
        }
        else
            raiseDelayTimer = raiseDelay;

        // Handle lowering
        if (lowerDelayTimer - 60 * Time.deltaTime > 0f)
        {
            lowerDelayTimer -= 60 * Time.deltaTime;
            lowerDelayTimer = Mathf.Max(lowerDelayTimer, 0f);
        }
        else
            Lower();

        sfxSource.panStereo = AudioHelper.getAudioPan(transform.position.x);
    }

    /* My methods */

    private void Retreat() {
        // Retreat up

        if (transform.position.y + retreatSpeed * Time.deltaTime >= highY) {
            transform.position          = new Vector3(transform.position.x, highY, transform.position.z);
            transformThread.localScale  = new Vector3(transformThread.localScale.x, 1, transformThread.localScale.z);
            rigAnimator.SetInteger("Direction", 0);
        } else {
            var mult = 1f - (raiseDelayTimer / raiseDelay);
            transform.position          += new Vector3(0f, retreatSpeed, 0f) * Time.deltaTime * mult;
            transformThread.localScale  += new Vector3(0f, -retreatSpeed, 0f) * Time.deltaTime * mult;
            rigAnimator.SetInteger("Direction", 1);
        }
        currentLowerSpeed = 0f;
    }

    private void Lower() {
        //if ((light.transform.position - transform.position).magnitude < light.transform.localScale.x)
        //    return;

        currentLowerSpeed = Mathf.MoveTowards(currentLowerSpeed, lowerSpeed, lowerAcc * Time.deltaTime);

        // Lower.. down
        if (transform.position.y - currentLowerSpeed * Time.deltaTime <= lowY) {
            transform.position          = new Vector3(transform.position.x, lowY, transform.position.z);
            transformThread.localScale  = new Vector3(transformThread.localScale.x, highY - lowY + 1, transformThread.localScale.z);
            rigAnimator.SetInteger("Direction", 0);
        } else {
            transform.position          += new Vector3(0f, -currentLowerSpeed, 0f) * Time.deltaTime;
            transformThread.localScale  += new Vector3(0f, currentLowerSpeed, 0f) * Time.deltaTime;
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
            if (!inLight)
                sfxSource.PlayOneShot(raiseClip);
            inLight = true;
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
            inLight = false;
        }

    }

}
