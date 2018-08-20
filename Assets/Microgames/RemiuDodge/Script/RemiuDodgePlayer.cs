using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemiuDodgePlayer : MonoBehaviour
{
    [SerializeField]
    private AudioClip deathSound;

    private bool alive = true;

    //This method will be triggered when the circle collider is hit by something.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (alive) //Kill Reimu if she is alive.
            Kill();

        //Tell that the Microgame controller is finished
        MicrogameController.instance.setVictory(victory: false, final: true);
    }

    void Kill()
    {
        alive = false;

        //Referecne to the player sprite.
        SpriteRenderer spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        //Make the sprite disapppear by disabling the GameObject it's attached to
        spriteRenderer.gameObject.SetActive(false);

        //Diable the Followcursor script to prevent the object from moving
        FollowCursor followCursor = GetComponent<FollowCursor>();
        followCursor.enabled = false;

        //Play death sound effect
        MicrogameController.instance.playSFX(deathSound, volume: 0.2f, panStereo: AudioHelper.getAudioPan(transform.position.x));

        //Reference death particle and play it.
        ParticleSystem particleSystem = GetComponentInChildren<ParticleSystem>();
        particleSystem.Play();
    }
}
