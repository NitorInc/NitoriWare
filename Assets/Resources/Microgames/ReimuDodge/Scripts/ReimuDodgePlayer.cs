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
		// Test that this works
		print("Player was hit!");
		if (alive) {
			Kill ();
		}
	}

	void Kill()
	{
		//Reimu is ded
		alive = false;
		//Reimu's sprite disappears
		SpriteRenderer spriteRenderer = GetComponentInChildren<SpriteRenderer> ();
		spriteRenderer.gameObject.SetActive (false);
		//FollowCursor scripts stops
		FollowCursor followCursor = GetComponent<FollowCursor> ();
		followCursor.enabled = false;
		//Death sound plays
		MicrogameController.instance.playSFX (deathSound, AudioHelper.getAudioPan (transform.position.x),1,0.5f);
		//Particles starts showing
		ParticleSystem particleSystem = GetComponentInChildren<ParticleSystem> ();
		particleSystem.Play ();

		MicrogameController.instance.setVictory(victory: false, final: true);
	}

}
