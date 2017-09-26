using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuukotoSweepPatch : MonoBehaviour
{
    public RuukotoSweep_Movement playerMovement;
    public ParticleSystem leafParticles;
    public int spawnCountMin, spawnCountMax;
    public Vector2 xSpawnBounds, ySpawnBounds;
    
	void Start ()
    {
        leafParticles.Emit(Random.Range(spawnCountMin, spawnCountMax));
        var leafModule = leafParticles.main;
        leafModule.simulationSpeed = 0f;

        setStartPosition();
	}

    void setStartPosition()
    {
        do
        {
            transform.position = new Vector3(MathHelper.randomRangeFromVector(xSpawnBounds), MathHelper.randomRangeFromVector(ySpawnBounds), transform.position.z);
        }
        while (isTooClose());
    }

    bool isTooClose()
    {
        for (int i = 0; i < transform.parent.childCount; i++)
        {
            //Debug.Log(((Vector2)(transform.parent.GetChild(i).position - transform.position)).magnitude);
            Transform childTransform = transform.parent.GetChild(i);
            if (childTransform != transform && ((Vector2)(childTransform.position - transform.position)).magnitude < 1f)
                return true;
        }
        return ((Vector2)(transform.position - playerMovement.transform.position)).magnitude < 2f;
    }
	
	void Update ()
    {
	}

    void collide(Collider2D other)
    {
        if (transform.parent != null && other.name.Contains("Player"))
        {
            var leafModule = leafParticles.main;
            leafModule.simulationSpeed = 1f;

            Transform leafParent = transform.parent;
            transform.parent = null;
            if (leafParent.childCount == 0)
            {
                playerMovement.victory();
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        collide(other);
    }

    void OnTriggerStay2D(Collider2D other)
    {
        collide(other);
    }
}
