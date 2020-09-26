using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperThiefNitori : MonoBehaviour
{
	public static PaperThiefNitori instance;
	public static bool dead;

	public Transform coreTransform;

#pragma warning disable 0649
    [SerializeField]
	private float walkSpeed, jumpMoveSpeed, walkAcc, walkDec, jumpAcc, jumpDec, jumpSpeed, maxLandSnapHeight, spinCooldown,
	shotCooldown, shotSpeed, minGunCursorDistance, deathMusicDelay;
    [SerializeField]
    private int _forceDirection;
    public int forceDirection
    {
        get { return _forceDirection; }
        set { _forceDirection = value; }
    }
	[SerializeField]
	private Animator rigAnimator;
	[SerializeField]
	private Transform gunTransform, gunCursor, shotMarker, cucumberTransform, victoryTransform, boundsParent;
	[SerializeField]
	private PaperThiefSpin spinner;
	[SerializeField]
	private BoxCollider2D walkCollider;
	[SerializeField]
	private GameObject shotPrefab, gunDiscardPrefab;
    [SerializeField]
    private LayerMask walkMask;
    [SerializeField]
    private AudioSource deathSound, deathMusic, sfxSource;
    [SerializeField]
    private AudioClip stepClip, jumpClip, gunFireClip, gunEquipClip, deathClip;
    [SerializeField]
    private float stepClipVolume = 1f;
#pragma warning restore 0649

    private Rigidbody2D _rigidBody2D;
    private Transform startParent;
	private float spinCooldownTimer, shotCooldownTimer, lastStepSoundPlayedAt;

	[SerializeField]
	private State state;
	public enum State
	{
		Platforming,
		Gun,
		Dead
	}

	[SerializeField]
	private bool _hasControl;
	public bool hasControl
	{
		get { return _hasControl; }
		set { _hasControl = value; }
	}

	public enum QueueAnimation
	{
		Idle,
		GetCucumber,
		GunRecoil,
		Shock,
		Confused
	}

	void Awake()
	{
		instance = this;
		dead = false;
		_rigidBody2D = GetComponent<Rigidbody2D>();
        startParent = transform.parent;
        sfxSource.pitch = 1f;
        lastStepSoundPlayedAt = Time.time;

        if (MicrogameController.instance.isDebugMode())
        {
            hasControl = true;
        }
	}

	public State getState()
	{
		return state;
	}

	void Update()
	{
		switch (state)
		{
			case (State.Platforming):
				updatePlatforming();
				break;
			case (State.Gun):
				updateGun();
				break;
			default:
				break;
		}

        if (MicrogameController.instance.isDebugMode())
        {
            if (Input.GetKeyDown(KeyCode.V))
                MicrogameController.instance.setVictory(true, true);
            else if (Input.GetKeyDown(KeyCode.F))
                MicrogameController.instance.setVictory(false, true);
            if (Input.GetKeyDown(KeyCode.G))
            {
                MicrogameController.instance.displayCommand("send nudes");
            }

            if (Input.GetKeyDown(KeyCode.S))
                Time.timeScale *= 4f;
            if (Input.GetKeyUp(KeyCode.S))
                Time.timeScale /= 4f;
        }

    }

    public void changeState(State state)
	{
		switch (state)
		{
			case (State.Platforming):
				PaperThiefCamera.instance.setGoalPosition(new Vector3(20f, 20f, 0f));
                PaperThiefCamera.instance.setGoalSize(Camera.main.orthographicSize);
                gunCursor.gameObject.SetActive(false);
                break;
			case (State.Gun):
				rigAnimator.SetBool("Walking", false);
				rigAnimator.SetFloat("WalkSpeed", 1f);
				rigAnimator.SetInteger("Jump", 0);
                boundsParent.gameObject.SetActive(false);

                PaperThiefCamera.instance.startChase();

                sfxSource.panStereo = AudioHelper.getAudioPan(transform.position.x);
                sfxSource.PlayOneShot(gunEquipClip);
                break;
			default:
				break;
		}
        if (this.state == State.Gun && state != State.Gun)
        {
            Instantiate(gunDiscardPrefab, gunTransform.position, Quaternion.Euler(0f, 0f, updateGunTilt()));
            gunTransform.gameObject.SetActive(false);
        }

		this.state = state;
		rigAnimator.SetInteger("State", (int)state);
	}


	void updateGun()
	{
		spinner.facingRight = true;
		_rigidBody2D.velocity = new Vector2(walkSpeed * 2f, _rigidBody2D.velocity.y);

        if (hasControl)
        {
            float gunAngle = updateGunTilt();

            if (shotCooldownTimer > 0f)
                shotCooldownTimer -= Time.deltaTime;
            if (shotCooldownTimer <= 0f /*&& Input.GetMouseButton(0)*/)
            {
                float realAngle = ((Vector2)(gunCursor.position - shotMarker.position)).getAngle();
                if (Mathf.Abs(realAngle - gunAngle) <= 10f)
                    createShot(realAngle);
                else
                    createShot(gunAngle);
            }
        }
	}

	void updateSpinner(int direction)
	{
		if (spinCooldownTimer > 0f)
			spinCooldownTimer -= Time.deltaTime;
		else if (direction != 0)
		{
			bool facingRight = direction == 1;
			if (spinner.facingRight != facingRight)
			{
				spinner.facingRight = facingRight;
				spinCooldownTimer = spinCooldown;
			}
		}
	}

	void createShot(float angle)
	{
		Rigidbody2D shot = Instantiate(shotPrefab, shotMarker.position, Quaternion.Euler(0f, 0f, angle + 120f)).GetComponent<Rigidbody2D>();
		shot.transform.parent = transform;
		shot.velocity = MathHelper.getVector2FromAngle(angle, shotSpeed) + (Vector2.right * _rigidBody2D.velocity.x);
		shot.AddTorque(-200f);

		queueAnimation(QueueAnimation.GunRecoil);
		shotCooldownTimer = shotCooldown;

        sfxSource.panStereo = AudioHelper.getAudioPan(transform.position.x) / 2f;
        sfxSource.pitch = 1f;
        sfxSource.PlayOneShot(gunFireClip);
	}

	float updateGunTilt()
	{
        PaperThiefCamera.instance.Update();
		Vector2 toCursor = (Vector2)(CameraHelper.getCursorPosition() - coreTransform.position);
		float degrees = MathHelper.trueMod(toCursor.getAngle(), 360f);

        if (degrees >= 90f && degrees <= 180f)
            degrees = 89.9f;
        else if (degrees > 270f)
            degrees = 0;

		if (degrees >= 0f && degrees <= 90f && toCursor.magnitude >= minGunCursorDistance)
			rigAnimator.SetFloat("GunTilt", degrees / 90f);
		else
			degrees = rigAnimator.GetFloat("GunTilt") * 90f;
		return degrees;
	}

	void updatePlatforming()
	{
		int direction = 0;
        if (hasControl)
        {
            if (Input.GetKey(KeyCode.LeftArrow) && !wallContact(false))
                direction -= 1;
            if (Input.GetKey(KeyCode.RightArrow) && !wallContact(true))
                direction += 1;
        }
        else //if ((forceDirection == 1 && !wallContact(true)) || (forceDirection == -1 && !wallContact(false)))
            direction = _forceDirection;

        RaycastHit2D groundHit = isGrounded();
        bool grounded = groundHit;

        if (grounded)
        {
            //Snap to ground y when landing
            if (_rigidBody2D.velocity.y < 0f
                && transform.position.y < groundHit.transform.position.y
                && transform.position.y >= groundHit.transform.position.y - maxLandSnapHeight)
            {
                transform.position = new Vector3(transform.position.x, groundHit.transform.position.y, transform.position.z);
                _rigidBody2D.velocity = new Vector2(_rigidBody2D.velocity.x, 0f);

                if (hasControl && Time.time >= lastStepSoundPlayedAt + .2f)
                {
                    sfxSource.panStereo = AudioHelper.getAudioPan(transform.position.x);
                    sfxSource.PlayOneShot(stepClip, stepClipVolume);
                    lastStepSoundPlayedAt = Time.time;
                }
            }

            //Attach to moving objects
            if (groundHit.transform.name.Contains("Moving"))
                transform.parent = groundHit.transform;
            else
                transform.parent = startParent;
            
            //Jump
            if (hasControl && Input.GetKeyDown(KeyCode.Space))
            {
                _rigidBody2D.velocity = new Vector2(_rigidBody2D.velocity.x, jumpSpeed);
                grounded = false;
                transform.parent = startParent;
                sfxSource.panStereo = AudioHelper.getAudioPan(transform.position.x);
                sfxSource.PlayOneShot(jumpClip);
            }
        }
        else
            transform.parent = startParent;

        int actualDirection = _rigidBody2D.velocity.x == 0 ? 0 : (int)Mathf.Sign(_rigidBody2D.velocity.x);
        if (actualDirection == 0 && direction != 0)
            actualDirection = direction;

        updateSpinner((direction == 0 || (direction != 0 && actualDirection != direction)) ? 0 : actualDirection);

        updateWalkSpeed(direction, grounded);
        updateAnimatorValues(direction, grounded);

        //Bounds stuff
        if (direction == 0)
        {
            RaycastHit2D leftWallHit = wallContact(false);
            if (leftWallHit && leftWallHit.collider.transform.parent == boundsParent)
            {
                if (leftWallHit && wallContact(true))
                {
                    float dist = (walkCollider.bounds.extents.x * 2f) - .15f;
                    if (PhysicsHelper2D.visibleRaycast((Vector2)transform.position + new Vector2(-walkCollider.bounds.extents.x + .075f, walkCollider.bounds.extents.y),
                        Vector2.right, dist, walkMask))
                        kill(true);
                }
                else if (grounded && PaperThiefCamera.instance.getCurrentShiftSpeed() > 1f)
                {
                    actualDirection = direction = 1;
                    updateSpinner(1);
                    rigAnimator.SetBool("Walking", true);
                    rigAnimator.SetFloat("WalkSpeed", 1f);
                }
            }
        }

        if (hasControl && transform.position.x >= cucumberTransform.position.x - 1f)
        {
            PaperThiefController.instance.startScene(PaperThiefController.Scene.CucumberSteal);
            if (_rigidBody2D.velocity.y > 0f)
                _rigidBody2D.velocity = new Vector2(_rigidBody2D.velocity.x, 0f);
        }
        else if (PaperThiefMarisa.defeated && transform.position.x >= victoryTransform.position.x - .5f)
        {
            _forceDirection = 0;
            PaperThiefController.instance.startScene(PaperThiefController.Scene.Victory);
            MicrogameController.instance.setVictory(true, true);
        }
	}

	void updateWalkSpeed(int direction, bool grounded)
	{
		Vector2 velocity = _rigidBody2D.velocity;
		float goalSpeed, acc;

		goalSpeed = (float)direction * (grounded ? walkSpeed : jumpMoveSpeed);
		if (grounded || Mathf.Abs(velocity.x) > jumpMoveSpeed)
			acc = (direction == 0f) ? walkDec : walkAcc;
		else
			acc = (direction == 0f) ? jumpDec : jumpAcc;

        if (isTurningAround(direction))
            acc *= 2f;

		if (!MathHelper.Approximately(velocity.x, goalSpeed, .0001f))
		{
			float diff = acc * Time.deltaTime;
			if (Mathf.Abs(goalSpeed - velocity.x) <= diff)
				_rigidBody2D.velocity = new Vector2(goalSpeed, velocity.y);
			else
				_rigidBody2D.velocity = new Vector2(velocity.x + (diff * Mathf.Sign(goalSpeed - velocity.x)), velocity.y);
		}
	}

    public void playStepSound()
    {
        if (canStep())
        {
            sfxSource.panStereo = AudioHelper.getAudioPan(transform.position.x);
            sfxSource.PlayOneShot(stepClip, stepClipVolume);
            lastStepSoundPlayedAt = Time.time;
        }
    }

    bool canStep()
    {
        if (state == State.Gun)
            return false;
        if (!hasControl)
            return true;
        if (Time.time < (lastStepSoundPlayedAt + .2f))
            return false;
        return rigAnimator.GetBool("Walking") && isGrounded();
    }

    bool isTurningAround(int direction)
    {
        return direction != 0 && _rigidBody2D.velocity.x != 0
            && Mathf.Sign((float)direction) == -Mathf.Sign(_rigidBody2D.velocity.x);
    }

	void updateAnimatorValues(int direction, bool grounded)
	{
		rigAnimator.SetBool("Walking", direction != 0);

		if (direction == 0)
			rigAnimator.SetFloat("WalkSpeed", Mathf.Lerp(.9995f, 1f, Mathf.Abs(_rigidBody2D.velocity.x / walkSpeed)));
		else
			rigAnimator.SetFloat("WalkSpeed", 1f);

		if (grounded || _rigidBody2D.velocity.y == 0f)
			rigAnimator.SetInteger("Jump", 0);
		else
			rigAnimator.SetInteger("Jump", _rigidBody2D.velocity.y > 0f ? 1 : 2);

		AnimatorStateInfo animationState = rigAnimator.GetCurrentAnimatorStateInfo(0);
		AnimatorClipInfo[] animatorClip = rigAnimator.GetCurrentAnimatorClipInfo(0);
		if (animatorClip.Length > 0)
			rigAnimator.SetFloat("NormalizedTime", animatorClip[0].clip.length * animationState.normalizedTime);
	}

	public void queueAnimation(QueueAnimation animation)
	{
		rigAnimator.SetInteger("QueuedAnimation", (int)animation);
	}

	public RaycastHit2D isGrounded()
	{
        float dist = (walkCollider.bounds.extents.x * 2f) - .15f;
		return PhysicsHelper2D.visibleRaycast((Vector2)transform.position + new Vector2(-walkCollider.bounds.extents.x + .075f, -walkCollider.edgeRadius -.025f),
			Vector2.right, dist, walkMask);
	}

	RaycastHit2D wallContact(bool right)
	{
		float xOffset = (walkCollider.bounds.extents.x + walkCollider.edgeRadius) + .1f;
        return PhysicsHelper2D.visibleRaycast((Vector2)transform.position + new Vector2((right ? xOffset : -xOffset), .1f),
            Vector2.up, (walkCollider.bounds.extents.y + walkCollider.edgeRadius) * 1.8f, walkMask);
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (dead)
			return;

		if (other.name.EndsWith("Death"))
		{
			kill(false);
		}
        else if (other.name.EndsWith("Gun"))
        {
            changeState(State.Gun);
            Destroy(other.gameObject);
        }
    }

	public void kill(bool playAnimation)
	{
		PaperThiefCamera.instance.stopScroll();

		_rigidBody2D.bodyType = RigidbodyType2D.Kinematic;
		_rigidBody2D.velocity = Vector2.zero;

		if (playAnimation)
		{
			if (state == State.Gun)
			{
				rigAnimator.Play("GunWait");
				rigAnimator.Play("Death");
			}
			changeState(State.Dead);
		}
		else
			rigAnimator.enabled = false;

        MicrogameController.instance.setVictory(false, true);
        CameraShake.instance.setScreenShake(.15f);
		CameraShake.instance.shakeCoolRate = .5f;

        AudioSource[] sources = deathSound.transform.parent.GetComponentsInChildren<AudioSource>();
        foreach (AudioSource source in sources)
        {
            source.Stop();
        }
        deathSound.Play();
        AudioHelper.playScheduled(deathMusic, deathMusicDelay);

		dead = true;
		enabled = false;
	}

	public Rigidbody2D getRigidBody()
	{
		return _rigidBody2D;
	}

    public void setFacingRight(bool facingRight)
    {
        spinner.facingRight = facingRight;
    }

    public bool isFacingRight()
    {
        return spinner.facingRight;
    }

    public void blink()
    {
        StartCoroutine(quickBlink());
    }

    public void stopBlinking()
    {
        rigAnimator.SetInteger("BlinkQueue", -1);
    }

    IEnumerator quickBlink()
    {
        rigAnimator.SetInteger("BlinkQueue", 1);
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        rigAnimator.SetInteger("BlinkQueue", 0);
    }
}

