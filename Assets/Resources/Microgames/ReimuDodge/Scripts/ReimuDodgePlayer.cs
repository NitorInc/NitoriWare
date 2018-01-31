using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReimuDodgePlayer : MonoBehaviour 
{
    [SerializeField]
    private AudioClip deathSound;
    private bool alive = true;

    //Runs when player's hitbox collides with bullet hitbox
    void OnTriggerEnter2D(Collider2D other)
    {
	    Kill();
        MicrogameController.instance.setVictory(victory: false, final: true);
    }

    void Kill() 
    {
	    // kill reimu (rip reimu)
	    alive = false;
        // get sprite
        SpriteRenderer sprite = GetComponentInChildren<SpriteRenderer>();
        // make sprite disappear by disabling it's gameobject
        sprite.gameObject.SetActive(false);

        // disable cursor follow script
        FollowCursor followCursor = GetComponent<FollowCursor>();
        followCursor.enabled = false;
        
        // play death sound
        MicrogameController.instance.playSFX(
            deathSound, volume: 0.5f, 
            panStereo: AudioHelper.getAudioPan(transform.position.x)
        );

        // play death explosion animation
        ParticleSystem particleSystem = GetComponentInChildren<ParticleSystem>();
        particleSystem.Play();
    }
}
