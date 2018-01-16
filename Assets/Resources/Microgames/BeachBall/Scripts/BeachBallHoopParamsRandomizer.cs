using UnityEngine;

public class BeachBallHoopParamsRandomizer : MonoBehaviour
{
    void Start()
    {
        var animation = GetComponent<Animation>()["hoop_move_lr"];
        if (Random.Range(0f, 1f) < 0.5f)
            animation.time = Random.Range(0f, animation.length / 4f);
        else
            animation.time = Random.Range(animation.length *3/ 4f, animation.length);
        if (Random.Range(0f, 1f) < 0.5f)
            animation.speed = -1;
    }

    void Update()
    {

    }
}
