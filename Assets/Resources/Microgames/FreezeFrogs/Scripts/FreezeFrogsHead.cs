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

	//void Start()
	//{
	//	reset();
	//}

	public void reset()
	{
		angle = 0f;
		setAngle(angle);
	}
	
	void Update ()
	{
		angle += Input.GetKey(KeyCode.UpArrow) && angle < maxAngle ? rotateSpeed * Time.deltaTime : Input.GetKey(KeyCode.DownArrow) && angle > minAngle ? -rotateSpeed * Time.deltaTime : 0f;
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
