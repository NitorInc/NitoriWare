using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RazorController : MonoBehaviour {
    [SerializeField]
    private float speed = 180f;
    [SerializeField]
    private float maxAngle = 60f;

    private bool has_moved = false;
    // Use this for initialization
    void Start () {
    	
    }
    
    // Update is called once per frame
    void Update () {
        int direction = 0;
        var angle = transform.eulerAngles.z;
        while (angle > 180f)
            angle -= 360f;
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            if (angle > -maxAngle)
                direction = -1;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            if (angle < maxAngle)
                direction = 1;
        }

        transform.Rotate(0f, 0f, direction * Time.deltaTime * speed);
    }
}
