using UnityEngine;
using System.Collections;

public class SpaceshipRocket : MonoBehaviour
{

	public Rigidbody2D[] spaceshipBodies;
	public Rigidbody2D leftBody, rightBody;
	public ParticleSystem explosionParticles, liftoffParticles;
	public SpaceshipBar bar;
	public Animator shipAnimator, sceneAnimator;
	public AudioClip liftoffClip, explosionClip;

	private AudioSource _audioSource;
	private State state = State.Default;
	private enum State
	{
		Default,
		Victory,
		Failure
	}

	void Awake()
	{
		_audioSource = GetComponent<AudioSource>();
	}

	void Update()
	{
		if (!MicrogameController.instance.getVictoryDetermined() && Input.GetKeyDown(KeyCode.Z))
		{
			if (bar.isWithinThreshold())
				liftoff();
			else
				explode();

			bar.enabled = false;
		}
	}

	void liftoff()
	{
		state = State.Victory;
		MicrogameController.instance.setVictory(true, true);

		for (int i = 0; i < spaceshipBodies.Length; i++)
		{
			spaceshipBodies[i].GetComponent<Vibrate>().vibrateOn = true;
			spaceshipBodies[i].GetComponent<Collider2D>().enabled = false;
		}
		liftoffParticles.Play();
		shipAnimator.SetInteger("state", (int)state);
		sceneAnimator.SetInteger("state", (int)state);
		_audioSource.pitch = Time.timeScale;
		_audioSource.PlayOneShot(liftoffClip);

		leftBody.bodyType = rightBody.bodyType = RigidbodyType2D.Kinematic;

		CameraShake.instance.xShake = .05f;
		CameraShake.instance.yShake = .025f;
		CameraShake.instance.shakeSpeed = 2f;
		CameraShake.instance.shakeCoolRate = .0f;
	}

	void explode()
	{
		state = State.Failure;
		MicrogameController.instance.setVictory(false, true);

		for (int i = 0; i < spaceshipBodies.Length; i++)
		{
			spaceshipBodies[i].isKinematic = false;
			spaceshipBodies[i].AddForce(MathHelper.getVectorFromAngle2D(Random.Range(30f, 150f), 600f));
			spaceshipBodies[i].AddTorque(Random.Range(-1f, 1f) * 700f);
			//spaceshipBodies[i].AddForce(MathHelper.getVectorFromAngle2D(Random.Range(3f, 150f), 600f));
			//spaceshipBodies[i].AddTorque(Random.Range(-1f, 1f) * 500f);
		}

		explosionParticles.Play();
		CameraShake.instance.setScreenShake(.3f);
		CameraShake.instance.shakeSpeed = 15f;

		float force = 200f, torque = 20f;
		leftBody.AddForce((Vector3)MathHelper.getVectorFromAngle2D(120f, force));
		leftBody.AddTorque(torque);
		rightBody.AddForce((Vector3)MathHelper.getVectorFromAngle2D(30f, force));
		rightBody.AddTorque(torque * -1f);

		sceneAnimator.SetInteger("state", (int)state);

		_audioSource.pitch = Time.timeScale * .75f;
		_audioSource.PlayOneShot(explosionClip);
	}
}
