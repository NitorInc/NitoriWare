using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BroomRaceBackgroundSpeed : MonoBehaviour
{
    public static BroomRaceBackgroundSpeed instance;

    [SerializeField]
    private float baseSpeed;
    public float BaseSpeed => baseSpeed;
    [SerializeField]
    private float speedMult;
    public float SpeedMult => speedMult;

    private void Awake()
    {
        instance = this;
    }
}
