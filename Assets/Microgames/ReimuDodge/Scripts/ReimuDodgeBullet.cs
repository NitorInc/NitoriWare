using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReimuDodgeBullet : MonoBehaviour {
    // A Unity in-editor variable
    [Header("The object to fly towards")]
    [SerializeField]  // Make the following block can be "complied" to the unity scene file ---- that means this variable can be modified with the inspector and saved in a scene.
    private GameObject target;  // Protect the vairable be accessed and modified by other scripts

    [Header("Speed of the bullet")]
    [SerializeField]
    private float speed = 1f;

    [Header("Firing delay in seconds")]
    [SerializeField]
    private float delay = 1f;

    // Stores the direction of travel for the bullet
    private Vector2 trajectory;

	// Use this for initialization
	void Start () {
        // Invoke the setTrajectory method after the delay
        Invoke("SetTragetory", delay);  // Alternative solution: coroutine
    }

    // Update is called once per frame
    void Update () {
		if (trajectory != null)
        {
            Vector2 newPosition = (Vector2)this.transform.position + (trajectory * speed * Time.deltaTime);
            this.transform.position = newPosition;
        }
	}

    void SetTragetory()
    {
        // Calculate the trajectory towards the target
        trajectory = (target.transform.position - this.transform.position).normalized;
        // Vector2.normalized: Returns this vector with a magnitude of 1 (Read Only).
    }
}
