using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperThiefSpin : MonoBehaviour 
{
	[SerializeField]
	private int rotateSpeed;

	[SerializeField]
	private bool _facingRight;
	public bool facingRight
	{
		get {return _facingRight;}
		set {_facingRight = value;}
	}

	void Start()
	{
		_facingRight = MathHelper.Approximately(getSpinRotation(), -180f, 1f);
	}

	void Update()
	{
		updateRotation();
	}

	void updateRotation()
	{
		float rotation = getSpinRotation();

		//Spin between 0 and -180 degrees
		float goalRotation = _facingRight ? -180f : 0f;
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
		Vector3 eulers = transform.localRotation.eulerAngles;
		return eulers.y <= 0f ? eulers.y : eulers.y - 360f;
	}

	void setSpinRotation(float rotation)
	{
		Vector3 eulers = transform.localRotation.eulerAngles;
		transform.localRotation = Quaternion.Euler(eulers.x, rotation, eulers.z);
	}
}
