using UnityEngine;
using System.Collections;

public class DestroyAfterTime : MonoBehaviour
{

	public float lifetime;

	void Start ()
	{

	}
	
	void Update ()
	{
		if (lifetime <= 0f)
			return;

		lifetime -= Time.deltaTime;
		if (lifetime <= 0f)
			Destroy(gameObject);
	}
}
