using UnityEngine;
using System.Collections;

public class FreezeFrogsBreath : MonoBehaviour
{

	public int poolSize;
	public float lifetime, fireSpeed, fireRate;
	public Vector3 spawnOffset;
		
	public GameObject prefab;
	public ObjectPool pool;
	public Transform parentObject;
	public ParticleSystem particles;

	public FreezeFrogsTarget[] targets;

	private float fireTime;

	void Start()
	{
		reset();
	}

	public void reset()
	{
		fireTime = 0f;

		int tries = 100;
		while (parentObject.childCount > 0 && tries > 0)
		{
			pool.poolObject(parentObject.GetChild(0).gameObject);
			tries--;
		}

		if (tries == 0)
			Debug.Log("TOO MANY TRIES");
	}
	
	void Update ()
	{

		if (!MicrogameController.instance.getVictory())
		{
			bool done = true;
			for (int i = 0; i < targets.Length; i++)
			{
				if (targets[i].progress < 1)
					done = false;
				if (!targets[i].gameObject.activeInHierarchy)
					Debug.Log("WRONG FROG");
			}
			if (done)
			{
				MicrogameController.instance.setVictory(true, true);
			}
		}

		if (fireRate <= 0f)
			return;

		fireTime += Time.deltaTime;
		if (fireTime > 1f / fireRate)
		{
			emit();
			fireTime -= 1f / fireRate;
		}
	}

	void emit()
	{
		GameObject breathParticle = pool.getObjectFromPool();
		breathParticle.transform.position = transform.position;
		breathParticle.transform.Translate(spawnOffset, transform);
		breathParticle.name = "Breath";
		breathParticle.GetComponent<Rigidbody2D>().velocity = MathHelper.getVector2FromAngle(getAngle(), fireSpeed);
		breathParticle.transform.parent = parentObject;
	}

	public float getAngle()
	{
		return transform.rotation.eulerAngles.z;
	}
}
