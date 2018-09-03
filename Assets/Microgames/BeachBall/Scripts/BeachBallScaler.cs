using System;
using UnityEngine;

/// <summary>
/// Scales until reaches DesiredScale
/// </summary>
public class BeachBallScaler : MonoBehaviour
{
    public float DesiredScale = 0.5f;
    public float Speed = 0.1f;
    public bool Started = false;

    private float ballThrowSpeed = 1f;

    void Start()
    {
        ballThrowSpeed = GameObject.Find("Ball")
            .GetComponent<BeachBallBallLauncher>().ThrowMultiplier;
    }

    void Update()
    {
        if (Started && Math.Abs(transform.localScale.x - DesiredScale) > 0.01f)
        {
            transform.localScale *= 1 - (Time.deltaTime * Speed * Mathf.Sqrt(ballThrowSpeed));
        }
    }
}
