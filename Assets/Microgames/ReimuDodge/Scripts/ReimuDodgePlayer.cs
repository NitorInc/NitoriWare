﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReimuDodgePlayer : MonoBehaviour {

    [SerializeField]
    private AudioClip deathSound;

    private bool alive = true;

    // This will happen when the player's hitbox collides with a bullet
    void OnTriggerEnter2D(Collider2D other)
    {
        if (alive)
            Kill();
        
    }

    void Kill()
    {
        alive = false;

        SpriteRenderer spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        spriteRenderer.gameObject.SetActive(false);

        FollowCursor followCursor = GetComponent<FollowCursor>();
        followCursor.enabled = false;

        MicrogameController.instance.playSFX(deathSound, volume: 0.5f,
            panStereo: AudioHelper.getAudioPan(transform.position.x));

        ParticleSystem particleSystem = GetComponentInChildren<ParticleSystem>();
        particleSystem.Play();

        MicrogameController.instance.setVictory(victory: false, final: true);
    }
}
