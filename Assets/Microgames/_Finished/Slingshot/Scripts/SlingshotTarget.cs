using UnityEngine;
using System.Collections;

public class SlingshotTarget : MonoBehaviour
{

	public float floatSpeed, floatAmplitude;

	public bool linearMovement;
	public float hardAmplitude, hardCycleTime;

	public float hitRotateSpeed, hitGravity;
	public Vector2 hitVelocity;

	public bool hit;
	private float startTime;

	public ParticleSystem hitParticles;
	public Vector3 initialPosition, initialParticlePosition;

	public Rigidbody2D rigidThing;


	public void reset()
	{
		startTime = Time.time;
		hitParticles.Stop();
		hitParticles.SetParticles(new ParticleSystem.Particle[0], 0);
		rigidThing.isKinematic = true;
		hit = false;


		transform.position = initialPosition;
		transform.rotation = Quaternion.identity;
		hitParticles.transform.position = initialParticlePosition;
		hitParticles.transform.parent = transform;
	}

	void Awake()
	{
		initialPosition = transform.position;
		initialParticlePosition = hitParticles.transform.position;
		rigidThing = GetComponent<Rigidbody2D>();
	}

	
	void Update ()
	{
		if (!hit)
		{
			updatePosition();
		}
		else
		{
			rigidThing.MoveRotation(rigidThing.rotation + (hitRotateSpeed * Time.deltaTime));
		}
	}

	void updatePosition()
	{
		if (!linearMovement)
		{
			transform.position = new Vector3(initialPosition.x,
				initialPosition.y + Mathf.Sin((Time.time - startTime) * floatSpeed) * floatAmplitude,
				initialPosition.z);
		}
		else
		{
			float t = ((Time.time - startTime) % hardCycleTime) / hardCycleTime;

			if (t > .5f)
			{
				t -= .5f;
				t = .5f - t;
			}

			t *= 2f;

			transform.position = new Vector3(initialPosition.x,
				initialPosition.y - Mathf.Lerp(0f, hardAmplitude, t),
				initialPosition.z);
		}

	}
}