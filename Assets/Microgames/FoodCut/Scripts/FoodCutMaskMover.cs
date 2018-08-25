using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodCutMaskMover : MonoBehaviour {
    [Header("Put distance of mask from center here:")]
    [SerializeField]
    private float distance = 0f;

    public GameObject line;
    public GameObject knife;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space) && line != null)
        {
            transform.position = new Vector2((knife.transform.position.x + distance), transform.position.y);
        }
    }

}
