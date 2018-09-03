using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcePath_Camera : MonoBehaviour {
    float pi = Mathf.PI;

    [Header("Buncha customizables")]
    [SerializeField] private float speed;
    [SerializeField] private float magnitude;
    private float counter;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = magnitude * new Vector2(Mathf.Cos(counter), Mathf.Cos(counter));

        counter = (counter + speed/ 2 * pi) % 2 * pi;
    }
}
