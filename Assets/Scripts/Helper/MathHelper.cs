using UnityEngine;
using System.Collections;

public class MathHelper
{

	/// <summary>
	/// Gets the angle of a vector (degrees)
	/// </summary>
	/// <param name="vector"></param>
	/// <returns></returns>
	public static float getVectorAngle2D(Vector2 vector)
	{
		if (vector == Vector2.zero)
			return 0f;
		else if (vector.x >= 0f)
			return Mathf.Atan(vector.y / vector.x) * Mathf.Rad2Deg;
		else
			return (Mathf.Atan(vector.y / vector.x) + Mathf.PI) * Mathf.Rad2Deg;
	}

	/// <summary>
	/// Returns a 2D vector from an angle (degrees) and magnitude
	/// </summary>
	/// <param name="angle"></param>
	/// <param name="magnitude"></param>
	/// <returns></returns>
	public static Vector2 getVectorFromAngle2D(float angle, float magnitude)
	{
		return new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad) * magnitude, Mathf.Sin(angle * Mathf.Deg2Rad) * magnitude);
	}

	/// <summary>
	/// Returns a vector with the same angle as the given vector, resized to the given magnitude
	/// </summary>
	/// <param name="vector"></param>
	/// <param name="magnitude"></param>
	/// <returns></returns>
	public static Vector2 resizeVector2D(Vector2 vector, float magnitude)
	{
		float angle = getVectorAngle2D(vector) * Mathf.Deg2Rad;
		return new Vector2(Mathf.Cos(angle) * magnitude, Mathf.Sin(angle) * magnitude);
	}

	/// <summary>
	/// Returns the shortest distance between two angles (degrees)
	/// </summary>
	/// <param name="angle"></param>
	/// <param name="target"></param>
	/// <returns></returns>
	public static float getAngleDifferences(float angle, float target)
	{
		float dist = 0f;
		int direction;

		if (angle < 180f)
		{
			if (angle < target)
				if (target - 180f == angle)
				{
					dist = 180f;
					direction = 0;
				}
				else if (target - 180f < angle)
				{
					dist = target - angle;
					direction = 1;
				}
				else
				{
					dist = angle + (360f - target);
					direction = -1;
				}
			else
			{
				dist = angle - target;
				direction = -1;
			}
		}
		else
		{
			if (target < angle)
				if (angle - 180 == target)
				{
					dist = 180;
					direction = 0;
				}
				else if (angle - 180 < target)
				{
					dist = angle - target;
					direction = -1;
				}
				else
				{
					dist = target + (360 - angle);
					direction = 1;
				}
			else
			{
				dist = target - angle;
				direction = 1;
			}
		}
		return dist * (float)direction;
	}

	/// <summary>
	/// An alternateative to Mathf.approximately with a settable proximity threshold
	/// </summary>
	/// <param name="value1"></param>
	/// <param name="value2"></param>
	/// <param name="threshold"></param>
	/// <returns></returns>
	public static bool Approximately(float value1, float value2, float threshold)
	{
		return Mathf.Abs(value2 - value1) < threshold;
	}

	/// <summary>
	/// Returns the 2D distance (no z) between two vector3's
	/// </summary>
	/// <param name="position1"></param>
	/// <param name="position2"></param>
	/// <returns></returns>
	public static float get2DDistance(Vector3 position1, Vector3 position2)
	{
		//Debug.Log(position1 + " " + position2);
		return (new Vector2(position1.x, position1.y) - new Vector2(position2.x, position2.y)).magnitude;
	}


	/// <summary>
	/// Multiplies a color to a certain lightness
	/// </summary>
	/// <param name="color"></param>
	/// <param name="lightness"></param>
	/// <returns></returns>
	public static Color multiplyColor(Color color, float lightness)
	{
		color.r *= lightness;
		color.g *= lightness;
		color.b *= lightness;
		return color;
	}

	/// <summary>
	/// Returns a random float value between the x and y values of the given Vector2
	/// </summary>
	/// <param name="range"></param>
	/// <returns></returns>
	public static float randomRange(Vector2 range)
	{
		return Random.Range(range.x, range.y);
	}

	/// <summary>
	/// Moves an object (frame by frame) to a certain point, will return true if the point is reached that frame
	/// </summary>
	/// <param name="transform"></param>
	/// <param name="goalPosition"></param>
	/// <param name="speed"></param>
	/// <returns></returns>
	public static bool moveTowards2D(Transform transform, Vector2 goalPosition, float speed)
	{
		float diff = speed * Time.deltaTime;

		Vector2 relativePosition = goalPosition - (Vector2)transform.position;

		if (relativePosition.magnitude <= diff)
		{
			transform.position = new Vector3(goalPosition.x, goalPosition.y, transform.position.z);
			return true;
		}
		else
		{
			transform.Translate((Vector3)resizeVector2D(relativePosition, diff));
			return false;
		}
	}

	public static float clamp(float var, float min, float max)
	{
		return Mathf.Min(Mathf.Max(var, min), max);
	}

}
 