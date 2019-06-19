using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OniPunchMountainPart : MonoBehaviour 
{
	public float velocityMultiplier = 1f;
	public float angularVelocityMultiplier = 30f;

	private void OnEnable()
	{
		Rigidbody2D rb = GetComponent<Rigidbody2D>();
		Vector2 randomForce = ((Vector2)transform.position.normalized * velocityMultiplier)
            + (Random.insideUnitCircle.normalized * velocityMultiplier / 3f);
		randomForce.y = Mathf.Max(randomForce.y, 0f);
		rb.velocity = (Vector2.up * velocityMultiplier * 1.5f) + randomForce;
		rb.angularVelocity = Random.Range(-angularVelocityMultiplier, angularVelocityMultiplier);
	}
}
