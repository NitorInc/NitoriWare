using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuukotoSweepPatch : MonoBehaviour
{
    public RuukotoSweep_Movement playerMovement;
    public ParticleSystem leafParticles;
    public int spawnCountMin, spawnCountMax;
    
	void Start ()
    {
        leafParticles.Emit(Random.Range(spawnCountMin, spawnCountMax));
        var leafModule = leafParticles.main;
        leafModule.simulationSpeed = 0f;
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
