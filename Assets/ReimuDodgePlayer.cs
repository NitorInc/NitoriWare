using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReimuDodgePlayer : MonoBehaviour {

    [SerializeField]
    private AudioClip deathSound;

    private bool alive = true;


   // Use this for initialization
   void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
       if (alive)
        {
            Kill();

            MicrogameController.instance.setVictory(victory: false, final: true);
        }
    }

    private void Kill()
    {
        // Kill Reimu
        alive = false;

        SpriteRenderer spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        spriteRenderer.gameObject.SetActive(false);

        FollowCursor followCursor = GetComponent<FollowCursor>();
        followCursor.enabled = false;

        MicrogameController.instance.playSFX(deathSound, volume: 0.5f, panStereo: AudioHelper.getAudioPan(transform.position.x));

        ParticleSystem particleSystem = GetComponentInChildren<ParticleSystem>();
        particleSystem.Play();
    }
}
