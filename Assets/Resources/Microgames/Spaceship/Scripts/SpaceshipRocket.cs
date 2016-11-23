using UnityEngine;
using System.Collections;

public class SpaceshipRocket : MonoBehaviour
{

	public Rigidbody2D[] spaceshipBodies;

	public ParticleSystem placeholderParticles;

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Z))
		{
			explode();
		}
	}

	void explode()
	{
		for (int i = 0; i < spaceshipBodies.Length; i++)
		{
			spaceshipBodies[i].isKinematic = false;
			spaceshipBodies[i].AddForce(MathHelper.getVectorFromAngle2D(Random.Range(0f, 180f), 500f));
		}

		placeholderParticles.Play();

		CameraShake.instance.setScreenShake(.3f);
		CameraShake.instance.shakeSpeed = 15f;
	}
}
