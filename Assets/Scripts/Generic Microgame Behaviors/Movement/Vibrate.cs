using UnityEngine;
using System.Collections;

public class Vibrate : MonoBehaviour
{

	//Simulates shaking or vibration by moving randomly within specified parameters
	//Attach to a parent object, because it will directly edit localPosition

	[SerializeField]
	private bool _vibrateOn;

	public bool vibrateOn
	{
		get { return _vibrateOn; }
		set
		{
			if (!value && resetOnStop)
				resetPosition();
			else if (!value && resetOnStart)
				resetVibrateGoal();
			_vibrateOn = value;
		}
	}
	public float vibrateSpeed, vibrateMaxX, vibrateMaxY;
	private Vector2 vibrateGoal, offset;
	public bool relativeToStartPosition, resetOnStop, resetOnStart;


	void Awake()
	{
		resetOffset();
		resetVibrateGoal();
	}

	void resetOffset()
	{
		offset = relativeToStartPosition ? (Vector2)transform.localPosition : Vector2.zero;
	}

	void setOffset(Vector3 position)
	{
		offset = (Vector2)position;
	}

	void Update()
	{
		if (vibrateOn)
			updateVibrate();
	}

	void resetPosition()
	{
		transform.localPosition = new Vector3(0f, 0f, transform.localPosition.z);
	}

	void updateVibrate()
	{
		if (MathHelper.moveTowardsLocal2D(transform, vibrateGoal, vibrateSpeed))
			resetVibrateGoal();
	}


	void resetVibrateGoal()
	{
		vibrateGoal = new Vector2(Random.Range(-1f * vibrateMaxX, vibrateMaxX), Random.Range(-1f * vibrateMaxY, vibrateMaxY)) + offset;
	}
}
