using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReimuDodgeBullet : MonoBehaviour
{

    [Header("The thing to fly towards")]
    [SerializeField]
    private GameObject target;

    [Header("Firing delay in seconds")]
    [SerializeField]
    private float delay = 1f;

    [Header("Use constant speed?")]
    [SerializeField]
    private bool isSpeedConstant;

    [Header("Constant speed")]
    [SerializeField]
    private float constantSpeed;

    [Header("If variable speed: acceleration value")]
    [SerializeField]
    private float acceleration;

    [Header("If variable speed: max speed")]
    [SerializeField]
    private float maxSpeed;

    private Vector2 trajectory;

    private float speedMod;

    private bool trajectorySet = false;

    bool hasHit = false;

    // Use this for initialization
    void Start()
    {
        Invoke("SetTrajectoryTowardsTarget", delay);
    }

    // Update is called once per frame
    void Update()
    {
        if (this.trajectory != null)
        {
            if (trajectorySet)
            {
                Vector2 newPosition = GetNewPosition();
                transform.position = newPosition;
                if (!hasHit) {
                    this.trajectory = GetTrajectoryTowardsTarget();
                }
                
            }
            
        }

    }

    void SetTrajectoryTowardsTarget()
    {
        trajectory = (target.transform.position - transform.position).normalized;
        trajectorySet = true;
    }

    Vector2 GetTrajectoryTowardsTarget()
    {
        return (target.transform.position - this.transform.position).normalized;
    }

    Vector2 GetNewPosition()
    {

        if (isSpeedConstant)
        {
            this.speedMod = this.constantSpeed;
        }
        else
        {
            if (this.speedMod < this.maxSpeed)
            {
                this.speedMod = this.speedMod + this.acceleration;
            }
        }

        return (Vector2)transform.position + (trajectory * Time.deltaTime * speedMod);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        this.hasHit = true;
    }
}
