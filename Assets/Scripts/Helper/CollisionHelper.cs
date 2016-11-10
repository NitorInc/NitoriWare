using UnityEngine;
using System.Collections;

public class CollisionHelper
{
	/// <summary>
	/// Ignores collision for every single GameObject with a particular tag. If you want to do this when a microgame starts, call it in the OnStart event in MicrogameController or it will be undone!
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
}
