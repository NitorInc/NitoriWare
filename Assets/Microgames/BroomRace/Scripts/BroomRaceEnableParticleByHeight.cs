using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BroomRaceEnableParticleByHeight : MonoBehaviour
{
    [SerializeField]
    private GameObject aboveParticle;
    [SerializeField]
    private GameObject belowParticle;
    [SerializeField]
    private float yThreshold;

    private void Awake()
    {
        if (transform.position.y >= yThreshold)
            aboveParticle.SetActive(true);
        else
            belowParticle.SetActive(true);
    }

}
