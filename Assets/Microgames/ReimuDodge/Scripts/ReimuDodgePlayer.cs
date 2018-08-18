using UnityEngine;
public class ReimuDodgePlayer : MonoBehaviour
{
    [SerializeField]
    private AudioClip deathSound;
    private bool alive = true;
    //this will happen when the players hitbox collides with a bullet
    void OnTriggerEnter2D(Collider2D other)
    {
        // Only kill if still alive
        if (alive)
        {
            Kill();
			//you lose
			MicrogameController.instance.setVictory(victory: false, final: true);
        }
    }
    void Kill()
    {
        alive = false;
        //get a ref to the players sprite
        SpriteRenderer spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        //make the sprite disappear by disabling the the gameObject its attached to
        spriteRenderer.gameObject.SetActive(false);

        //lastly disable the FollowCursor script to stop the object from moving
        FollowCursor followCursor = GetComponent<FollowCursor>();
        followCursor.enabled = false;

        //play the death sound effect at a custom volume 
		//and panned to the players x pos
        MicrogameController.instance.playSFX(deathSound, volume: 0.5f,
			panStereo: AudioHelper.getAudioPan(transform.position.x));
        // Now get a ref to the death effect and start it
        ParticleSystem particleSystem = GetComponentInChildren<ParticleSystem>();
        particleSystem.Play();
    }
}