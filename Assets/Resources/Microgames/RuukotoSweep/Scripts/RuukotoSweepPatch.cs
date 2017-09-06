using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuukotoSweepPatch : MonoBehaviour
{
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
        if (Input.GetKeyDown(KeyCode.Space))
            collide();
	}

    void collide()
    {
        var leafModule = leafParticles.main;
        leafModule.simulationSpeed = 1f;
    }
}
