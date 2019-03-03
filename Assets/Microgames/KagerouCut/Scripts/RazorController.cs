using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RazorController : MonoBehaviour {
    [SerializeField]
    private float speed = 180f;
    private bool has_moved = false;
    // Use this for initialization
    void Start () {
    	
    }
    
    // Update is called once per frame
    void Update () {
        int direction = 0;
        if (Input.GetKey(KeyCode.LeftArrow) &&
                transform.rotation.z > -Mathf.PI/4) {
            direction = -1;
        } else if (Input.GetKey(KeyCode.RightArrow) &&
                transform.rotation.z < Mathf.PI/4) { 
            direction = 1;
        }

        transform.Rotate(0f, 0f, direction * Time.deltaTime * speed);
    }
}
