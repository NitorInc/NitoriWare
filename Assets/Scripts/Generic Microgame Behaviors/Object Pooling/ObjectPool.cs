using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectPool : MonoBehaviour
{
	public bool addChildrenToPool;
	public int createObjectsOnAwake;

	public GameObject prefab;

	private List<GameObject> pool, poolArchive;


	void Awake()
	{
		if (addChildrenToPool)
		{
			addChildren();
		}
		if (createObjectsOnAwake > 0)
		{
			addToPool(createObjectsOnAwake, false);
		}
	}

	void addChildren()
	{
		for (int i = 0; i < transform.childCount; i++)
		{
			poolObject(transform.GetChild(i).gameObject);
		}
	}

	/// <summary>
	/// Returns true if the pool size is 0
	/// </summary>
	/// <returns></returns>
	public bool isEmpty()
	{
		return pool == null || pool.Count == 0;
	}

	/// <summary>
	/// Returns the amount of objects stored in the pool
	/// </summary>
	/// <returns></returns>
	public float getPoolSize()
	{
		return pool.Count;
	}

	/// <summary>
	/// Creates the specified amount of prefabs in the pool
	/// </summary>
	/// <param name="count"></param>
	/// <param name="onlyIfEmpty"></param>
	/// <returns></returns>
	public bool addToPool(int count, bool onlyIfEmpty)
	{
		if (prefab == null)
		{
			Debug.Log("Trying to make a pool from null prefab!");
			return false;
		}
		if (onlyIfEmpty && pool != null)
			return false;

		for (int i = 0; i < count; i++)
		{
			poolObject((GameObject)GameObject.Instantiate(prefab));
		}
		return true;
	}

	/// <summary>
	/// Deactivates an object and pools it
	/// </summary>
	/// <param name="objectToPool"></param>
	public void poolObject(GameObject objectToPool)
	{
		if (pool == null)
		{
			pool = new List<GameObject>();
			poolArchive = new List<GameObject>();
		}
		pool.Add(objectToPool);
		objectToPool.SetActive(false);
			objectToPool.transform.parent = transform;

		if (!poolArchive.Contains(objectToPool))
			poolArchive.Add(objectToPool);
	}

	/// <summary>
	/// Deactivates and pools every object that has ever been in the pool
	/// </summary>
	public void poolAllObjects()
	{
		foreach (GameObject objectToPool in poolArchive)
		{
			if (!pool.Contains(objectToPool))
			{
				poolObject(objectToPool);
			}
		}
	}

	/// <summary>
	/// Activates and unpools the object at a epecific index
	/// </summary>
	/// <param name="index"></param>
	/// <returns></returns>
	public GameObject getObjectFromPoolAtIndex(int index)
	{
		GameObject poolObject = pool[index];
		pool.RemoveAt(index);
		poolObject.transform.parent = null;
		poolObject.SetActive(true);
		PoolAfterTime _poolAfterTime = poolObject.GetComponent<PoolAfterTime>();
		if (_poolAfterTime != null && _poolAfterTime.startTimerOnUnpool)
		{
			_poolAfterTime.reset();
		}
		return poolObject;
	}

	/// <summary>
	/// Activates and unpools an object
	/// </summary>
	/// <returns></returns>
	public GameObject getObjectFromPool()
	{
		return getObjectFromPoolAtIndex(0);
	}

	/// <summary>
	/// Activates and unpools a specific object
	/// </summary>
	/// <param name="gameObject"></param>
	/// <returns></returns>
	public GameObject getObjectFromPool(GameObject gameObject)
	{
		return getObjectFromPoolAtIndex(pool.IndexOf(gameObject));
	}

	/// <summary>
	/// Returns a reference to a pooled object without unpooling it
	/// </summary>
	/// <returns></returns>
	public GameObject getObjectWithoutUnpooling()
	{
		return getObjectWithoutUnpooling(0);
	}

	/// <summary>
	/// Returns a reference to the pooled object at a specfic index without unpooling it
	/// </summary>
	/// <param name="index"></param>
	/// <returns></returns>
	public GameObject getObjectWithoutUnpooling(int index)
	{
		return pool[index];
	}

}
