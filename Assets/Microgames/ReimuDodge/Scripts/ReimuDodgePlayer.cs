using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReimuDodgePlayer : MonoBehaviour
{
    [SerializeField]
    private AudioClip deathSound;

    private bool alive = true;

    // This will happen when the player's hitbox collides with a bullet
    void OnTriggerEnter2D(Collider2D other)
    {
        // Only kill Reimu if she's still alive
        if (alive)
        {
            Kill();
        }
    }
    
    void Kill()
    {
        // Murder
        alive = false;
        // Get a reference to player's sprite
        SpriteRenderer spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        // Make the sprite dissapear by disabling the GameObject it's at
        spriteRenderer.gameObject.SetActive(false);

        // Finally disable the FollowCursor script to stop the object from moving
        FollowCursor followCursor = GetComponent<FollowCursor>();
        followCursor.enabled = false;

        // Play the death sound effect
        // At a custom volume
        // And panned to the player's X Position
        MicrogameController.instance.playSFX(deathSound, volume: 0.5f,
            panStereo: AudioHelper.getAudioPan(transform.position.x));
        // Now get a reference to the death explosion and start it
        ParticleSystem particleSystem = GetComponentInChildren<ParticleSystem>();
        particleSystem.Play();
    }
}
