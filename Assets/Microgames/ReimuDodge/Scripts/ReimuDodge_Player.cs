using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReimuDodge_Player : MonoBehaviour 
{

	[SerializeField]
    private AudioClip deathSound;

    private bool alive = true;
	private SpriteRenderer _SpriteRenderer;
	private FollowCursor _FollowCursor;
	private ParticleSystem[] deathParticles;

	void Start()
	{
		_SpriteRenderer = GetComponentInChildren<SpriteRenderer>();
		_FollowCursor = GetComponent<FollowCursor>();
		deathParticles = GetComponentsInChildren<ParticleSystem>();
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (alive && other.gameObject.layer == 15)
        {
            die();
        }
    }

    void die()
    {
        alive = false;
        _SpriteRenderer.gameObject.SetActive(false);
        _FollowCursor.enabled = false;

        MicrogameController.instance.playSFX(deathSound, volume: 0.5f, panStereo: AudioHelper.getAudioPan(transform.position.x));
		for (int i = 0; i < deathParticles.Length; i++)
		{
			deathParticles[i].Play();
		}
        
		MicrogameController.instance.setVictory(victory: false, final: true);
    }
}
