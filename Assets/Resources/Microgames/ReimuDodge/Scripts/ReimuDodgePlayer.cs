using UnityEngine;

public class ReimuDodgePlayer : MonoBehaviour
{

    [SerializeField]
    private AudioClip deathSound;

    private bool alive = true;

    void OnTriggerEnter2D(Collider2D other)
    {
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

        // Get a reference to the player's sprite
        SpriteRenderer spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        // Make the sprite disappear by disabling the the GameObject it's attached to
        spriteRenderer.gameObject.SetActive(false);

        // Finally, disable the FollowCursor script to stop the object from moving
        FollowCursor followCursor = GetComponent<FollowCursor>();
        followCursor.enabled = false;

        // Play the death sound effect
        // At a custom volume
        // And panned to the player's X Posision
        MicrogameController.instance.playSFX(deathSound, volume: 0.5f,
            panStereo: AudioHelper.getAudioPan(transform.position.x));

        // Now get a reference to the death exposion and start it
        ParticleSystem particleSystem = GetComponentInChildren<ParticleSystem>();
        particleSystem.Play();
    }

}