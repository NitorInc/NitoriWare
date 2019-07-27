using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReimuDodgeBullet : MonoBehaviour
{

    [Header("The thing to fly towards")]
    [SerializeField]
    private GameObject target;

    [SerializeField]
    private float speed = 5f;

    [SerializeField]
    private float delay = 1f;

    private Vector2 trajectory;  // Stores the direction of travel for the bullet

    void Start()
    {
        Invoke("SetTrajectory", delay);  // Call SetTrajectory() after delay
    }

    void Update()
    {
        if (trajectory != null)  // Only when trajectory is set
        {
            // Move the bullet
            Vector2 newPosition = (Vector2)transform.position + (trajectory * speed * Time.deltaTime);
            transform.position = newPosition;
        }
    }

    void SetTrajectory()
    {
        // Calculate the trajectory towards the target;
        trajectory = (target.transform.position - transform.position).normalized;
    }
}
