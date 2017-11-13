using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReimuDodgePlayerScript : MonoBehaviour {

    [SerializeField]
    private AudioClip deathSound;

    //declare private variables
    private int currentFrames = 0;
    private bool alive = true;

    void Start()
    {
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (alive)
        {
            Kill();
        }
    }

    void Kill()
    {
        // Kill Reimu
        alive = false;

        MicrogameController.instance.setVictory(victory: false, final: true);

        // Get a reference to the player's sprite
        //GameObject rig = GameObject.Find("rig");
        SpriteRenderer spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        // Make the sprite disappear by disabling the the GameObject it's attached to
        spriteRenderer.gameObject.SetActive(false);
        //rig.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);

        // Finally, disable the FollowCursor script to stop the object from moving
        FollowCursor followCursor = GetComponent<FollowCursor>();
        followCursor.enabled = false;

        // Play the death sound effect
        // At a custom volume
        // And panned to the player's X Posision
        MicrogameController.instance.playSFX(deathSound, volume: 0.5f,
            panStero: AudioHelper.getAudioPan(transform.position.x));

        // Now get a reference to the death exposion and start it
        ParticleSystem particleSystem = GetComponentInChildren<ParticleSystem>();
        particleSystem.Play();
    }
}
