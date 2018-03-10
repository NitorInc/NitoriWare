using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodCutController : MonoBehaviour {

    [Header("How fast the knife moves")]
    [SerializeField]
    private float speed = 1f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //Handle Movement
        transform.Translate(Input.GetAxisRaw("Horizontal") * Time.deltaTime * speed, 0f, 0f);
	}
}
