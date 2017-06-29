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
	shotCooldown, shotSpeed, minGunCursorDistance;
    [SerializeField]
    private int _forceDirection;
	[SerializeField]
	private Animator rigAnimator;
	[SerializeField]
	private Transform gunTransform, gunCursor, shotMarker, cucumberTransform, victoryTransform;
	[SerializeField]
	private PaperThiefSpin spinner;
	[SerializeField]
	private BoxCollider2D walkCollider;
	[SerializeField]
	private GameObject shotPrefab, gunDiscardPrefab;
    [SerializeField]
    private LayerMask walkMask;
#pragma warning restore 0649

    public int forceDirection
    {
        get { return _forceDirection; }
        set { _forceDirection = value; }
    }

    private Rigidbody2D _rigidBody2D;
    private Transform startParent;
	private float spinCooldownTimer, shotCooldownTimer;

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
		Idle,			//0
		GetCucumber,	//1
		GunRecoil,		//2
		Shock,			//3
		Confused		//4
	}

	void Awake()
	{
		instance = this;
		dead = false;
		_rigidBody2D = GetComponent<Rigidbody2D>();
        startParent = transform.parent;

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
                //changeState(state == State.Gun ? State.Platforming : State.Gun);
            }

            if (Input.GetKeyDown(KeyCode.S))
                Time.timeScale *= 4f;
            if (Input.GetKeyUp(KeyCode.S))
                Time.timeScale /= 4f;
            //else if (Input.GetKeyDown(KeyCode.T))
            //{
            //	//rigAnimator.Play("Hop");
            //	queueAnimation(QueueAnimation.Confused);
            //	//queueAnimation(QueueAnimation.Shock);
            //	//queueAnimation(QueueAnimation.GetCucumber);
            //}
            //else if (Input.GetKeyDown(KeyCode.I))
            //{
            //	queueAnimation(QueueAnimation.Idle);
            //}
        }

    }

    public void changeState(State state)
	{
		switch (state)
		{
			case (State.Platforming):
				//PaperThiefCamera.instance.transform.parent = null;
				PaperThiefCamera.instance.setGoalPosition(new Vector3(20f, 20f, 0f));
                PaperThiefCamera.instance.setGoalSize(Camera.main.orthographicSize);
                gunCursor.gameObject.SetActive(false);
                break;
			case (State.Gun):
				rigAnimator.SetBool("Walking", false);
				rigAnimator.SetFloat("WalkSpeed", 1f);
				rigAnimator.SetInteger("Jump", 0);

                PaperThiefCamera.instance.startChase();
				//PaperThiefCamera.instance.transform.parent = transform;
				//PaperThiefCamera.instance.setFollow(null);
				//PaperThiefCamera.instance.setGoalPosition(new Vector3(25f, 20f, 0f));
				//PaperThiefCamera.instance.setGoalSize(6.5f);
				//gunCursor.gameObject.SetActive(true);
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
            if (shotCooldownTimer <= 0f && Input.GetMouseButton(0))
                createShot(gunAngle);
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
	}

	float updateGunTilt()
	{
        PaperThiefCamera.instance.LateUpdate();
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
            direction = forceDirection;

        RaycastHit2D groundHit = isGrounded();
        bool grounded = groundHit; // && _rigidBody2D.velocity.y <= 0f;

        if (grounded)
        {
            //Snap to ground y when landing
            if (_rigidBody2D.velocity.y < 0f
                && transform.position.y < groundHit.transform.position.y
                && transform.position.y >= groundHit.transform.position.y - maxLandSnapHeight)
            {
                //float snapY = groundHit.transform.position.y + (groundHit.collider.bounds.extents.y);
                transform.position = new Vector3(transform.position.x, groundHit.transform.position.y, transform.position.z);
                _rigidBody2D.velocity = new Vector2(_rigidBody2D.velocity.x, 0f);
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
        
        if (hasControl && transform.position.x >= cucumberTransform.position.x - 1f)
        {
            PaperThiefController.instance.startScene(PaperThiefController.Scene.CucumberSteal);
            if (_rigidBody2D.velocity.y > 0f)
                _rigidBody2D.velocity = new Vector2(_rigidBody2D.velocity.x, 0f);
        }
        else if (PaperThiefMarisa.defeated && transform.position.x >= victoryTransform.position.x - .5f)
        {
            forceDirection = 0;
            PaperThiefController.instance.startScene(PaperThiefController.Scene.Victory);
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
		//rigAnimator.SetFloat("WalkSpeed", Mathf.Lerp(1f, 1f, Mathf.Abs(_rigidBody2D.velocity.x / walkSpeed)));
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

		CameraShake.instance.setScreenShake(.15f);
		CameraShake.instance.shakeCoolRate = .5f;
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

