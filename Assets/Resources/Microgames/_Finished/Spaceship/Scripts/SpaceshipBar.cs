using UnityEngine;
using System.Collections;

public class SpaceshipBar : MonoBehaviour
{
	public float speed, threshold, maxScale, arrowMinY, arrowMaxY;
	public Transform arrow;
	private sbyte goingUp;
	private float progress_;
	private float progress{
	get{
	return progress_;
	}
	set{
	progress_ = value;
	if (progress_ != Mathf.Clamp01(progress)){
	progress = Mathf.Clamp01(progress);
	goingUp *= -1;
	}
	}
	}
	void Start()
	{
		progress = 0f;
		goingUp = 1;
		updatePosition();
	}

	void Update()
	{
		progress = progress + (speed * Time.deltaTime * (float)goingUp);
		updatePosition();

	}

	public bool isWithinThreshold()
	{
		return progress >= threshold;
	}

	void updatePosition()
	{
		transform.localScale = new Vector3(1f, Mathf.Lerp(maxScale, 0f, progress), 1f);
		arrow.localPosition = new Vector3(0f, Mathf.Lerp(arrowMinY, arrowMaxY, progress), 0f);
	}
}
