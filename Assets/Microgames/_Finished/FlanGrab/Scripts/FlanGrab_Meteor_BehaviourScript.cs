using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlanGrab_Meteor_BehaviourScript : MonoBehaviour {

    public float movementSpeed;
    public float rotationSpeed;
    public float xLeftLimit;
    public float xRightLimit;
    public float yLowerLimit;

    private Vector2 endingPosition;         

    public bool diagonalMovementIsOn;       // If meteor will move diagonally or not.

    public bool waveMovementIsOn;           // If meteor will move following a Sine wave or not.
    public float frequency;                 // Sine wave frequency (Only useful when waveMovementIsOn = True).
    public float amplitude;                 // Sine wave amplitude  (Only useful when waveMovementIsOn = True).
    private float phase = 0;                // Sine wave phase  (Only useful when waveMovementIsOn = True).
    private Vector2 originalPosition;       // Meteor position if wave movement wasn't enabled.
    public AudioClip lossClip;

    private bool hasBeenDestroyed = false;  // If meteor has been destroyed or not.
    private bool hasLanded = false;         // If meteor has landed or not.

    // Components
    private SpriteRenderer meteorSprite;
    private ParticleSystem particleTrail;
    public ParticleSystem explosionEffect;
    private FlanGrab_Microgame_Behaviour microgameBehaviour;


    // Use this for initialization
    void Start () {

        particleTrail = GetComponentInChildren<ParticleSystem>();
        meteorSprite = GetComponentInChildren<SpriteRenderer>();
        meteorSprite.transform.Rotate(0f, 0f, Random.Range(0f, 360f));
        microgameBehaviour = MicrogameController.instance.GetComponent<FlanGrab_Microgame_Behaviour>();
        selectEndingPoint();

    }
	
	// Update is called once per frame
	void Update () {
        
        if ( !MicrogameController.instance.getVictoryDetermined() && !hasBeenDestroyed) {

            // If game hasn't ended and if the meteor hasn't been destroyed, then it will move downwards
            rotationMovement();         // Meteor will rotate while moving downwards.
            downwardMovement();         // Meteor will move downwards.

        }

        else if (!hasLanded && !hasBeenDestroyed)
        { 

            // If game has ended, then destroy every meteor except the one that landed
            destroyMeteor();
            
        }

        if (hasBeenDestroyed && !particleTrail.IsAlive())
        {
            // Destroy meteor's gameobject when smoke trail is no longer active
            Destroy(transform.gameObject);

        }

    }

    // Calculate the position where the meteor is headed to
    void selectEndingPoint()
    {
        var midPosition = (xLeftLimit + xRightLimit) / 2;

        if (waveMovementIsOn)
        {
            originalPosition = transform.position;
            phase = Random.Range(0, Mathf.PI / 2);
            endingPosition = new Vector2(Random.Range(xLeftLimit + amplitude, xRightLimit - amplitude), yLowerLimit);
        }

        else if (diagonalMovementIsOn)
        {
            if (transform.position.x < midPosition)
            {
                endingPosition = new Vector2(Random.Range(midPosition, xRightLimit), yLowerLimit);
            }

            else
            {
                endingPosition = new Vector2(Random.Range(xLeftLimit, midPosition), yLowerLimit);
            }
        }

        else
        {
            endingPosition = new Vector2(transform.position.x, yLowerLimit);
        }

    }

    // Rotate the meteor's sprite
    void rotationMovement()
    {
        meteorSprite.transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }

    // Move the meteor downards
    void downwardMovement()
    {

        if (waveMovementIsOn)
        {
            originalPosition = Vector2.MoveTowards(originalPosition, endingPosition, movementSpeed * Time.deltaTime);
            var omegaComponent = (Time.time * frequency + phase) % (2.0f * Mathf.PI);
            var xPosition = amplitude * Mathf.Sin(omegaComponent) + originalPosition.x;
            transform.position = new Vector2(xPosition, originalPosition.y);
        }

        else
        {
            transform.position = Vector2.MoveTowards(transform.position, endingPosition, movementSpeed * Time.deltaTime);
        }

    }

    public void meteorHasLanded()
    {
        // If meteor lands on ground, set the defeat.
        MicrogameController.instance.setVictory(false, true);
        MicrogameController.instance.playSFX(lossClip);
        hasLanded = true;

        // Shake camera
        microgameBehaviour.hitShake.enabled = false;
        microgameBehaviour.loseShake.enabled = true;

		FlanGrab_Microgame_Behaviour.instance.flanimator.Play("Loss");
    }


    public void destroyMeteor()
    {
        // Destroy Meteor's sprite but not the gameObject. When the ParticleSystem associated is no longer active, meteor's gameObject will be destroyed in Update().
        hasBeenDestroyed = true;
        GetComponent<Collider2D>().enabled = false;
		ParticleSystem particles = GetComponentInChildren<ParticleSystem>();
		ParticleSystem.EmissionModule emission = particles.emission;
		emission.enabled = false;
        ParticleSystem.MainModule module = particles.main;
        module.startSizeMultiplier *= 1.8f;
		particles.Emit(5);
        Instantiate(explosionEffect, meteorSprite.transform.position, Quaternion.identity);
        Destroy(meteorSprite.gameObject);
        microgameBehaviour.increaseDestructionCounter();

        // Shake camera
        if (!(MicrogameController.instance.getVictoryDetermined() && !MicrogameController.instance.getVictory()))
        {
            microgameBehaviour.hitShake.enabled = true;
            microgameBehaviour.hitShake.resetShake();
        }

    }


}
