using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReimuDodge_Bullet : MonoBehaviour 
{
	/*
			Changes from the initial tutorial:
				- Using a rigidbody2D instead of raw movement, much more performant and precise
				- Setting rigidbody velocity only once, thus not calling the Update method, lighter!
				- Trajectory (targetPos) made a local variable
				- Rigidbodies for the bullet and player must be kinematic
				- camelCase :p
	*/

	[Header("-- Object towards which the bullet will move")]
	[SerializeField] private Transform target;
	[Header("-- Bullet movement speed")]
	[SerializeField] private float moveSpeed = 1f;
	[Header("-- Delay in seconds before movement starts")]
	[SerializeField] private float moveDelay = 2f;

	private Rigidbody2D _Rigidbody;
	private Collider2D _Collider;

	void Start () 
	{
		_Rigidbody = GetComponent<Rigidbody2D>();
		_Collider = GetComponent<Collider2D>();
		Invoke("chase", moveDelay);
	}

	void chase()
	{
		_Collider.enabled = true;
		Vector2 targetPos = (target.transform.position - transform.position).normalized;
		_Rigidbody.velocity = targetPos * moveSpeed;
	}
}
