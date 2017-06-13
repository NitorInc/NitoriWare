using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PaperThiefZIndexer : MonoBehaviour
{
	[SerializeField]
	private float zIndex;

	void Start ()
	{
		transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, zIndex);
	}
	
	void LateUpdate ()
	{
		transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, getZIndexMult() * zIndex);
	}

	float getZIndexMult()
	{
		float degrees = transform.rotation.eulerAngles.y % 360;
		bool flip = degrees > 90 && degrees < 270;
		if (MathHelper.Approximately(degrees, 0f, .1f) || MathHelper.Approximately(degrees, 180f, .1f))
			return (flip ? -1f : 1f);
		degrees %= 180;
		if (degrees > 90)
			degrees = 180 - degrees;
		return (90 - degrees) / 90 * (flip ? -1f : 1f);

	}
}
