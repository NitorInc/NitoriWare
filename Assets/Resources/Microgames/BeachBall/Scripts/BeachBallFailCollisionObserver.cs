using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeachBallFailCollisionObserver : BeachBallCollisionObserver
{
    private BeachBallCollisionObserver beachBallCollisionObserver;
    protected override void Start()
    {
        base.Start();
        beachBallCollisionObserver = GameObject.Find("Hoop")
            .GetComponent<BeachBallCollisionObserver>();
    }
    public override void OnTriggerStay2D(Collider2D other)
    {
        if (!fired && !beachBallCollisionObserver.Fired && ballPhysics.velocity.y < -0.01 && other == ballCollider)
        {
            fired = true;
            other.gameObject.GetComponent<BeachBallZSwitcher>().Revert();

            var downscaler = other.gameObject.GetComponent<BeachBallScaler>();
            downscaler.DesiredScale = 2f;
            downscaler.Speed *= -2;

            var rigidBody = other.gameObject.GetComponent<Rigidbody2D>();
            rigidBody.AddForce(new Vector2(Random.Range(-300f, 300f), Random.Range(-300f, 300f)));
            rigidBody.AddTorque(Random.Range(-50f, 50f));

            MicrogameController.instance.setVictory(victory: false, final: true);
        }
    }
}
