using UnityEngine;
using System.Collections;

public class SpaceshipBar : MonoBehaviour
{
	public float speed, threshold, maxScale, arrowMinY, arrowMaxY;
	public Transform arrow;

	private float progress;
	private bool goingUp;

	void Start()
	{
		progress = 0f;
		goingUp = true;
		updatePosition();
	}

	void Update()
	{
		progress += speed * Time.deltaTime * (goingUp ? 1f : -1f);
		
		if (progress > 1f || progress < 0f)
		{
			progress = Mathf.Clamp(progress, 0f, 1f);
			goingUp = !goingUp;
		}

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