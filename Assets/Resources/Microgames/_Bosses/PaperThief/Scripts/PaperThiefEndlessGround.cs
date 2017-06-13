using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperThiefEndlessGround : MonoBehaviour
{

	void Update()
	{
		for (int i = 0; i < transform.childCount; i++)
		{
			Transform floorPiece = transform.GetChild(i);
			if (floorPiece.position.x < Camera.main.transform.position.x && CameraHelper.isObjectOffscreen(floorPiece))
				floorPiece.transform.localPosition += Vector3.right * (floorPiece.transform.localScale.x * (float)transform.childCount);
		}
	}
}
