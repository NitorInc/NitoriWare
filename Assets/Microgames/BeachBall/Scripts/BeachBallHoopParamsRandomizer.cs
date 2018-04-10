using UnityEngine;

/// <summary>
/// Randomizes hoop animation
/// </summary>
public class BeachBallHoopParamsRandomizer : MonoBehaviour
{
    [Header("Animation name")]
    public string AnimationName;
    [Header("Animation scale")]
    public float AnimationScale = 1f;
    void Start()
    {
        var animation = GetComponent<Animation>()[AnimationName];
        animation.time = animation.length + Random.Range(0.2f, 0.2f);
        animation.speed = 1 * AnimationScale;

        if (Random.Range(0f, 1f) < 0.5f)
            transform.parent.localScale = new Vector3(
                -transform.parent.localScale.x, transform.parent.localScale.y, transform.parent.localScale.z);
    }

    void Update()
    {

    }
}
