using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReimuDodgePlayer : MonoBehaviour
{
    [SerializeField]
    private AudioClip deathSound;
    private bool alive = true;

    void OnTriggerEnter2D(Collider2D other)
    {
        //Only kill reimu if she's still alive
        if (alive)
        {
            Kill();
        }
}
    void Kill()
    {
        //kill reimu
        alive = false;
        //get a reference to the player's sprite
        SpriteRenderer spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        //Make the sprite disappear by disabling the GameObject it's attached to
        spriteRenderer.gameObject.SetActive(false);
        //Finally, disable the FollowCursor script to stop the object from moving
        FollowCursor followCursor = GetComponent<FollowCursor>();
        followCursor.enabled = false;
        //play the custom sound effect
        //at a custom volume
        //and panned to the player's X position
        MicrogameController.instance.playSFX(deathSound, volume: 0.5f,
            panStero: AudioHelper.getAudioPan(transform.position.x));
        //now get a reference to the death exposion and start it
        ParticleSystem particleSystem = GetComponentInChildren<ParticleSystem>();
        particleSystem.Play();
    }

}