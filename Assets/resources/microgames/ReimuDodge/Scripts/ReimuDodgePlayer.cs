using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReimuDodgePlayer : MonoBehaviour
{

	[SerializeField]
	private AudioClip deathSound;

	private bool alive = true;

	// On collision with bullet
	void OnTriggerEnter2D(Collider2D other)
	{
		// How can you kill that which is already dead
		if(alive)
		{
			Kill();

			// We've lost forever!!!
			MicrogameController.instance.setVictory(victory: false, final: true);
		}
	}


	void Kill()
	{
		// Die when killed
		alive = false;

		// Reference player sprite
		SpriteRenderer spriteRenderer = GetComponentInChildren<SpriteRenderer>();
		// leave
		spriteRenderer.gameObject.SetActive(false);

		// and stop following me
		FollowCursor followCursor = GetComponent<FollowCursor>();
		followCursor.enabled = false;

		// Pchoo
		// with custom volume stereo-panned to position
		MicrogameController.instance.playSFX(
			deathSound,
			volume: 0.5f,
			panStereo: AudioHelper.getAudioPan(transform.position.x)
		);

		// Explosion
		ParticleSystem particleSystem = GetComponentInChildren<ParticleSystem>();
		particleSystem.Play();
	}

}