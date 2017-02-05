using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlanGrab_Meteor_BehaviourScript : MonoBehaviour {

    public float movementSpeed;           
    public float leftLimit;
    public float rightLimit;

    // Only useful when waveMovementIsOn = True;
    public bool waveMovementIsOn;           // If meteor will move following a Sine wave or not.
    public float frequency;                 // Sine wave frequency
    public float amplitude;                 // Sine wave amplitude
    private float phase = 0;                // Sine wave phase
    private float startPoint = 0;           // Sine wave center position

    private Transform theTransform;
    private bool isAlive = true;
    private bool hasLanded = false;
    private bool hasBeenDestroyed = false;
    private ParticleSystem particleTrail;

    // Use this for initialization
    void Start () {
        this.theTransform = this.transform;
        this.startPoint = this.transform.position.x;
        this.particleTrail = GetComponentInChildren<ParticleSystem>();
        phase = Random.Range(0, 2 * Mathf.PI);
    }
	
	// Update is called once per frame
	void Update () {

        // Move downwards if meteor has not landed or has not been destroyed
        if (!hasLanded && !hasBeenDestroyed)
        {
            fallingMovement();
            if (waveMovementIsOn) { waveMovement(); }
        }

        // Destroy gameobject when smoke trail is no longer active
        if (hasBeenDestroyed)
        {
           if (!this.particleTrail.IsAlive())
            {
                Destroy(theTransform.gameObject);
            }
        }


    }

    void fallingMovement()
    {
        var moveVector = new Vector3(0, -1, 0);
        theTransform.position = theTransform.position + (moveVector * this.movementSpeed * Time.deltaTime);
    }

    void waveMovement()
    {
        var omegaComponent = (Time.time * frequency + phase) % (2.0f * Mathf.PI);
        var xPosition = amplitude * Mathf.Sin( omegaComponent );
        theTransform.position = new Vector3(startPoint + xPosition, theTransform.position.y , theTransform.position.z);
    }

    public void meteorHasLanded()
    {
        this.hasLanded = true;
        this.GetComponent<Collider2D>().enabled = false;


    }

    void OnMouseDown()
    {
        if (!this.hasLanded)
        {
            destroyMeteor();
        }
    }

    void destroyMeteor()
    {
        this.hasBeenDestroyed = true;
        this.GetComponent<Collider2D>().enabled = false;
        this.GetComponentInChildren<ParticleSystem>().loop = false;
        Destroy(this.GetComponentInChildren<SpriteRenderer>().gameObject);
    }


}
