using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RabbitTrapSpeedChangeTrigger : MonoBehaviour {

    [SerializeField]
    private float newSpeed = 0;

    private bool hasTriggered = false;

    public float NewSpeed
    {
        get
        {
            return newSpeed;
        }

        set
        {
            newSpeed = value;
        }
    }

    public bool HasTriggered
    {
        get
        {
            return hasTriggered;
        }

        set
        {
            hasTriggered = value;
        }
    }
}
