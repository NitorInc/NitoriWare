using UnityEngine;
using System.Collections;



public class PoolAfterTime : MonoBehaviour
{
	public float lifetime;
	public bool startTimerOnUnpool;
	public ObjectPool pool;

	public float timeLeft;

	void Start()
	{
		timeLeft = lifetime;
	}

	/// <summary>
	/// Resets the timer so the object will be pooled in the time given
	/// </summary>
	public void reset()
	{
		reset(lifetime);
	}

	/// <summary>
	/// Sets lifetime and resets the timer
	/// </summary>
	/// <param name="lifetime"></param>
	public void reset(float lifetime)
	{
		this.lifetime = lifetime;
		timeLeft = lifetime;
	}
	
	void Update ()
	{
		if (timeLeft > 0f)
		{
			timeLeft -= Time.deltaTime;
			if (timeLeft <= 0f)
			{
				timeLeft = 0f;
				pool.poolObject(gameObject);
			}
		}
	}

}
