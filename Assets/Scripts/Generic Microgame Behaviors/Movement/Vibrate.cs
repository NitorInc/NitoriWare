using UnityEngine;
using System.Collections;

public class Vibrate : MonoBehaviour
{

	//Simulates shaking or vibration by moving randomly within specified parameters
	//Attach to a parent object, because it will directly edit localPosition

	public bool vibrateOn;
	public float vibrateSpeed, vibrateMaxX, vibrateMaxY;
	private Vector2 vibrateGoal;


	void Start ()
	{
	
	}
	
	void Update ()
	{
		if (vibrateOn)
			updateVibrate();
		else
			transform.localPosition = new Vector3(0f, 0f, transform.localPosition.z);
	}


	void updateVibrate()
	{
		Vector2 diff = vibrateGoal - (Vector2)transform.localPosition;
		if (diff.magnitude <= vibrateSpeed * Time.deltaTime)
		{

			transform.localPosition = new Vector3(vibrateGoal.x, vibrateGoal.y, transform.localPosition.z);
			resetVibrateGoal();
		}
		else
		{
			transform.localPosition += (Vector3)MathHelper.resizeVector2D((vibrateGoal - (Vector2)transform.localPosition), vibrateSpeed * Time.deltaTime);

				//(Vector3)MathHelper.getVectorFromAngle2D(MathHelper.getVectorAngle2D(vibrateGoal - (Vector2)transform.localPosition), vibrateSpeed * Time.deltaTime);
		}

	}


	void resetVibrateGoal()
	{
		vibrateGoal = new Vector2(Random.Range(-1f * vibrateMaxX, vibrateMaxX), Random.Range(-1f * vibrateMaxY, vibrateMaxY));
	}
}
