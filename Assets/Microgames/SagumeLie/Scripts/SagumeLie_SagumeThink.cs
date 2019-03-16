using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SagumeLie_SagumeThink : MonoBehaviour {

    [SerializeField]
    private GameObject arm;

    [SerializeField]
    private GameObject wing;

    private int frame = 0;
    private float currentRotation;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        frame++;
        currentRotation = Mathf.Sin((frame * 4 / 180f) * Mathf.PI);
        arm.transform.Rotate(new Vector3(0, 0, 1), currentRotation / 12.5f);
        wing.transform.Rotate(new Vector3(0, 0, 1), -currentRotation / 2f);
    }
}
