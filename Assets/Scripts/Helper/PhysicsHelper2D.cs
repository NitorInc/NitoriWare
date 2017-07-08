using UnityEngine;
using System.Collections;

public class PhysicsHelper2D
{
	/// <summary>
	/// Ignores collision for every single GameObject with a particular tag
	/// </summary>
	/// <param name="object1"></param>
	/// <param name="tag"></param>
	/// <param name="ignore"></param>
	public static void ignoreCollisionWithTag(GameObject object1, string tag, bool ignore)
	{
		Collider2D[] colliders = object1.GetComponents<Collider2D>();

		GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag(tag);
		foreach (GameObject taggedObject in taggedObjects)
		{
			for (int i = 0; i < colliders.Length; i++)
			{
				Physics2D.IgnoreCollision(colliders[i], taggedObject.GetComponent<Collider2D>(), ignore);
			}
		}
	}

	/// <summary>
	/// performs Physics2D.raycast and also Debug.DrawLine with the same vector
	/// </summary>
	public static RaycastHit2D visibleRaycast(Vector2 origin, Vector2 direction,
        float distance = float.PositiveInfinity, int layerMask = Physics2D.DefaultRaycastLayers,
        float minDepth = -Mathf.Infinity, float maxDepth = Mathf.Infinity,
        Color? color = null)
	{
		Debug.DrawLine(origin, origin + (direction.resize(distance)), color ?? Color.white);
		return Physics2D.Raycast(origin, direction, distance, layerMask, minDepth, maxDepth);
	}
}
