
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkRoom_BatBehavior : MonoBehaviour {

    [Header("Adjustables")]
    [SerializeField] private float speed;
    [SerializeField] private float flySinAmplitude;
    [SerializeField] private float flySinSpeed;
    [SerializeField] private float flyAwayAccel;
    [Header("Alarms | counts down by 1 per frame.")]
    [SerializeField] private float health;

    [Header("GameObjects")]
    [SerializeField] private GameObject renko;

    private ParticleSystem myParticleSystem;

    private bool isActive = false;
    private float flyDistance;
    private float flyAngle;

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
            if (transform.position.x - renko.transform.position.x < 8f) {
                // Activate and follow Renko
                isActive = true;
                transform.parent.parent = renko.transform;
                flyDistance = (transform.position - renko.transform.position).magnitude;
                flyAngle = MathHelper.getAngle(transform.position - renko.transform.position) * Mathf.Deg2Rad;
            } else return;
        }

        // Handle flying
        Fly();
        flyDistance -= 3 * Time.deltaTime;

	}

    /* My methods */

    private void Fly() {
        // Fly
        float targetX = flyDistance * Mathf.Cos(flyAngle);
        float targetY = flyDistance * Mathf.Sin(flyAngle);
        transform.position = new Vector3(targetX, targetY, transform.position.z);
    }
    
    private void FlyAway() {
        // Fly away
        flyAwaySpeed += flyAwayAccel * Time.deltaTime;
        transform.position += new Vector3(flyAwayComponentX, flyAwayComponentY, 0f) * flyAwaySpeed * Time.deltaTime;
    }
    
    /* Collision handling */

    private void OnTriggerStay2D(Collider2D otherCollider) {
        GameObject other = otherCollider.gameObject;

        // WITH: Light
        if (other.name == "Light") {
            flyDistance += 6 * Time.deltaTime;
        }

    }

    private void OnTriggerEnter2D(Collider2D otherCollider) {
        GameObject other = otherCollider.gameObject;

        // WITH: Light
        if (other.name == "Light") {
            myParticleSystem.Play();
        }

    }

    private void OnTriggerExit2D(Collider2D otherCollider) {
        GameObject other = otherCollider.gameObject;

        // WITH: Light
        if (other.name == "Light") {
            myParticleSystem.Stop();
        }

    }

    /* Getters and setters */

    public bool HasFlownAway { get { return hasFlownAway; } set { hasFlownAway = value; } }

}
