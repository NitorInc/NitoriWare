using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperThiefCucumber : MonoBehaviour
{

#pragma warning disable 0649
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private Vector3 holdOffset;
    [SerializeField]
    private SineWave sineWave;
    [SerializeField]
    private ParticleSystem[] particleSystems;
#pragma warning restore 0649

    private Vector3 goalPosition;

    void Update()
    {
        if (PaperThiefNitori.dead)
        {
            for (int i = 0; i < particleSystems.Length; i++)
            {
                var module = particleSystems[i].main;
                module.simulationSpeed = 0f;
            }

            sineWave.enabled = false;
            enabled = false;
        }
        if (goalPosition != Vector3.zero)
        {
            goalPosition = PaperThiefNitori.instance.transform.position + holdOffset;
            if (transform.moveTowards(goalPosition, moveSpeed))
                goalPosition = Vector3.zero;
        }
    }

    public void collect()
    {
        sineWave.enabled = false;
        goalPosition = PaperThiefNitori.instance.transform.position + holdOffset;

    }
}
