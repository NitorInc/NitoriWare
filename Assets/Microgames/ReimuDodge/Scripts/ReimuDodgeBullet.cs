using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReimuDodgeBullet : MonoBehaviour {
    [Header("This thing to fly towards")]
    [SerializeField]
    private GameObject target;

    [Header("How fast the bullet goes")]
    [SerializeField]
    private float speed = 1f;

    [Header("Firing delay in seconds")]
    [SerializeField]
    private float delay = 1f;

    private Vector2 trajectory;

	void Start () {
        Invoke("SetTrajectory", delay);
	}

	void Update () {
		if (trajectory != null) {
            Vector2 newPosition = (Vector2)transform.position + (trajectory * speed * Time.deltaTime);
            transform.position = newPosition;
        }
	}

    void SetTrajectory() {
        trajectory = (target.transform.position - transform.position).normalized;
    }
}
