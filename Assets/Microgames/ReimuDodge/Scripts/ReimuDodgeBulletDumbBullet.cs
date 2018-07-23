using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReimuDodgeBulletDumbBullet: MonoBehaviour
{
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
        Invoke("SetTrajectory", delay + 0.1f);
        
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
        }
    }

    void RandomizeSpeed()
    {
        print("help");
        speed = Random.Range(-1f, -6f);
    }

    void SetTrajectory()
    {
        // Get a trajectory towards the target
        trajectory = this.transform.parent.GetComponent<ReimuDodgeBulletBigBullet>().trajectory ;
    }
}