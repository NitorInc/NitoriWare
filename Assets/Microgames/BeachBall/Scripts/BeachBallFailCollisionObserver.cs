using UnityEngine;

/// <summary>
/// Observes ball params. Launches the ball backwards and triggers loss if requirements are met
/// </summary>
public class BeachBallFailCollisionObserver : BeachBallCollisionObserver
{
    private BeachBallCollisionObserver beachBallCollisionObserver;

    public float velocityTreshold = 0.5f;

    [Header("Backwards launch params")]
    public float torqueRange = 50f;
    public float forceRange = 300f;

    protected override void Start()
    {
        base.Start();
        beachBallCollisionObserver = GameObject.Find("Hoop")
            .GetComponent<BeachBallCollisionObserver>();
    }
    public override void OnTriggerStay2D(Collider2D other)
    {
        if (!fired && !beachBallCollisionObserver.Fired &&
            ballPhysics.velocity.y < -velocityTreshold
            * (1 / Time.timeScale) && other == ballCollider)
        {
            fired = true;
            other.gameObject.GetComponent<BeachBallZSwitcher>().Revert();

            var downscaler = other.gameObject.GetComponent<BeachBallScaler>();
            downscaler.DesiredScale = 2f;
            downscaler.Speed *= -2;

            var rigidBody = other.gameObject.GetComponent<Rigidbody2D>();
            rigidBody.AddForce(new Vector2(Random.Range(-forceRange, forceRange),
                Random.Range(-forceRange, forceRange)));
            rigidBody.AddTorque(Random.Range(-torqueRange, torqueRange));

            MicrogameController.instance.setVictory(victory: false, final: true);
        }
    }
}
