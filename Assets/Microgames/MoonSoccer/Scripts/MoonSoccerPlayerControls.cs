using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonSoccerPlayerControls : MonoBehaviour {
    
    // A Unity in-editor variable
    [Header("Movement Speed")]
    [SerializeField]
    private float moveSpeed = 1f;
    
    public MoonSoccerBall ballScript;
    

	// Update is called once per frame
	void Update () 
    {
        updateMovement();
        updateKick();
	}
    
    
    void updateMovement ()
    {
        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.position = new Vector2(transform.position.x, transform.position.y - moveSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.position = new Vector2(transform.position.x, transform.position.y + moveSpeed * Time.deltaTime);
        }
    }
    
    
    void updateKick ()
    {
     if (Input.GetKey(KeyCode.Space))
     {
         // TODO: Add kick animation
         print("Kick");
         ballScript.activate(transform.position);
     }
    }
}
