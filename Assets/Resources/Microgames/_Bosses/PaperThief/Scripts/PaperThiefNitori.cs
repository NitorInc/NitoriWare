using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperThiefNitori : MonoBehaviour
{
	[SerializeField]
	private float walkSpeed, jumpMoveSpeed, walkAcc, walkDec, jumpAcc, jumpDec, rotateSpeed, walkCooldown, jumpSpeed;
	[SerializeField]
	private Animator rigAnimator;
	[SerializeField]
	private Transform spinTransform;

	private Rigidbody2D _rigidBody2D;
	private bool facingRight;
	private float walkCooldownTimer;

	void Awake()
	{
		_rigidBody2D = GetComponent<Rigidbody2D>();
		facingRight = MathHelper.Approximately(getSpinRotation(), -180f, 1f);
	}
	
	void Update()
	{
		updatePlatforming();
	}

	void updatePlatforming()
	{
		int direction = 0;
		if (Input.GetKey(KeyCode.LeftArrow))
			direction -= 1;
		if (Input.GetKey(KeyCode.RightArrow))
			direction += 1;

		//_rigidBody2D.velocity += Vector2.right * direction * walkAcc * Time.deltaTime;

		updateWalkSpeed(direction);
		updateAnimatorValues(direction);
		updateSpinRotation(direction);

		if (isGrounded() && Input.GetKeyDown(KeyCode.Space))
		{
			rigAnimator.Play("Idle");
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

		bool walking = true;
		if (direction == 0)
		{
			if (walkCooldownTimer > 0f)
				walkCooldownTimer -= Time.deltaTime;
			if (!isGrounded())
				walkCooldown = 0f;
			walking = walkCooldownTimer > 0f;
			//walking = false;
		}
		else
			walkCooldownTimer = walkCooldown;
		rigAnimator.SetBool("Walking", walking);

		if (direction == 0)
			rigAnimator.SetFloat("WalkSpeed", Mathf.Lerp(.9965f, 1f, Mathf.Abs(_rigidBody2D.velocity.x / walkSpeed)));
		else
			rigAnimator.SetFloat("WalkSpeed", 1f);

		if (isGrounded())
			rigAnimator.SetInteger("Jump", 0);
		else
			rigAnimator.SetInteger("Jump", _rigidBody2D.velocity.y > 0f ? 1 : 2);

	}

	bool isGrounded()
	{
		//Debug.DrawLine(transform.position + (Vector3.down * 1f), transform.position + (Vector3.down * 1f) + (Vector3.down * .1f));
		return Physics2D.Raycast((Vector2)transform.position + (Vector2.down * .01f), Vector2.down, .01f);
		
	}

	void updateSpinRotation(int direction)
	{
		float rotation = getSpinRotation();
		if (!isGrounded() || !(MathHelper.Approximately(rotation, 0f, .001f) || MathHelper.Approximately(rotation, -180f, .001f)))
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
