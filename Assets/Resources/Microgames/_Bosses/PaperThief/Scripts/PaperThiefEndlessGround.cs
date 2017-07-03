using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperThiefEndlessGround : MonoBehaviour
{
	[SerializeField]
	private Transform[] shiftTransforms;

	void Update()
	{
		if (Camera.main.transform.parent.parent == null)
			return;
		while (transform.position.x < Camera.main.transform.position.x && CameraHelper.isObjectOffscreen(transform, transform.localScale.x / 2f))
		{
            for (int i = 0; i < shiftTransforms.Length; i++)
            {
                shiftTransforms[i].localPosition += Vector3.left * transform.localScale.x;
            }
		}
	}
}
