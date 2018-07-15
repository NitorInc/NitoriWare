using UnityEngine;

public class ReimuDodgePlayer : MonoBehaviour
{

    [SerializeField]
    private AudioClip deathsound;

    private bool alive = true;

    // This will happen when the player's hitbox collides with a bullet
    void OnTriggerEnter2D(Collider2D other)
    {
        // Only Kill Reimu if she's still alive
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

        // Finally, disable the FollowCursor script to stop the object from moving
        FollowCursor followCursor = GetComponent<FollowCursor>();
        followCursor.enabled = false;

        // Play the death sound effect
        // At a custom volume
        // And panned to the player's X position
        MicrogameController.instance.playSFX(deathsound, volume: 0.5f,
            panStereo: AudioHelper.getAudioPan(transform.position.x));

        // Now get a reference to the death explosion and start it
        ParticleSystem particleSystem = GetComponentInChildren<ParticleSystem>();
        particleSystem.Play();
    }
}
