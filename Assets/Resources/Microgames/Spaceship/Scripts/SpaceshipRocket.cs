using UnityEngine;
using System.Collections;

public class SpaceshipRocket : MonoBehaviour
{

	public Rigidbody2D[] spaceshipBodies;
	public ParticleSystem explosionParticles;
	public SpaceshipBar bar;

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
		MicrogameController.instance.setVictory(true, true);

		//TODO victory liftoff
	}

	void explode()
	{
		MicrogameController.instance.setVictory(false, true);

		for (int i = 0; i < spaceshipBodies.Length; i++)
		{
			spaceshipBodies[i].isKinematic = false;
			spaceshipBodies[i].AddForce(MathHelper.getVectorFromAngle2D(Random.Range(30f, 150f), 600f));
			spaceshipBodies[i].AddTorque(Random.Range(-1f, 1f) * 500f);

			//TODO fix Vibrate.cs and remove this
			spaceshipBodies[i].GetComponent<Vibrate>().enabled = false;
		}

		explosionParticles.Play();

		CameraShake.instance.setScreenShake(.3f);
		CameraShake.instance.shakeSpeed = 15f;
	}
}
