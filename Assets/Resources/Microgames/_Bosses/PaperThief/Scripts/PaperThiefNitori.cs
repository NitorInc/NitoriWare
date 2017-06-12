using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperThiefNitori : MonoBehaviour
{
	[SerializeField]
	private float walkSpeed, walkAcc, rotateSpeed, walkCooldown;
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

		_rigidBody2D.velocity += Vector2.right * direction * walkAcc * Time.deltaTime;
		_rigidBody2D.velocity = new Vector2(Mathf.Clamp(_rigidBody2D.velocity.x, -walkSpeed, walkSpeed), _rigidBody2D.velocity.y);

		updateAnimatorValues(direction);
		updateSpinRotation(direction);
	}

	void updateAnimatorValues(int direction)
	{

		bool walking = true;
		if (direction == 0)
		{
			if (walkCooldownTimer > 0f)
				walkCooldownTimer -= Time.deltaTime;
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
	}

	void updateSpinRotation(int direction)
	{
		float rotation = getSpinRotation();
		if (!(MathHelper.Approximately(rotation, 0f, .001f) || MathHelper.Approximately(rotation, -180f, .001f)))
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
