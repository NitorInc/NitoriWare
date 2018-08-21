using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReimuDodgeBullet : MonoBehaviour {
    private float pi = Mathf.PI;
    
    // In-editor variables
    [Header("Bullet target")]
    [SerializeField]
    private GameObject target;

    [Header("Bullet distance")]
    [SerializeField]
    private float distance = 10f;

    [Header("Bullet speed")]
    [SerializeField]
    private float speed = 1f;

    [Header("Bullet delay")]
    [SerializeField]
    private float delay = 1f;
    

    private Vector2 direction;

    void Awake () {
        // Set starting position
        float random_angle = Random.Range(0, 2 * pi);

        transform.position = new Vector2(distance * Mathf.Cos(random_angle), distance * Mathf.Sin(random_angle));
    }

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
