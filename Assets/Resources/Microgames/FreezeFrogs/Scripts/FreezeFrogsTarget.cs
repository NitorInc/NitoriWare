using UnityEngine;
using System.Collections;

public class FreezeFrogsTarget : MonoBehaviour
{
	public float progress, progressFromBreath;
	public SpriteRenderer white;
	public Animator animator;
	public ParticleSystem[] particleSystems;

	new public AudioSource audio;

	void Start()
	{
		reset();
	}
	
	public void reset()
	{
		progress = 0;
		animator.SetFloat("FrozenProgress", 0f);
		updateProgress();

		for (int i = 0; i < particleSystems.Length; i++)
		{
			particleSystems[i].Stop();
			particleSystems[i].SetParticles(new ParticleSystem.Particle[0], 0);

		}

		//transform.parent.GetComponent<Animator>().animation["bluh"].normalizedTime = 0f;


	}

	void Update ()
	{
		if (progress < 1f)
			updateProgress();
	}

	void updateProgress()
	{
		if (progress > 1f)
			progress = 1f;

		Color color = white.color;
		color.a = progress;
		white.color = color;

		animator.SetFloat("FrozenProgress", progress);
	}

	void freeze()
	{
		progress = 1f;
		updateProgress();
		for (int i = 0; i < particleSystems.Length; i++)
		{
			particleSystems[i].Play();
		}

		audio.pitch = 1.35f * Time.timeScale;
		audio.Play();
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (progress < 1f && other.name == "Breath" && MicrogameTimer.instance.beatsLeft >= .5f)
		{
			progress += progressFromBreath;
			if (progress >= 1f)
			{
				freeze();
			}
		}
	}
}
