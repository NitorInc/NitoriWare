using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReimuDodgeBullet : MonoBehaviour
{


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

    [Header("Manually set direction of travel (Normalized)")]
    [SerializeField]
    private Vector3 startingTrajectory;

    private Vector3 trajectory;

    // Use this for initialization
    void Start()
    {
		// Invoke the setTrajectory method after the delay
		Invoke("SetTrajectory", delay);
    }

    // Update is called once per frame
    void Update()
    {
		// Move the bullet a certain distance based on trajectory speed and time
		transform.position = transform.position + (trajectory * speed * Time.deltaTime);
    }

    void SetTrajectory()
    {
		if (target != null) {
			// Calculate a trajectory towards the target
			trajectory = (target.transform.position - transform.position).normalized;
		} else {
			trajectory = startingTrajectory.normalized;
		}
    }
}
