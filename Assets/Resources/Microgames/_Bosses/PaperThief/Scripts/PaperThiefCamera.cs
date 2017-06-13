using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperThiefCamera : MonoBehaviour
{
	[SerializeField]
	private Transform follow;

	private BoxCollider2D boxCollider;

	void Awake()
	{
		boxCollider = GetComponent<BoxCollider2D>();
	}
	
	void Update()
	{
		boxCollider.enabled = true;

		Vector3 bounds = boxCollider.bounds.extents;
		transform.position = new Vector3(
			Mathf.Clamp(transform.position.x, follow.position.x - bounds.x, follow.position.x + bounds.x),
			Mathf.Clamp(transform.position.y, follow.position.y - bounds.y, follow.position.y + bounds.y),
			transform.position.z);

		boxCollider.enabled = false;
	}
}
