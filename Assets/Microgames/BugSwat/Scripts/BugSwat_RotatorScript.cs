using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BugSwat_RotatorScript : MonoBehaviour {

    private float rotationSpeed;
    private Vector3 rotateVector;

	// Use this for initialization
	void Start () {
        float randomAngle = Random.Range(0, 360);
        rotationSpeed = Random.Range(2, 7);
        float randomX = Random.Range(-4, 4);
        float randomY = Random.Range(-3, 3);

        rotateVector = new Vector3(0, 0, randomAngle);
        transform.Rotate(rotateVector);

        transform.position = new Vector3(randomX, randomY, 0);

        rotateVector = new Vector3(0, 0, rotationSpeed);
    }
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(rotateVector);
    }
}
