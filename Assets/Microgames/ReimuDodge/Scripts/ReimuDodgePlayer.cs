using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReimuDodgePlayer : MonoBehaviour
{

    [SerializeField]
    private AudioClip deathsound;

    private bool alive = true;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (alive)
        {
            Kill();
        }
    }

    void Kill()
    {
        alive = false;

        SpriteRenderer spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        spriteRenderer.gameObject.SetActive(false);

        FollowCursor followcursor = GetComponent<FollowCursor>();
        followcursor.enabled = false;

        MicrogameController.instance.playSFX(deathsound, volume: 0.5f, panStereo: AudioHelper.getAudioPan(transform.position.x));

        ParticleSystem particleSystem = GetComponentInChildren<ParticleSystem>();
        particleSystem.Play();
    }
}