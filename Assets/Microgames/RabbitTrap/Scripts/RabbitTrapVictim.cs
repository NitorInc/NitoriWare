using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RabbitTrapVictim : MonoBehaviour {

    [Header("Travel speed")]
    [SerializeField]
    private float speed;

    private Vector2 trajectory;

    private float maxXPosition;
    private Vector2 maxPosition;
    private bool stopMovement;

    // Use this for initialization
    void Start () {
        maxXPosition = -8;
        Vector2 maxPosition = new Vector2(maxXPosition, 0);
        trajectory = new Vector2(speed,0);
        stopMovement = false;        
}
	
	// Update is called once per frame
	void Update () {

        if (!stopMovement)
        {
            Vector2 newPosition = GetNewPosition();
            this.transform.position = newPosition;

            stopMovement = IsStopMovement();
        }
        
    }

    Vector2 GetNewPosition()
    {
        return (Vector2)transform.position + (trajectory * Time.deltaTime * -speed);
    }

    bool IsStopMovement()
    {
        if (this.transform.position.x<maxXPosition)
        {
            return true;
        } else
        {
            return false;
        }
    }


}
