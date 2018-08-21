using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReimuDodgeBullet : MonoBehaviour {

    // In-editor variables
    [Header("Bullet target")]
    [SerializeField]
    private GameObject target;

    [Header("Bullet speed")]
    [SerializeField]
    private float speed = 1f;

    [Header("Bullet delay")]
    [SerializeField]
    private float delay = 1f;

    // Bullet direction
    private Vector2 direction;

	// Use this for initialization
	void Start () {
        // Invoke SetDirection after delay
        Invoke("SetDirection", delay);
	}
	
	// Update is called once per frame
	void Update () {
		
        // Assign direction if set
        if (direction != null) {
            Vector2 newPosition = (Vector2)transform.position + (direction * speed * Time.deltaTime);
            transform.position = newPosition;
        }

	}

    void SetDirection () {
        // Set direction
        direction = (target.transform.position - transform.position).normalized;
    }
}
