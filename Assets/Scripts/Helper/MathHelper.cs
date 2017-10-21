using UnityEngine;
using System.Collections;

public static class MathHelper
{

	/// <summary>
	/// Gets the angle of a Vector2 (degrees)
	/// </summary>
	/// <param name="vector"></param>
	/// <returns></returns>
	public static float getAngle(this Vector2 vector)
	{
		if (vector == Vector2.zero)
			return 0f;
		else if (vector.x >= 0f)
			return Mathf.Atan(vector.y / vector.x) * Mathf.Rad2Deg;
		else
			return (Mathf.Atan(vector.y / vector.x) + Mathf.PI) * Mathf.Rad2Deg;
	}

    /// <summary>
    /// Returns modulus that works with negative numbers (always between 0 and m)
    /// </summary>
    /// <param name=""></param>
    /// <returns></returns>
    public static float trueMod(float x, float m)
    {
        float r = x % m;
        while (r < 0f)
        {
            r += m;
        }
        return r;
    }

    /// <summary>
    /// Returns modulus that works with negative numbers (always between 0 and m)
    /// </summary>
    /// <param name=""></param>
    /// <returns></returns>
    public static int trueMod(int x, int m)
    {
        int r = x % m;
        while (r < 0)
        {
            r += m;
        }
        return r;
    }

    /// <summary>
    /// Returns a 2D vector from an angle (degrees) and magnitude
    /// </summary>
    /// <param name="angle"></param>
    /// <param name="magnitude"></param>
    /// <returns></returns>
    public static Vector2 getVector2FromAngle(float angle, float magnitude)
	{
		return new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad) * magnitude, Mathf.Sin(angle * Mathf.Deg2Rad) * magnitude);
	}

	/// <summary>
	/// Returns a Vector3 with the same direction resized to the given magnitude
	/// </summary>
	/// <param name="vector"></param>
	/// <param name="magnitude"></param>
	/// <returns></returns>
	public static Vector3 resize(this Vector3 vector, float magnitude)
	{
		return Vector3.Scale(vector, Vector3.one * (magnitude / vector.magnitude));
	}

	/// <summary>
	/// Returns a Vector3 with the same direction resized to the given magnitude
	/// </summary>
	/// <param name="vector"></param>
	/// <param name="magnitude"></param>
	/// <returns></returns>
	public static Vector2 resize(this Vector2 vector, float magnitude)
	{
		return Vector2.Scale(vector, Vector2.one * (magnitude / vector.magnitude));
	}

	/// <summary>
	/// Returns the shortest distance between two angles (degrees)
	/// </summary>
	/// <param name="angle"></param>
	/// <param name="target"></param>
	/// <returns></returns>
	public static float getAngleDifference(float angle, float target)
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
	/// Returns True or False randomly
	/// </summary>
	/// <returns></returns>
	public static bool randomBool()
	{
		return Random.value >= 0.5;
	}

	/// <summary>
	/// Alternative method to Mathf.approximately with a proximity threshold
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
	public static float randomRangeFromVector(Vector2 range)
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
	public static bool moveTowards(this Transform transform, Vector3 goalPosition, float speed)
	{
		float diff = speed * Time.deltaTime;

		Vector3 relativeGoalPosition = goalPosition - transform.position;

		if (relativeGoalPosition.magnitude <= diff)
		{
			transform.position = goalPosition;
			return true;
		}
		else
		{
			transform.position += relativeGoalPosition.resize(diff);
			return false;
		}
	}

    /// <summary>
    /// (2D) Moves an object (frame by frame) to a certain point in 2D space, will return true if the point is reached that frame
    /// </summary>
    /// <param name="transform"></param>
    /// <param name="goalPosition"></param>
    /// <param name="speed"></param>
    /// <returns></returns>
    public static bool moveTowards2D(this Transform transform, Vector2 goalPosition, float speed)
	{
		return moveTowards(transform, new Vector3(goalPosition.x, goalPosition.y, transform.position.z), speed);
	}

	/// <summary>
	/// Moves an object (frame by frame) to a certain point locally, will return true if the point is reached that frame
	/// </summary>
	/// <param name="transform"></param>
	/// <param name="goalPosition"></param>
	/// <param name="speed"></param>
	/// <returns></returns>
	public static bool moveTowardsLocal(Transform transform, Vector3 goalPosition, float speed)
	{
		float diff = speed * Time.deltaTime;

		Vector3 relativeGoalPosition = goalPosition - transform.localPosition;

		if (relativeGoalPosition.magnitude <= diff)
		{
			transform.localPosition = goalPosition;
			return true;
		}
		else
		{
			transform.localPosition += relativeGoalPosition.resize(diff);
			return false;
		}
	}
    /// <summary>
    /// (2D) Moves an object (frame by frame) to a certain point in 2D space, will return true if the point is reached that frame
    /// </summary>
    /// <param name="transform"></param>
    /// <param name="goalPosition"></param>
    /// <param name="speed"></param>
    /// <returns></returns>
    public static bool moveTowardsLocal2D(this Transform transform, Vector2 goalPosition, float speed)
	{
		return moveTowardsLocal(transform, new Vector3(goalPosition.x, goalPosition.y, transform.localPosition.z), speed);
	}

}
 