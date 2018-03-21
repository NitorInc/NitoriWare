using UnityEngine;

/// <summary>
/// Triggers fail if y velocity value is too low
/// </summary>
public class BeachBallFailVelocityObserver : MonoBehaviour
{

    public float velocityThreshold = 1f;

    private Rigidbody2D rigidBody;

    private float ballThrowSpeed;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        ballThrowSpeed = GameObject.Find("Ball")
            .GetComponent<BeachBallBallLauncher>().ThrowMultiplier;
    }

    void Update()
    {
        if (rigidBody.velocity.y < -velocityThreshold * ballThrowSpeed
            * (1 / Time.timeScale))
            MicrogameController.instance.setVictory(false, true);
    }
}
