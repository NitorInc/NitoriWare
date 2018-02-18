using UnityEngine;
using System.Collections;

public class FreezeFrogsHead : MonoBehaviour
{

	public float minAngle, maxAngle, rotateSpeed;

	private float angle;

	void Awake()
	{

		reset();
	}

	public void reset()
	{
		angle = 0f;
		setAngle(angle);
	}
	
	void Update ()
	{
		if (Input.GetKey(KeyCode.UpArrow))
		{
			angle += rotateSpeed * Time.deltaTime;
		}
		if (Input.GetKey(KeyCode.DownArrow))
		{
			angle -= rotateSpeed * Time.deltaTime;
		}
		angle = Mathf.Clamp(angle, minAngle, maxAngle);
		setAngle(angle);
	}


	public void setAngle(float angle)
	{
		transform.rotation = Quaternion.Euler(0f, 0f, angle * Mathf.Rad2Deg * Mathf.Rad2Deg);
	}

	public float getAngle()
	{
		return transform.rotation.eulerAngles.z * Mathf.Deg2Rad;
	}

}
