using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReimuDodgeBullet3: MonoBehaviour
{
    //values that will be set in the Inspector
    public Transform Target;

    //values for internal use
    private Quaternion _lookRotation;
    private Vector2 _direction;
    private Vector3 vectorToTarget;
    private float angle;
    private float collisions;
    private Quaternion q;



    // A Unity in-editor variable
    [Header("The thing to fly towards")]
    [SerializeField]
    private GameObject target;

    [Header("How fast the bullet goes")]
    [SerializeField]
    private float speed = 1f;

    [Header("Firing delay in seconds")]
    [SerializeField]
    private float delay = 1f;

    // Stores the direction of travel for the bullet
    private Vector2 trajectory;

    // Use this for initialization
    void Start()
    {
        // Invoke the setTrajectory method after the delay
        Invoke("SetFirstTrajectory", delay);
        vectorToTarget = target.transform.position - transform.position;
        angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg - 90;
        collisions = 0;
        q = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    // Update is called once per frame
    void Update()
    {
        // Only start moving after the trajectory has been set
        if (trajectory != null)
        {
            // Move the bullet a certain distance based on trajectory speed and time
            Vector2 newPosition = (Vector2)transform.position + (trajectory * speed * Time.deltaTime);
            transform.position = newPosition;

            transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime);
        }
    }

    // This will happen when the player's hitbox collides with a bullet
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == ("MicrogameTag1"))
        {
            collisions++;
            if(collisions>1)
            {
                SetTrajectory(other);
                vectorToTarget = target.transform.position - transform.position;
                angle += 90;
                q = Quaternion.AngleAxis(angle, Vector3.forward);
            }
        }
    }

    void SetFirstTrajectory()
    {
        // Calculate a trajectory towards the target
        trajectory = (target.transform.position - transform.position).normalized;
    }

    void SetTrajectory(Collider2D wall)
    {
        // Calculate a trajectory bouncing off something
        if ( wall.name.Contains("East") || wall.name.Contains("West"))
        {
            trajectory = new Vector2(-1 * trajectory.x, trajectory.y);
        }
        if (wall.name.Contains("North") || wall.name.Contains("South"))
        {
            trajectory = new Vector2(trajectory.x, -1 * trajectory.y);
        }
        speed += 2;
    }
}