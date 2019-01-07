using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ReimuDodgePlayer : MonoBehaviour
{

    [SerializeField]
    private AudioClip deathSound;

    private bool alive = true;

    public GameObject SpriteRender; //Part of the temporary bootleg particle solution. Get the players sprite renderer.
    public GameObject ParticleRender;//Gameobject containing the Particle System

    // This will happen when the player's hitbox collides with a bullet
    void OnTriggerEnter2D(Collider2D other)
    {
        // Only kill Reimu if she's still alive
        if (alive)
        {
            Kill();

            MicrogameController.instance.setVictory(victory: false, final: true);
        }
    }

    void Kill()
    {
        // Kill Reimu
        alive = false;

        //// Get a reference to the player's sprite
        //SpriteRenderer spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        //// Make the sprite disappear by disabling the the GameObject it's attached to
        //spriteRenderer.gameObject.SetActive(false);


        //TEMP SYSTEM
        SpriteRender.SetActive(false);//Disable the sprite renderer, while leaving the GameObject active.

        // Finally, disable the FollowCursor script to stop the object from moving
        FollowCursor followCursor = GetComponent<FollowCursor>();
        followCursor.enabled = false;

        // Play the death sound effect
        // At a custom volume
        // And panned to the player's X Posision
        MicrogameController.instance.playSFX(deathSound, volume: 0.5f,
            panStereo: AudioHelper.getAudioPan(transform.position.x));

        // Now get a reference to the death exposion and start it
        //ParticleSystem particleSystem = GetComponentInChildren<ParticleSystem>();
        //particleSystem.Play();

        ParticleRender.SetActive(true);//Enable the Particle system. It has "Play on Awake" enabled so it will automatically trigger the effect,

        //bootleg system to get an explosion until I can pinpoint the issue.
        //particleSystem.gameObject.SetActive(true);
    }

}