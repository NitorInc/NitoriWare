using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReimuDodgeBullet : MonoBehaviour
{
    //A Unity in-editor variable
    [Header("the thing to fly towards")]
    [SerializeField]
    private GameObject target;

    [Header("How fast the bullet goes")]
    [SerializeField]
    private float speed = 1f;

    [Header("Firing delay in seconds")]
    [SerializeField]
    private float delay = 1f;

    //Stores the direction of travel for the bullet
    private Vector2 trajectory;

	// Use this for initialization
	void Start ()
    {
        // Invoke the setTrajectory method after the delay
        Invoke("SetTrajectory", delay);
	}
	
	// Update is called once per frame
	void Update ()
    {
		// Only start moving after the trajectory has been set
        if (trajectory != null)
        {
            // Move the bullet a certain distance based on trajectory speed and time
            Vector2 newPosition = (Vector2)transform.position + (trajectory * Time.deltaTime);
            transform.position = newPosition;
        }    
	}
    void SetTrajectory()
    {
        // Calculate a trajectory towards the target
        trajectory = (target.transform.position - transform.position).normalized;
    }
}
