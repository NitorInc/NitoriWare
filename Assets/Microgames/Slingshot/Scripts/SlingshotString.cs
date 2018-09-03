using UnityEngine;
using System.Collections;

public class SlingshotString : MonoBehaviour
{

	public Vector2 breakVelocity;
	public float breakTorque, breakGravity;

	public Transform anchor1, anchor2;

	public SlingshotAmmo ammo;

	private Rigidbody2D rigidThing;


	void Start()
	{
		rigidThing = GetComponent<Rigidbody2D>();
	}
	
	void LateUpdate()
	{
		if (ammo.state != SlingshotAmmo.State.Broken)
		{
			rigidThing.isKinematic = true;
			updatePosition();
		}
		else
		{
			if (rigidThing.isKinematic)
			{
				rigidThing.isKinematic = false;
				rigidThing.velocity = breakVelocity;
			}
			rigidThing.gravityScale = breakGravity;
			rigidThing.AddTorque(breakTorque);
		}
	}

	void updatePosition()
	{
		transform.position = new Vector3((anchor1.position.x + anchor2.position.x) / 2f,
			(anchor1.position.y + anchor2.position.y) / 2f, transform.position.z);

		float scale = ((Vector2)anchor1.transform.position - (Vector2)anchor2.transform.position).magnitude;

		transform.localRotation = Quaternion.Euler(0f, 0f,
			((Vector2)(anchor2.transform.position - anchor1.transform.position)).getAngle());

		transform.localScale = new Vector3(scale, transform.localScale.y, transform.localScale.z);
	}
}
