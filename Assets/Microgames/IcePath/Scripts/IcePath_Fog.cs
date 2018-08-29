using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcePath_Fog : MonoBehaviour {

    [Header("Speed")]
    [SerializeField]
    private float speed;

	// Use this for initialization
	void Start () {
        // Set direction
        if (transform.position.x > 0) {
            speed *= -1;
        }
		
	}
	
	// Update is called once per frame
	void Update () {
        // Move
        Vector2 newPosition = (Vector2)transform.position + new Vector2(speed * Time.deltaTime, 0);
        transform.position = newPosition;
        
	}
}
