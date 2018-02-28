using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReimuDodgePlayer : MonoBehaviour {

	[SerializeField]
	private AudioClip deathSound;

	private bool alive = true;

	// Use this for initialization
	void OnTriggerEnter2D(Collider2D other) {
		
		//Test it works
		//print("Collision occured!");

		//Kill Miko. ;__;
		if(alive)
		{
			Kill();

			//We should tell the MicrogameController that the scene and game are thus over.
			//And we lost forever.
			//Go back to Easy
			MicrogameController.instance.setVictory(victory: false, final: true);
		}
	}

	void Kill()
	{
		//Kill Reimu
		alive = false;

		//get a reference to the sprite
		SpriteRenderer spriteRenderer = GetComponentInChildren<SpriteRenderer>();
		//Make the sprite disappear by disabling the GameObject it's attach to.
		spriteRenderer.gameObject.SetActive(false);

		//Finally, disable the FollowCursor Script to stop the object from moving.
		FollowCursor followCursor = GetComponent<FollowCursor>();
		followCursor.enabled = false;

		//Play the death sound effect at a custom volume
		//and panned to the player's X position
		MicrogameController.instance.playSFX(deathSound, volume: 0.5f, panStereo: AudioHelper.getAudioPan(transform.position.x));

		//Now get a reference to the death explosion and start it.
		ParticleSystem particleSystem = GetComponentInChildren<ParticleSystem>();
		particleSystem.Play();
	}
	

}
