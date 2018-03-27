using UnityEngine;

/// <summary>
/// Launches the ball backwards and triggers loss
/// if the ball changes velocity to negative while
/// inside the outer circle but outside the win capsule
/// </summary>
public class BeachBallFailCollisionObserver : BeachBallCollisionObserver
{
    private BeachBallCollisionObserver beachBallCollisionObserver;

    public float velocityThreshold = 0.5f;

    [Header("Backwards bounce params")]
    public float torqueRange = 50f;
    public float forceRange = 300f;

    public AudioClip bounceSound;

    private float ballThrowSpeed = 1f;

    protected override void Start()
    {
        base.Start();

        beachBallCollisionObserver = GameObject.Find("Hoop")
            .GetComponent<BeachBallCollisionObserver>();
        ballThrowSpeed = GameObject.Find("Ball")
            .GetComponent<BeachBallBallLauncher>().ThrowMultiplier;
    }
    public override void OnTriggerStay2D(Collider2D other)
    {
        if (!fired && !beachBallCollisionObserver.Fired &&
            ballPhysics.velocity.y < -velocityThreshold * ballThrowSpeed
            * (1 / Time.timeScale) && other == ballCollider)
        {
            fired = true;

            //Revert Z changes
            other.gameObject.GetComponent<BeachBallZSwitcher>().Revert();

            //Trigger upscaling
            var downscaler = other.gameObject.GetComponent<BeachBallScaler>();
            downscaler.DesiredScale = 2f;
            downscaler.Speed *= -2;

            //Add torque and force
            var rigidBody = other.gameObject.GetComponent<Rigidbody2D>();
            rigidBody.AddForce(Mathf.Sqrt(ballThrowSpeed) * new Vector2(Random.Range(-forceRange, forceRange),
                Random.Range(-forceRange, forceRange)));
            rigidBody.AddTorque(Random.Range(-torqueRange, torqueRange));

            MicrogameController.instance.setVictory(victory: false, final: true);

            MicrogameController.instance.playSFX(bounceSound, volume: 0.5f,
                panStereo: AudioHelper.getAudioPan(transform.position.x));
        }
    }
}
