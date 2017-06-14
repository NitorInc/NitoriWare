using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperThiefStar : MonoBehaviour
{
	[SerializeField]
	private Vector2 LinearVelocity;
	[SerializeField]
	private float cameraActivationX, seekMoveSpeed, seekPower, rotateSpeed, forceAngleDirection,
		hitSlowDownMult, hitAcc, killSpeed, flashSpeed, shrinkSpeed;
	[SerializeField]
	private int hitStarCount, killStarCount;
	[SerializeField]
	private Vector2 seekAngleBounds;
	[SerializeField]
	private SpriteRenderer flash;
	[SerializeField]
	private ParticleSystem trailParticles, explosionParticles;

	[SerializeField]
	private MovementType movementType;
	private enum MovementType
	{
		Linear,
		Seeking
	}

	private Vector2 velocity;
	private ParticleSystem.MainModule trailParticleModule, explosionParticleModule;
	private bool flashing, dead;

	void Start()
	{
		dead = false;
		transform.rotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));

		trailParticleModule = trailParticles.main;
		explosionParticleModule = explosionParticles.main;

		GetComponent<SpriteRenderer>().color = new HSBColor(Random.Range(2f / 18f, 3f / 18f), 1f, 1f).ToColor();
		trailParticleModule.startColor = getRandomHueColor();

		if (movementType == MovementType.Seeking)
		{
			trailParticleModule.simulationSpace = ParticleSystemSimulationSpace.Custom;
			trailParticleModule.customSimulationSpace = PaperThiefCamera.instance.transform;

			ParticleSystem.MainModule explosionModule = flash.GetComponent<ParticleSystem>().main;
			explosionModule.simulationSpace = ParticleSystemSimulationSpace.Custom;
			explosionModule.customSimulationSpace = PaperThiefCamera.instance.transform;
		}
	}
	
	void Update()
	{
		trailParticleModule.startColor = getRandomHueColor();
		if (PaperThiefCamera.instance.transform.position.x >= cameraActivationX)
			updateMovement();
	}

	void LateUpdate()
	{
		if (PaperThiefNitori.dead)
			stop();
	}

	void updateMovement()
	{
		if (dead)
		{
			transform.localScale -= Vector3.one * shrinkSpeed * Time.deltaTime;
			if (transform.localScale.x <= 0f)
			{
				transform.localScale = Vector3.zero;
				Invoke("destroy", 1f);
				enabled = false;
			}
			return;
		}
		switch (movementType)
		{
			case (MovementType.Linear):
				velocity = LinearVelocity;
				break;
			case (MovementType.Seeking):
				if (velocity == Vector2.zero)
					velocity = MathHelper.getVector2FromAngle(getAngleToPlayer() + (MathHelper.randomRangeFromVector(seekAngleBounds) *
						((forceAngleDirection != 0f ? forceAngleDirection : (MathHelper.randomBool() ? 1f : -1f)))),
						seekMoveSpeed);
				updateSeeking();
				break;
			default:
				break;
		}

		transform.localPosition += (Vector3)velocity * Time.deltaTime;
		transform.rotation = Quaternion.Euler(0f, 0f, transform.rotation.eulerAngles.z + rotateSpeed);
	}

	void updateSeeking()
	{
		float angleDiff = MathHelper.getAngleDifference(velocity.getAngle(), getAngleToPlayer());
		angleDiff = (seekPower * (angleDiff / 90f) * Time.deltaTime) * (velocity.magnitude / seekMoveSpeed);
		velocity = MathHelper.getVector2FromAngle(velocity.getAngle() + angleDiff, velocity.magnitude);

		updateKnockBack();
	}

	void updateKnockBack()
	{
		if (velocity.magnitude < seekMoveSpeed)
			velocity = velocity.resize(Mathf.Min(seekMoveSpeed, velocity.magnitude + (hitAcc * Time.deltaTime)));

		if (flashing || dead)
		{
			setFlashAlpha(Mathf.Min(getFlashAlpha() + (flashSpeed * Time.deltaTime), 1f));
			if (getFlashAlpha() == 1f)
				flashing = false;
		}
		else if (getFlashAlpha() > 0f)
			setFlashAlpha(Mathf.Max(getFlashAlpha() - (flashSpeed * Time.deltaTime), 0f));
	}

	float getAngleToPlayer()
	{
		return ((Vector2)(PaperThiefNitori.instance.coreTransform.position - transform.position)).getAngle();
	}

	public void stop()
	{
		trailParticleModule.simulationSpeed = explosionParticleModule.simulationSpeed = 0f;
		enabled = false;
	}
	
	void OnTriggerEnter2D(Collider2D other)
	{
		if (!dead && !PaperThiefNitori.dead && !CameraHelper.isObjectOffscreen(transform))
		{
			if (other.name.Contains("Nitori"))
				PaperThiefNitori.instance.kill(true);
			else if (other.name.Contains("Shot"))
			{
				other.GetComponent<PaperThiefShot>().kill();
				velocity = velocity.resize(velocity.magnitude * hitSlowDownMult);
				if (velocity.magnitude <= killSpeed)
				{
					velocity = Vector2.zero;
					trailParticles.Stop();
					trailParticleModule.simulationSpeed *= 2f;
					emitExplosionStars(killStarCount);
					dead = true;
					flash.GetComponent<ParticleSystem>().Play();
					GetComponent<Collider2D>().enabled = false;
				}
				else
				{
					flashing = true;
					emitExplosionStars(hitStarCount);
				}
				
			}
		}
	}

	void destroy()
	{
		if (!PaperThiefNitori.dead)
			Destroy(gameObject);
	}

	void emitExplosionStars(int count)
	{
		for (int i = 0; i < count; i++)
		{
			explosionParticleModule.startColor = getRandomHueColor();
			explosionParticles.Emit(1);
		}
	}

	void setFlashAlpha(float alpha)
	{
		Color color = flash.color;
		color.a = alpha;
		flash.color = color;
	}

	float getFlashAlpha()
	{
		return flash.color.a;
	}

	Color getRandomHueColor()
	{
		Color color = new HSBColor(Random.Range(0f, 1f), 1f, 1f).ToColor();
		color.a = Random.Range(.25f, .75f);
		return color;
	}
}
