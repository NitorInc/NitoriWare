using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReimuDodgePlayer : MonoBehaviour
{
	[SerializeField]
	private AudioClip deathSound;
	
	private bool alive = true;
	
	//This will happen when the player collides with a bullet
	void OnTriggerEnter2D(Collider2D other)
	{
		//Only kill Reimu if she isn't already dead
		if (alive) Kill();
	}
	void Kill()
	{
		//Kills Reimu
		alive = false;
		
		//Get a reference to the player's sprite
		SpriteRenderer spriteRenderer = GetComponentInChildren<SpriteRenderer>();
		//Make the sprite disappear by disabling its respective GameObject
		spriteRenderer.gameObject.SetActive(false);
		
		//Disable FollowCursor as it's no longer needed
		FollowCursor followCursor = GetComponent<FollowCursor>();
		followCursor.enabled = false;
		
		//Play death sound, at custom volume, panned towards X position
		MicrogameController.instance.playSFX(deathSound, volume: 0.5f, panStereo: AudioHelper.getAudioPan(transform.position.x));
		
		//Now get a reference to the death explosion and start it
		ParticleSystem particleSystem = GetComponentInChildren<ParticleSystem>();
		particleSystem.Play();
	}
}