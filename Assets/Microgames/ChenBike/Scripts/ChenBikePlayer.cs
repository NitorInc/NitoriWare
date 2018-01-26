﻿using UnityEngine;
using System.Collections;

public class ChenBikePlayer : MonoBehaviour
{
	public AudioClip BikeHorn;
	public Animator chenAnimator;
	public AudioSource honkSource;
    public static bool honking;
    public int ammo = 3;
    public ParticleSystem honkParticle;
    public GameObject count1, count2, count3;
    public ChenBikePlayerFail ifdead;
    public ChenBikePlayerFail ifdead2;

    void Awake()
	{
		/* FEEDBACK: Do not use getComponent() every Update. It is slow.
		 * getComponent() should generally be stored as a value in Awake()
		 * 
		 * Or, as I like to do it, just make it a public variable and store it in the inspector, no need for GetComponent()
		 * plus then you can change the reference easily and without editing code
		 * */
		chenAnimator = GetComponent<Animator>();
		honkSource = GetComponent<AudioSource>();
        honkParticle = GetComponent<ParticleSystem>();
	}

	// Update is called once per frame
	void Update()
	{
        //FEEDBACK: No need to nest two if statements here since you could just use an And (&&)
        //And Input.GetKeyDown("z") won't even be checked if the first part is false, meaning it's the same computation time
        if (chenAnimator.GetCurrentAnimatorStateInfo(1).IsName("HonkLayer.Idle") && Input.GetKeyDown(KeyCode.Space) && (ammo > 0) && (ifdead2.dead == false) &&(ifdead.dead == false))
        {
            chenAnimator.Play("ChenHonk");
            honkParticle.Play(true);
            //FEEDBACK: Use Time.timescale for pitch in audioSources, so the pitch is increased when the game is faster
            honkSource.pitch = Time.timeScale;
            honkSource.PlayOneShot(BikeHorn, 1F);
            if (ammo == 3)
                Destroy(count3);
            if (ammo == 2)
                Destroy(count2);
            if (ammo == 1)
                Destroy(count1);
            ammo -= 1;

        }

        // player is honking for half the duration of the sound
        // it's too easy if for the whole duration of the sound
        // and unfair if player can honk only when characters are in range
        // thanks to this the player can honk prematurely a bit and win
        // update: according to feedback, changed to third of the duration
        if (honkSource.isPlaying && honkSource.time < BikeHorn.length/3f)
            honking = true;
        else
            honking = false;
	}
}
