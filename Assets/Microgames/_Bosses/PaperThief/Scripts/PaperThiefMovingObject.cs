using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperThiefMovingObject : MonoBehaviour
{

#pragma warning disable 0649
    [SerializeField]
    private SineWave sinewave;
    [SerializeField]
    private ParticleSystem[] particleSystems;
#pragma warning restore 0649

    void Update()
	{
        if (PaperThiefNitori.dead)
        {
            if (sinewave != null)
                sinewave.enabled = false;

            for (int i = 0; i < particleSystems.Length; i++)
            {
                var module = particleSystems[i].main;
                module.simulationSpeed = 0f;
            }

            enabled = false;
        }
	}
}
