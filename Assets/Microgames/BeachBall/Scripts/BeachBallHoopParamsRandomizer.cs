using UnityEngine;

/// <summary>
/// Randomizes hoop animation
/// </summary>
public class BeachBallHoopParamsRandomizer : MonoBehaviour
{
    [Header("Animation name")]
    public string AnimationName;
    [Header("Animation scale")]
    public float AnimationScale;
    void Start()
    {
        var animation = GetComponent<Animation>()[AnimationName];
        if (Random.Range(0f, 1f) < 0.5f)
        {
            animation.time = animation.length + Random.Range(0.2f, 0.2f);
            animation.speed = 1 * AnimationScale;
        }
        else
        {
            animation.speed = -1 * AnimationScale;
            animation.time = animation.length / 2 + Random.Range(-0.2f, 0.2f);
        }
    }

    void Update()
    {

    }
}
