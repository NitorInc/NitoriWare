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
    
    [Header("X Movement Distance")]
    [SerializeField]
    private float xMovement = 1f;   
    
    private bool downward = true;
    
    private float moveDistance = 0f;
    
	// Update Kaguya's position by moving her vertically according to moveSpeed, then adjusting her x value based on her height for diagonal movement.
	void Update () {
        float x = transform.position.x;
        float y = transform.position.y;
		if (downward == true)
        {
            if (transform.position.y >= minHeight)
                y = transform.position.y - moveSpeed * Time.deltaTime;
            else
                downward = false;
        }
        else
        {
            if (transform.position.y <= maxHeight)
                y = transform.position.y + moveSpeed * Time.deltaTime;
            else
                downward = true;
        }
        moveDistance = (minHeight * -1) + maxHeight;
        x = ((y - minHeight) / moveDistance) * xMovement;
        transform.position = new Vector2(-5.2f + x, y);
	}
}
