using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CupShuffleLilypad : MonoBehaviour
{
	public Transform snapTo;
	public Vector2 snapOffset;

	void Start()
	{
		snap();
	}
	
	void LateUpdate()
	{
		snap();
	}

	void snap()
	{
		transform.localPosition = snapTo.localPosition + (Vector3)snapOffset;
	}
}
