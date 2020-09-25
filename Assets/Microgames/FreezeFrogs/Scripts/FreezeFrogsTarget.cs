using UnityEngine;
using System.Collections;

public class FreezeFrogsTarget : MonoBehaviour
{
	public float progress, progressFromBreath;
	public SpriteRenderer white;
	public Animator animator;
	public ParticleSystem[] particleSystems;

    private Rigidbody2D _rigidBody;

	new public AudioSource audio;

	void Start()
	{
        _rigidBody = GetComponent<Rigidbody2D>();
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
	}

	void Update ()
	{
		if (progress < 1f)
			updateProgress();
	}

	void updateProgress()
	{
		progress = Mathf.Min(1f, progress);
		Color color = white.color;
		color.a = progress;
		white.color = color;

		animator.SetFloat("FrozenProgress", progress);
	}

	void freeze()
	{
		progress = 1f;
		updateProgress();
		for (int i = 0; i < particleSystems.Length;particleSystems[i].Play(), i++);
		audio.panStereo = AudioHelper.getAudioPan(transform.position.x);
		audio.Play();

        animator.enabled = false;
        transform.parent = null;
        _rigidBody.bodyType = RigidbodyType2D.Dynamic;
        _rigidBody.velocity = Vector2.up * 8f;
    }

	void OnTriggerEnter2D(Collider2D other)
	{
		if (progress < 1f && other.name == "Breath" && MicrogameController.instance.session.BeatsRemaining >= .5f)
		{
			progress += progressFromBreath;
			if (progress >= 1f)
			{
				freeze();
			}
		}
		
	}
}
