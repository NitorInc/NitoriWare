using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RazorController : MonoBehaviour {

    private float speed = 120f;
    private bool has_moved = false;
    // Use this for initialization
    void Start () {
    	
    }
    
    // Update is called once per frame
    void Update () {
        int direction = 0;
        if (Input.GetKey(KeyCode.LeftArrow)) direction = -1;
        else if (Input.GetKey(KeyCode.RightArrow)) direction = 1;

        transform.Rotate(0f, 0f, direction * Time.deltaTime * speed);
    }
}
