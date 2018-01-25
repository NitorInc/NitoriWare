using UnityEngine;

/// <summary>
/// Triggers fail if y velocity value is too low
/// </summary>
public class BeachBallFailVelocityObserver : MonoBehaviour
{

    public float velocityTreshold = 1f;

    private Rigidbody2D rigidBody;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (rigidBody.velocity.y < -velocityTreshold
            * (1 / Time.timeScale))
            MicrogameController.instance.setVictory(false, true);
    }
}
