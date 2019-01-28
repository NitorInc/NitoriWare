using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReimuDodgePlayer : MonoBehaviour
{

    private bool alive = true;
    [Header("Death Sound")]
    [SerializeField]
    private AudioClip death_sound;
    [Header("Effect played when reimu dies")]
    [SerializeField]
    private ParticleSystem deathEffect;
    void OnTriggerEnter2D(Collider2D other)
    {
        print("I'm hit!");
        Kill();
    }
    // Displays reimus death effects and stops player control
    void Kill()
    {
        if (!alive) return; //Can't die if you're not alive!
        alive = false;
        //Make the sprite dissapear
        SpriteRenderer spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        spriteRenderer.gameObject.SetActive(false);

        FollowCursor followCursor = GetComponent<FollowCursor>(); // FollowCursor script for player
        followCursor.enabled = false;

        //Play the death sound effect
        MicrogameController.instance.playSFX(
            death_sound,
            volume: 0.5f,
            panStereo: AudioHelper.getAudioPan(transform.position.x)
         );
         deathEffect.Play();
    }
}
