using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperThiefNitori : MonoBehaviour
{
	[SerializeField]
	private float walkSpeed, jumpMoveSpeed, walkAcc, walkDec, jumpAcc, jumpDec, rotateSpeed, spinCooldown, jumpSpeed;
	[SerializeField]
	private Animator rigAnimator;
	[SerializeField]
	private Transform spinTransform;
	[SerializeField]
	private BoxCollider2D walkCollider;

	private Rigidbody2D _rigidBody2D;
	private bool facingRight;
	private float spinCooldownTimer;

	void Awake()
	{
		_rigidBody2D = GetComponent<Rigidbody2D>();
		facingRight = MathHelper.Approximately(getSpinRotation(), -180f, 1f);
	}
	
	void Update()
	{
		updatePlatforming();
		if (Input.GetKeyDown(KeyCode.V))
			MicrogameController.instance.setVictory(true, true);
		else if (Input.GetKeyDown(KeyCode.F))
			MicrogameController.instance.setVictory(false, true);
	}

	void updatePlatforming()
	{
		int direction = 0;
		if (Input.GetKey(KeyCode.LeftArrow) && !wallContact(false))
			direction -= 1;
		if (Input.GetKey(KeyCode.RightArrow) && !wallContact(true))
			direction += 1;

		//_rigidBody2D.velocity += Vector2.right * direction * walkAcc * Time.deltaTime;

		updateWalkSpeed(direction);
		updateAnimatorValues(direction);
		int actualDirection = (int)Mathf.Sign(_rigidBody2D.velocity.x);
		updateSpinRotation((direction == 0 || actualDirection != direction) ? 0 : actualDirection);

		if (isGrounded() && Input.GetKeyDown(KeyCode.Space))
		{
			//rigAnimator.Play("Jump Up");
			_rigidBody2D.velocity = new Vector2(_rigidBody2D.velocity.x, jumpSpeed);
		}
	}

	void updateWalkSpeed(int direction)
	{
		Vector2 velocity = _rigidBody2D.velocity;
		float goalSpeed, acc;
		goalSpeed = (float)direction * (isGrounded() ? walkSpeed : jumpMoveSpeed);
		if (isGrounded() || Mathf.Abs(velocity.y) > jumpMoveSpeed)
			acc = (direction == 0f) ? walkDec : walkAcc;
		else
			acc = (direction == 0f) ? jumpDec: jumpAcc;

		if (!MathHelper.Approximately(velocity.x, goalSpeed, .0001f))
		{
			float diff = acc * Time.deltaTime;
			if (Mathf.Abs(goalSpeed - velocity.x) <= diff)
				_rigidBody2D.velocity = new Vector2(goalSpeed, velocity.y);
			else
				_rigidBody2D.velocity = new Vector2(velocity.x + (diff * Mathf.Sign(goalSpeed - velocity.x)), velocity.y);
		}
	}

	void updateAnimatorValues(int direction)
	{
		rigAnimator.SetBool("Walking", direction != 0);

		if (direction == 0)
			rigAnimator.SetFloat("WalkSpeed", Mathf.Lerp(.9995f, 1f, Mathf.Abs(_rigidBody2D.velocity.x / walkSpeed)));
			//rigAnimator.SetFloat("WalkSpeed", Mathf.Lerp(1f, 1f, Mathf.Abs(_rigidBody2D.velocity.x / walkSpeed)));
		else
			rigAnimator.SetFloat("WalkSpeed", 1f);

		if (isGrounded())
			rigAnimator.SetInteger("Jump", 0);
		else
			rigAnimator.SetInteger("Jump", _rigidBody2D.velocity.y > 0f ? 1 : 2);

		AnimatorStateInfo animationState = rigAnimator.GetCurrentAnimatorStateInfo(0);
		AnimatorClipInfo[] animatorClip = rigAnimator.GetCurrentAnimatorClipInfo(0);
		if (animatorClip.Length > 0)
			rigAnimator.SetFloat("NormalizedTime", animatorClip[0].clip.length * animationState.normalizedTime);
	}


	bool isGrounded()
	{
		float dist = (walkCollider.bounds.extents.x * 2f) - .2f;
		return PhysicsHelper2D.visibleRaycast((Vector2)transform.position + new Vector2(-walkCollider.bounds.extents.x + .1f, -.1f),
			Vector2.right, dist);
		
	}

	bool wallContact(bool right)
	{
		float xOffset = walkCollider.bounds.extents.x + .1f;
		return right ?
			PhysicsHelper2D.visibleRaycast((Vector2)transform.position + new Vector2(xOffset, .1f), Vector2.up, walkCollider.bounds.extents.y * 1.8f) :
			PhysicsHelper2D.visibleRaycast((Vector2)transform.position + new Vector2(-xOffset, .1f), Vector2.up, walkCollider.bounds.extents.y * 1.8f);
	}

	void updateSpinRotation(int direction)
	{
		float rotation = getSpinRotation();
		if (spinCooldownTimer > 0f)
			spinCooldownTimer -= Time.deltaTime;
		if (/*!isGrounded() || */ !(MathHelper.Approximately(rotation, 0f, .001f) || MathHelper.Approximately(rotation, -180f, .001f)) || spinCooldownTimer > 0f)
			direction = 0;
		if (direction != 0)
			facingRight = direction == 1;

		//Spin between 0 and -180 degrees
		float goalRotation = facingRight ? -180f : 0f;
		if (!MathHelper.Approximately(rotation, goalRotation, .0001f))
		{
			float diff = rotateSpeed * Time.deltaTime;
			if (Mathf.Abs(goalRotation - rotation) <= diff)
				setSpinRotation(goalRotation);
			else
				setSpinRotation(rotation + (diff * Mathf.Sign(goalRotation - rotation)));

			spinCooldownTimer = spinCooldown;
		}
	}

	float getSpinRotation()
	{
		Vector3 eulers = spinTransform.rotation.eulerAngles;
		return eulers.y <= 0f ? eulers.y : eulers.y - 360f;
	}

	void setSpinRotation(float rotation)
	{
		Vector3 eulers = spinTransform.rotation.eulerAngles;
		spinTransform.rotation = Quaternion.Euler(eulers.x, rotation, eulers.z);
	}
}
