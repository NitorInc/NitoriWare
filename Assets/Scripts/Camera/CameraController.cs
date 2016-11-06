using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{

	public static CameraController instance;

	public float shakeCoolRate;

	private Camera _camera;
	private float xShake, yShake, shakeSpeed;
	private Vector3 initialPosition;
	private Vector3 goalPosition;

	void Awake()
	{
		//instance = this;
		initialPosition = transform.localPosition;
		goalPosition = initialPosition;

		_camera = GetComponent<Camera>();

		_camera.aspect = 4f / 3f;
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
	/// Set the x amplitude for the screen to shake
	/// </summary>
	/// <param name="shake"></param>
	public void setXShake(float shake)
	{
		xShake = shake;
	}

	/// <summary>
	/// Set the y amplitude for the screen to shake
	/// </summary>
	/// <param name="shake"></param>
	public void setYShake(float shake)
	{
		yShake = shake;
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
	/// Adds to the screenshake x amplitude instead of overriding it
	/// </summary>
	/// <param name="shake"></param>
	public void addXShake(float shake)
	{
		xShake += shake;
	}


	/// <summary>
	/// Adds to the screenshake y amplitude instead of overriding it
	/// </summary>
	/// <param name="shake"></param>
	public void addYShake(float shake)
	{
		yShake += shake;
	}


	/// <summary>
	/// Sets the speed at which the camera randomly moves around. Default is 0, meaning infinite (screen will randomly shift every frame)
	/// </summary>
	/// <param name="shakeSpeed"></param>
	public void setShakeSpeed(float shakeSpeed)
	{
		this.shakeSpeed = shakeSpeed;
	}

	/// <summary>
	/// Resets all parameters to their default values, centers camera, and stops shake
	/// </summary>
	public void reset()
	{
		xShake = yShake = shakeSpeed = 0f;
		shakeCoolRate = 1f;
		transform.localPosition = initialPosition;
		goalPosition = initialPosition;
	}

	/// <summary>
	/// Sets the rate at which shake dies down
	/// </summary>
	/// <param name="coolRate"></param>
	public void setShakeCoolRate(float coolRate)
	{
		this.shakeCoolRate = coolRate;
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
		else
		{
			if (MathHelper.moveTowards2D(transform, (Vector2)goalPosition, shakeSpeed))
			{
				resetGoal();
			}
		}



		if (xShake > 0f)
		{
			//Debug.Log(shakeCoolRate + " " + xShake);
			xShake -= shakeCoolRate * Time.deltaTime;
			if (xShake <= 0f)
			{
				xShake = 0f;
			}
		}

		if (yShake > 0f)
		{
			yShake -= shakeCoolRate * Time.deltaTime;
			if (yShake <= 0f)
			{
				yShake = 0f;
			}
		}


		if (xShake == 0f && yShake == 0f)
		{
			goalPosition = initialPosition;
			MathHelper.moveTowards2D(transform, (Vector2)goalPosition, shakeSpeed);
		}
	}
}
