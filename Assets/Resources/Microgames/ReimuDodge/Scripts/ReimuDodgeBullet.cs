using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReimuDodgeBullet : MonoBehaviour {

    //declare changeable variables
    [Header("How fast the bullet goes")]
    [SerializeField]
    private float speed = 1f;

    [Header("Firing delay in seconds")]
    [SerializeField]
    private float delay = 1f;

    //declare private variables
    private Vector2 trajectory;
    private int currentFrames = 0;

    // Use this for initialization
    void Start () {
        // Invoke the setTrajectory method after the delay
        Invoke("SetTrajectory", delay);
    }
	
	// Update is called once per frame
	void Update () {
        if (trajectory != null)
        {
            // Move the bullet a certain distance based on trajectory speed and time
            Vector2 newPosition = (Vector2)transform.position + (trajectory * speed * Time.deltaTime);
            transform.position = newPosition;
        }

        if (currentFrames > 2147483646)
        {
            currentFrames = 0;
        }
        currentFrames++;
	}

    void SetTrajectory()
    {
        // Find the player object in the scene and calculate a trajectory towards them
        GameObject player = GameObject.Find("raymoo");
        trajectory = (player.transform.position - transform.position).normalized;
    }
}
