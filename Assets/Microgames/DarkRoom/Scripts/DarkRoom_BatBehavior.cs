
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkRoom_BatBehavior : MonoBehaviour {

    [Header("Adjustables")]
    [SerializeField]
    private Vector2 advanceDirection;
    [SerializeField]
    private float advanceSpeed;
    [SerializeField]
    private float retreatSpeed;
    [SerializeField]
    private float retreatCooldown = .75f;
    [SerializeField]
    private float activateDistance = 8f;
    [SerializeField]
    private Animator rigAnimator;
    //[SerializeField] private float speed;
    //[SerializeField] private float flySinAmplitude;
    //[SerializeField] private float flySinSpeed;
    //[SerializeField] private float flyAwayAccel;
    [Header("Alarms | counts down by 1 per frame.")]
    [SerializeField] private float health;

    [Header("GameObjects")]
    [SerializeField] private GameObject renko;
    [SerializeField]
    private AudioSource sfxSource;
    [SerializeField]
    private AudioSource scaredSource;
    [SerializeField]
    private AudioClip appearClip;
    [SerializeField]
    private float volumeMult = 1f;

    private ParticleSystem myParticleSystem;

    private bool isActive = false;
    bool isRetreating = false;
    private float flyDistance;
    private float flyAngle;
    private float retreatCooldownTimer;

    private bool hasFlownAway = false;
    private float flyAwayDirection;
    private float flyAwayComponentX;
    private float flyAwayComponentY;
    private float flyAwaySpeed = 1f;

    /* Base methods */

    void Start () {
        // Initialization
        myParticleSystem = GetComponentInChildren<ParticleSystem>();

        flyAwayDirection = Random.Range(0.05f, 0.20f) * Mathf.PI;
        flyAwayComponentX = Mathf.Cos(flyAwayDirection);
        flyAwayComponentY = Mathf.Sin(flyAwayDirection);
	}

	void Update () {

        // Handle state
        if (!isActive) {
            if (transform.position.x - renko.transform.position.x < activateDistance) {
                // Activate and follow Renko
                isActive = true;
                myParticleSystem.Play();
                rigAnimator.SetTrigger("Activate");
                GetComponent<DarkRoomInstrumentDistance>().enabled = true;
                sfxSource.PlayOneShot(appearClip);
                //transform.parent = renko.transform;
                //flyDistance = (transform.position - renko.transform.position).magnitude;
                //flyAngle = MathHelper.getAngle(transform.position - renko.transform.position) * Mathf.Deg2Rad;
            } else return;
        }

        sfxSource.panStereo = sfxSource.panStereo = AudioHelper.getAudioPan(transform.position.x);
        scaredSource.volume = retreatCooldownTimer * volumeMult;
        if (scaredSource.volume > 0f && !scaredSource.isPlaying)
            scaredSource.Play();
        else if (scaredSource.volume <= 0f && scaredSource.isPlaying)
            scaredSource.Stop();

        // Handle flying
        if (isActive)
            Fly();

	}

    /* My methods */

    private void Fly() {
        // Fly
        //float targetX = flyDistance * Mathf.Cos(flyAngle);
        //float targetY = flyDistance * Mathf.Sin(flyAngle);
        //transform.position = new Vector3(targetX, targetY, transform.position.z);

        if (!isRetreating && retreatCooldownTimer <= 0f)
            transform.position += (Vector3)advanceDirection.resize(advanceSpeed) * Time.deltaTime;
        else
            transform.position += (Vector3)advanceDirection.resize(-retreatSpeed) * Time.deltaTime * retreatCooldownTimer;

        if (!isRetreating)
            retreatCooldownTimer -= Time.deltaTime;
    }
    
    /* Collision handling */

    private void OnTriggerStay2D(Collider2D otherCollider)
    {
        if (!isActive)
            return;
        GameObject other = otherCollider.gameObject;

        // WITH: Light
        if (other.name == "Light") {
            isRetreating = true;
            retreatCooldownTimer = retreatCooldown;
        }

    }

    private void OnTriggerEnter2D(Collider2D otherCollider)
    {
        if (!isActive)
            return;
        GameObject other = otherCollider.gameObject;

        // WITH: Light
        if (other.name == "Light")
        {
            isRetreating = true;
            retreatCooldownTimer = retreatCooldown;
        }

    }

    private void OnTriggerExit2D(Collider2D otherCollider) {
        if (!isActive)
            return;
        GameObject other = otherCollider.gameObject;

        // WITH: Light
        if (other.name == "Light")
        {
            //myParticleSystem.Stop();
            retreatCooldownTimer = retreatCooldown;
            isRetreating = false;
        }

    }

    /* Getters and setters */

    public bool HasFlownAway { get { return hasFlownAway; } set { hasFlownAway = value; } }

}
