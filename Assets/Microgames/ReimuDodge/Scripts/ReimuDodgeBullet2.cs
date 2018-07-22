using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReimuDodgeBullet2 : MonoBehaviour
{
    //values that will be set in the Inspector
    public Transform Target;

    //values for internal use
    private Quaternion _lookRotation;
    private Vector2 _direction;
    Vector3 vectorToTarget;
    float angle;
    Quaternion q;



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
        Invoke("SetTrajectory", delay);
        vectorToTarget = target.transform.position - transform.position;
        angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg - 90;
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

    void SetTrajectory()
    {
        // Calculate a trajectory towards the target
        trajectory = (target.transform.position - transform.position).normalized;
    }
}