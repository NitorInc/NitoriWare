using UnityEngine;

/// <summary>
/// Randomizes hoop animation
/// </summary>
public class BeachBallHoopParamsRandomizer : MonoBehaviour
{
    [Header("Insert animation name here")]
    public string AnimationName;
    void Start()
    {
        var animation = GetComponent<Animation>()[AnimationName];
        if (Random.Range(0f, 1f) < 0.5f)
        {
            animation.time = animation.length + Random.Range(0.2f, 0.2f);
            animation.speed = 1;
        }
        else
        {
            animation.speed = -1;
            animation.time = animation.length / 2 + Random.Range(-0.2f, 0.2f);
        }
    }

    void Update()
    {

    }
}
