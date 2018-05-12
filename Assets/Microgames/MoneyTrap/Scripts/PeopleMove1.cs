using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeopleMove1 : MonoBehaviour {

    //Controls movement for people objects

    [Header("How fast person moves")]
    [SerializeField]
    private float speed = 1f;

    [Header("Proximity threshold to follow")]
    [SerializeField]
    private float proximity = 1f;

    // Stores the direction of movement
    private Vector2 trajectory;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        GameObject player = GameObject.Find("Player");
        trajectory = (player.transform.position - transform.position).normalized;
        trajectory.y = 0f;

        if ( Mathf.Abs(transform.position.x- player.transform.position.x) < proximity)
        {
            Vector2 newPosition = (Vector2)transform.position + (trajectory * speed * Time.deltaTime);
            transform.position = newPosition;
        }
    }
}
