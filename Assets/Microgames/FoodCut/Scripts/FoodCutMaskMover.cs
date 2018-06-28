using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodCutMaskMover : MonoBehaviour {
    [Header("Put distance of mask from center here:")]
    [SerializeField]
    private float distance = 0f;

    public GameObject line;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        if (line != null)
        {
            transform.position = new Vector2((line.transform.position.x + distance), transform.position.y);
        }
    }
}
