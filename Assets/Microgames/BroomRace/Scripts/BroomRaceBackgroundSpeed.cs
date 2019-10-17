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
    private int speedMult;
    public int SpeedMult => speedMult;

    private void Awake()
    {
        instance = this;
    }
}
