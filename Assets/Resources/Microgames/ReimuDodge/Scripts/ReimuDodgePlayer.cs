using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReimuDodgePlayer : MonoBehaviour {

    [SerializeField]
    private AudioClip deathSound;

    private bool alive = true;

    // This will happen when the player's hitbox collides with the bullet
    void OnTriggerEnter2D(Collider2D other)
    {
        // Only kill Reimu if she's still alive
        // Killing dead people is forbidden under the spellcard rules
        if (alive)
        {
            Kill();

            // Now tell the MicrogameController in the scene that the game is over
            // and we've lost forever
            MicrogameController.instance.setVictory(victory: false, final: true);
        }
    }

    void Kill()
    {
        // Kill Reimu
        alive = false;

        // Get a reference to the player's sprite
        SpriteRenderer spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        // Make the sprite disappear by disabling the GameObject it's attached to
        spriteRenderer.gameObject.SetActive(false);

        // Finally, disable the FollowCursor script to stip the object from moving
        FollowCursor followCursor = GetComponent<FollowCursor>();
        followCursor.enabled = false;

        // Play the death sound effect
        // at a custom volume
        // and panned to the player's X position
        MicrogameController.instance.playSFX(deathSound, volume: 0.5f,
            panStero: AudioHelper.getAudioPan(transform.position.x));

        // Now get a reference to the death explosion and start it
        ParticleSystem particleSystem = GetComponentInChildren<ParticleSystem>();
        particleSystem.Play();
    }
}
