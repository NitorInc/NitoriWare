using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{

	public static CameraShake instance;

	public float xShake, yShake, shakeSpeed, shakeCoolRate = 1f;
	private Vector3 initialPosition;
	private Vector3 goalPosition;
    private float initialXShake, initialYShake;

	void Awake()
	{
		instance = this;

		initialPosition = transform.localPosition;
        initialXShake = xShake;
        initialYShake = yShake;
		goalPosition = initialPosition;
	}
	
	void Update ()
	{	
		updateScreenshake();
	}

	/// <summary>
	/// Set the amplitude for the screen to shake
	/// </summary>
	/// <param name="shake"></param>
	public void setScreenShake(float shake)
	{
		xShake = yShake = shake;
	}

	/// <summary>
	/// Adds to the screenshake amplitude instead of overriding it
	/// </summary>
	/// <param name="shake"></param>
	public void addScreenShake(float shake)
	{
		xShake += shake;
		yShake += shake;
	}

    /// <summary>
    /// Resets shake x and y values to initial values;
    /// </summary>
    public void resetShake()
    {
        xShake = initialXShake;
        yShake = initialYShake;
    }

	/// <summary>
	/// Centers camera and stops shake
	/// </summary>
	public void reset()
	{
		xShake = yShake = shakeSpeed = 0f;
		shakeCoolRate = 1f;
		transform.localPosition = initialPosition;
		goalPosition = initialPosition;
	}

	void resetGoal()
	{
		goalPosition = new Vector3(Random.Range(-1f * xShake, xShake), Random.Range(-1f * yShake, yShake), initialPosition.z);
	}

	void updateScreenshake()
	{
		if (xShake == 0f && yShake == 0f && transform.localPosition == goalPosition)
			return;

		if (shakeSpeed <= 0f)
		{
			resetGoal();
			transform.localPosition = goalPosition;
		}
		else if ((xShake + yShake > 0f) && transform.moveTowardsLocal2D((Vector2)goalPosition, shakeSpeed))
		{
			resetGoal();
		}



		if (xShake > 0f)
		{
			xShake -= shakeCoolRate * Time.deltaTime;
			xShake = Mathf.Max(xShake, 0f);
		}

		if (yShake > 0f)
		{
			yShake -= shakeCoolRate * Time.deltaTime;
			yShake = Mathf.Max(yShake, 0f);
		}


		if (xShake + yShake == 0f)
		{
			goalPosition = initialPosition;
			transform.moveTowardsLocal2D((Vector2)goalPosition, shakeSpeed);
		}
	}
}
