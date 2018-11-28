using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReimuDodgePlayer : MonoBehaviour {

    [SerializeField]
    private AudioClip deathSound;

    private bool alive = true;

	void OnTriggerEnter2D(Collider2D other)
    {
        if (alive)
        {
            Kill();

            // Tell MicrogameController that game is over
            MicrogameController.instance.setVictory(victory: false, final: true);
        }
    }

    void Kill()
    {
        // Kill Reimu
        alive = false;

        // Get a reference to the player's sprite
        SpriteRenderer spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        spriteRenderer.gameObject.SetActive(false);

        // Finally, disable the FollowCursor script to stop the object from moving
        FollowCursor followCursor = GetComponent<FollowCursor>();
        followCursor.enabled = false;

        // Play the death sound effect at a custom volume and panned to the player's x position
        MicrogameController.instance.playSFX(deathSound, volume: 0.5f, panStereo: AudioHelper.getAudioPan(transform.position.x));

        // Now get a reference to the death explosion and start it
        ParticleSystem particleSystem = GetComponentInChildren<ParticleSystem>();
        particleSystem.Play();
    }
}
