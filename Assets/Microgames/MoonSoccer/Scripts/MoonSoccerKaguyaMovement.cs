using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonSoccerKaguyaMovement : MonoBehaviour {
    
    [Header("Movement Speed")]
    [SerializeField]
    private float moveSpeed = 1f;

    [Header("Minimum Height")]
    [SerializeField]
    private float minHeight = 1f;
    
    [Header("Maximum Height")]
    [SerializeField]
    private float maxHeight = 1f;
    
    private bool downward = true;

	// Update is called once per frame
	void Update () {
		if (downward == true)
        {
            if (transform.position.y >= minHeight)
                transform.position = new Vector2(transform.position.x, transform.position.y - moveSpeed * Time.deltaTime);
            else
                downward = false;
        }
        else
        {
            if (transform.position.y <= maxHeight)
                transform.position = new Vector2(transform.position.x, transform.position.y + moveSpeed * Time.deltaTime);
            else
                downward = true;
        }
	}
}
