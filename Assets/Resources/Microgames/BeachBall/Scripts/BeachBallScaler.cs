using System;
using UnityEngine;

public class BeachBallScaler : MonoBehaviour
{
    public float DesiredScale = 0.5f;
    public float Speed = 0.1f;
    public bool Started = false;

    void Start()
    {

    }

    void Update()
    {
        if (Started && Math.Abs(transform.localScale.x - DesiredScale) > 0.01f)
        {
            transform.localScale *= 1 - (Time.deltaTime  * Speed);
        }
    }
}
