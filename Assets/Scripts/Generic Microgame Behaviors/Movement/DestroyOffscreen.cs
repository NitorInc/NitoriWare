using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOffscreen : MonoBehaviour
{
	[SerializeField]
	private float forceDistance;

	void Update ()
	{
		if (forceDistance > 0f)
		{
			if (CameraHelper.isObjectOffscreen(transform, forceDistance))
				Destroy(gameObject);
		}
		else
		{
			if (CameraHelper.isObjectOffscreen(transform))
				Destroy(gameObject);
		}
	}
}
