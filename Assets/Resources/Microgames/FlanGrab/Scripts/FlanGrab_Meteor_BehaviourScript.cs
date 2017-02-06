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
    private ParticleSystem particleTrail;
    private FlanGrab_Microgame_Behaviour microgameBehaviour;
    private bool hasBeenDestroyed = false;
    private bool hasLanded = false;

    // Use this for initialization
    void Start () {
        this.theTransform = this.transform;
        this.startPoint = this.transform.position.x;
        this.particleTrail = GetComponentInChildren<ParticleSystem>();
        this.phase = Random.Range(0, Mathf.PI /2);
        this.microgameBehaviour = MicrogameController.instance.GetComponent<FlanGrab_Microgame_Behaviour>();
    }
	
	// Update is called once per frame
	void Update () {
        
        if ( MicrogameController.instance.getVictory() ) { 
            // Move downwards if has not been destroyed
            if (!hasBeenDestroyed)
            {
                fallingMovement();
                if (waveMovementIsOn) { waveMovement(); }
            }
        }

        else
        {
            // If game has ended, then destroy every meteor except the one that landed
            if (!hasLanded && !hasBeenDestroyed)
            {
                destroyMeteor();
            }
        }

        // Destroy meteor's gameobject when smoke trail is no longer active
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
        // Falling movement only affects the Y position component
        var moveVector = new Vector3(0, -1, 0);
        theTransform.position = theTransform.position + (moveVector * this.movementSpeed * Time.deltaTime);
    }

    void waveMovement()
    {
        // Wave movement follows a sine wave and only affects the X position component
        var omegaComponent = (Time.time * frequency + phase) % (2.0f * Mathf.PI);
        var xPosition = amplitude * Mathf.Sin( omegaComponent );
        theTransform.position = new Vector3(startPoint + xPosition, theTransform.position.y , theTransform.position.z);
    }

    public void meteorHasLanded()
    {
        // If meteor lands on ground, set the defeat.
        MicrogameController.instance.setVictory(false, true);
        this.hasLanded = true;

        // Shake camera
        Camera.main.GetComponent<CameraShake>().enabled = true;
    }

    void OnMouseDown()
    {
        // Meteor can be destroyed by the player only if he hasn't been defeated yet.
        if (MicrogameController.instance.getVictory())              
        {
            destroyMeteor();
        }
    }

    void destroyMeteor()
    {
        // Destroy Meteor's sprite but not the gameObject. When the ParticleSystem associated is no longer active, meteor's gameObject will be destroyed.
        this.hasBeenDestroyed = true;
        this.GetComponent<Collider2D>().enabled = false;
        this.GetComponentInChildren<ParticleSystem>().loop = false;
        Destroy(this.GetComponentInChildren<SpriteRenderer>().gameObject);
        this.microgameBehaviour.increaseDestructionCounter();
    }


}
