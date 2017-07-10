using UnityEngine;
using System.Collections;

public class SpaceshipBar : MonoBehaviour
{
	public float speed, threshold, maxScale, arrowMinY, arrowMaxY;
	public Transform arrow;
	private float timer;
	private float progress;
	void Start()
	{
		progress = 0f;
		timer = 0f
		updatePosition();
	}

	void Update()
	{
		timer += time.deltaTime;
		progress = Mathf.PingPong(speed * timer, 1.0f);
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
