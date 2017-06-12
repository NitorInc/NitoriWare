using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperThiefNitori : MonoBehaviour
{
	[SerializeField]
	private float walkSpeed, walkAcc, rotateSpeed, walkCooldown;
	[SerializeField]
	private Animator rigAnimator;

	private Rigidbody2D _rigidBody2D;
	private bool facingRight;
	private float walkCooldownTimer;

	void Awake()
	{
		_rigidBody2D = GetComponent<Rigidbody2D>();
		facingRight = MathHelper.Approximately(getRotation(), 180f, 1f);
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
		updateRotation(direction);
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
			rigAnimator.SetFloat("WalkSpeed", Mathf.Lerp(.995f, 1f, Mathf.Abs(_rigidBody2D.velocity.x / walkSpeed)));
		else
			rigAnimator.SetFloat("WalkSpeed", 1f);
	}

	void updateRotation(int direction)
	{
		float rotation = getRotation();
		if (!(MathHelper.Approximately(rotation, 0f, .001f) || MathHelper.Approximately(rotation, 180f, .001f)))
			direction = 0;
		if (direction != 0)
			facingRight = direction == 1;

		float goalRotation = facingRight ? 180f : 0f;
		if (!MathHelper.Approximately(rotation, goalRotation, .0001f))
		{
			float diff = rotateSpeed * Time.deltaTime;
			if (Mathf.Abs(goalRotation - rotation) <= diff)
				setRotation(goalRotation);
			else
				setRotation(rotation + (diff * Mathf.Sign(goalRotation - rotation)));
		}
	}

	float getRotation()
	{
		return transform.rotation.eulerAngles.y;
	}

	void setRotation(float rotation)
	{
		Vector3 eulers = transform.rotation.eulerAngles;
		transform.rotation = Quaternion.Euler(eulers.x, rotation, eulers.z);
	}
}
