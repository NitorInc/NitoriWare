using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RabbitTrapVictim : MonoBehaviour {

    [Header("Travel speed")]
    [SerializeField]
    private float speed;

    private Vector2 trajectory;

    // Use this for initialization
    void Start () {
        trajectory = new Vector2(speed,0);
    }
	
	// Update is called once per frame
	void Update () {
        this.transform.position = GetNewPosition();
    }

    Vector2 GetNewPosition()
    {
        return (Vector2)transform.position + (trajectory * Time.deltaTime * -speed);
    }
}
