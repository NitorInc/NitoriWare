using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RabbitTrapPauseTrigger : MonoBehaviour {

    [SerializeField]
    private float pauseTime = 0;

    private bool hasTriggered = false;
    
    public float PauseTime
    {
        get
        {
            return pauseTime;
        }

        set
        {
            pauseTime = value;
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
