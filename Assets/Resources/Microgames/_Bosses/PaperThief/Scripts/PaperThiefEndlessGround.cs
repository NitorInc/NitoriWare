using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperThiefEndlessGround : MonoBehaviour
{
	[SerializeField]
	private Transform shiftTransform;

	void Update()
	{
		if (Camera.main.transform.parent.parent == null)
			return;
		while (transform.position.x < Camera.main.transform.position.x && CameraHelper.isObjectOffscreen(transform, transform.localScale.x / 2f))
		{
			shiftTransform.localPosition += Vector3.left * transform.localScale.x;
		}
	}
}
