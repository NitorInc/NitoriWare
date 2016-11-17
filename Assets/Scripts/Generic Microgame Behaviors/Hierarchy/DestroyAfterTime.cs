using UnityEngine;
using System.Collections;

public class DestroyAfterTime : MonoBehaviour
{

	public float lifetime;
	
	void Update ()
	{

		lifetime -= Time.deltaTime;
		if (lifetime <= 0f)
			Destroy(gameObject);
	}
}
