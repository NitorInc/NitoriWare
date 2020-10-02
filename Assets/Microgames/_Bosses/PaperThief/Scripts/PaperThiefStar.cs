using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperThiefStar : MonoBehaviour
{

#pragma warning disable 0649
    [SerializeField]
	private Vector2 LinearVelocity;
    [SerializeField]
    private int health = 3;
	[SerializeField]
	private float cameraActivationX, seekMoveSpeed, seekPower, rotateSpeed,
		hitSlowDownMult, hitAcc, flashSpeed, shrinkSpeed;
	[SerializeField]
	private int hitStarCount, killStarCount;
    [SerializeField]
    private bool displayDodgeCommand;
	[SerializeField]
	private Vector2 seekAngleBounds, sizeOverSpeedBounds;
	[SerializeField]
	private SpriteRenderer flash;
	[SerializeField]
	private ParticleSystem trailParticles, explosionParticles;
    [SerializeField]
    private AudioClip hitClip, deathClip, appearClip;
#pragma warning restore 0649
    public bool makeAppearSound;

    public float forceAngleDirection;
    private bool activated, outOfShootingRange;

    [SerializeField]
	private MovementType movementType;
	private enum MovementType
	{
		Linear,
		Seeking
	}

  private bool _canBeShot;

	private Vector2 velocity;
	private ParticleSystem.MainModule trailParticleModule, explosionParticleModule;
	private bool flashing, dead;

	void Start()
	{
        _canBeShot = false;
		dead = outOfShootingRange = false;
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

        checkActivation();
	}
	
	void Update()
	{
		trailParticleModule.startColor = getRandomHueColor();
        if (!activated)
            checkActivation();
		if (activated)
			updateMovement();
        updateFlash();

        if (transform.position.x < PaperThiefCamera.instance.transform.position.x - 15f
            || transform.position.y < PaperThiefCamera.instance.transform.position.y - 10f)
        {
            Destroy(gameObject);
        }
	}

    void checkActivation()
    {
        if (PaperThiefCamera.instance.transform.position.x >= cameraActivationX)
        {
            activated = true;
            trailParticles.Play();
            if (displayDodgeCommand)
                MicrogameController.instance.displayLocalizedCommand("commandb", "Watch out!");
        }
    }

	void LateUpdate()
	{
        if (PaperThiefMarisa.defeated && !dead)
            kill();
        else if (PaperThiefNitori.dead)
			stop();
	}

	void updateMovement()
	{
        if (makeAppearSound && !CameraHelper.isObjectOffscreen(transform, 1f))
        {
            MicrogameController.instance.playSFX(appearClip);
            makeAppearSound = false;
        }
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
		transform.rotation = Quaternion.Euler(0f, 0f, transform.rotation.eulerAngles.z + (rotateSpeed * Time.deltaTime));
	}

	void updateSeeking()
    {

        if (transform.position.x <= PaperThiefNitori.instance.transform.position.x)
        {
            transform.position = new Vector3(PaperThiefNitori.instance.transform.position.x, transform.position.y, transform.position.z);
            velocity = Vector2.down * velocity.magnitude;
            outOfShootingRange = true;
        }
        else if (transform.position.y <= PaperThiefNitori.instance.transform.position.y)
        {
            transform.position = new Vector3(transform.position.x, PaperThiefNitori.instance.transform.position.y, transform.position.z);
            velocity = Vector2.left * velocity.magnitude;
            outOfShootingRange = true;
        }
        else if (!outOfShootingRange)
        {
            float angle = MathHelper.getAngleDifference(velocity.getAngle(), getAngleToPlayer()),
                angleDiff = (seekPower * (angle / 90f) * Time.deltaTime) * (velocity.magnitude / seekMoveSpeed);
            velocity = MathHelper.getVector2FromAngle(velocity.getAngle() + angleDiff, velocity.magnitude);
        }

        updateKnockBack();

        if (_canBeShot)
        {
            float scale = Mathf.Lerp(sizeOverSpeedBounds.x, sizeOverSpeedBounds.y, Mathf.Pow(velocity.magnitude / seekMoveSpeed, .5f));
            transform.localScale = new Vector3(scale, scale, transform.localScale.x);
        }
	}

	void updateKnockBack()
	{
		if (velocity.magnitude < seekMoveSpeed)
			velocity = velocity.resize(Mathf.Min(seekMoveSpeed, velocity.magnitude + (hitAcc * Time.deltaTime)));
	}

    void updateFlash()
    {
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
        GetComponent<GrowToSize>().enabled = false;
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
                float soundVolume = Mathf.Lerp(1.5f, 1f, velocity.magnitude / seekMoveSpeed);
                velocity = velocity.resize(velocity.magnitude * hitSlowDownMult);
                health--;
                flashing = true;
                if (health <= 0)
				{
                    kill();
                    MicrogameController.instance.playSFX(deathClip, AudioHelper.getAudioPan(transform.position.x));
                }
				else
				{
					emitExplosionStars(hitStarCount);
                    MicrogameController.instance.playSFX(hitClip, AudioHelper.getAudioPan(transform.position.x), soundVolume);
                }
				
			}
		}
	}

    void kill()
    {
        velocity = Vector2.zero;
        trailParticles.Stop();
        trailParticleModule.simulationSpeed *= 2f;
        emitExplosionStars(killStarCount);
        flashSpeed *= 3f;
        dead = true;
        flash.GetComponent<ParticleSystem>().Play();
        GetComponent<GrowToSize>().enabled = false;
        GetComponent<Collider2D>().enabled = false;
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
