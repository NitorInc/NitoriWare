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
    [SerializeField]

    // Used for reverse logic only (don't shine light on)
    private SpriteRenderer[] reverseLogicEyeRenderers;
    [SerializeField]
    private Sprite reverseLogicRedEyeSprite;
    [SerializeField]
    private Color reverseLogicRedEyeColor;
    [SerializeField]
    private Sprite reverseLogicGrayEyeSprite;
    [SerializeField]
    private Color reverseLogicGrayEyeColor;

    private Transform transformThread;
    [SerializeField]
    private AudioClip raiseClip;
    [SerializeField]
    private AudioClip spotlightWarningClip;
    private float currentLowerSpeed;

    private float lowerDelayTimer;
    private float raiseDelayTimer;
    private AudioSource sfxSource;
    private bool inLight;
    public bool killedPlayer { get; set; }

    /* Base methods */

    void Start () {
        // Initialization
        transformThread = transform.parent.Find("Thread");
        sfxSource = GetComponent<AudioSource>();
	}
	
	void Update () {

        if (killedPlayer)
        {
            enabled = false;
            return;
        }
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

        foreach (var eyes in reverseLogicEyeRenderers)
        {
            eyes.sprite = inLight ? reverseLogicRedEyeSprite : reverseLogicGrayEyeSprite;
            eyes.color = inLight ? reverseLogicRedEyeColor : reverseLogicGrayEyeColor;
        }

        sfxSource.panStereo = AudioHelper.getAudioPan(transform.position.x);
    }

    /* My methods */

    private void Retreat() {
        // Retreat up

        var mult = 1f - (raiseDelayTimer / raiseDelay);
        var threadDiff = transform.position.y;

        var frameSpeed = retreatSpeed;
        if (killedPlayer)
            frameSpeed *= DarkRoomEffectAnimationController.instance.walkSpeed;

        transform.position = Vector3.MoveTowards(transform.position,
            new Vector3(transform.position.x, highY, transform.position.z),
            frameSpeed * Time.deltaTime * mult);
        threadDiff = transform.position.y - threadDiff;
        transformThread.localScale -= new Vector3(0f, threadDiff, 0f);

        if (MathHelper.Approximately(transform.position.y, highY, .01f))
            rigAnimator.SetInteger("Direction", 0);
        else
            rigAnimator.SetInteger("Direction", 1);

        if (transform.position.y >= highY) {
            //transform.position          = new Vector3(transform.position.x, highY, transform.position.z);
            //transformThread.localScale  = new Vector3(transformThread.localScale.x, 1, transformThread.localScale.z);
            rigAnimator.SetInteger("Direction", 0);
        } else {
        }
        currentLowerSpeed = 0f;
    }

    private void Lower() {
        //if ((light.transform.position - transform.position).magnitude < light.transform.localScale.x)
        //    return;

        currentLowerSpeed = Mathf.MoveTowards(currentLowerSpeed, lowerSpeed, lowerAcc * Time.deltaTime);

        var frameLowerSpeed = currentLowerSpeed;
        if (killedPlayer)
            frameLowerSpeed *= DarkRoomEffectAnimationController.instance.walkSpeed;

        // Lower.. down
        if (transform.position.y <= lowY) {
            //transform.position          = new Vector3(transform.position.x, lowY, transform.position.z);
            //transformThread.localScale  = new Vector3(transformThread.localScale.x, highY - lowY + 1, transformThread.localScale.z);
            rigAnimator.SetInteger("Direction", 0);
        } else {
            transform.position          += new Vector3(0f, -frameLowerSpeed, 0f) * Time.deltaTime;
            transformThread.localScale  += new Vector3(0f, frameLowerSpeed, 0f) * Time.deltaTime;
            rigAnimator.SetInteger("Direction", -1);
        }


        var threadDiff = transform.position.y;
        transform.position = Vector3.MoveTowards(transform.position,
            new Vector3(transform.position.x, lowY, transform.position.z),
            frameLowerSpeed * Time.deltaTime);
        threadDiff = transform.position.y - threadDiff;
        transformThread.localScale -= new Vector3(0f, threadDiff, 0f);


        if (MathHelper.Approximately(transform.position.y, lowY, .01f))
            rigAnimator.SetInteger("Direction", 0);
        else
            rigAnimator.SetInteger("Direction", -1);
    }
    
    /* Collision handling */

    private void OnTriggerStay2D(Collider2D otherCollider) {
        GameObject other = otherCollider.gameObject;

        // WITH: Light
        if (other.name == "Light") {
            Retreat();
            lowerDelayTimer = lowerDelay;
            if (!inLight)
            {
                sfxSource.PlayOneShot(raiseClip);
                if (spotlightWarningClip != null)
                    sfxSource.PlayOneShot(spotlightWarningClip);
            }
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
