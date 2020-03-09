using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RabbitTrapTrap : MonoBehaviour {

    //Startposition:
    //Position: X = 1, Y=-1.8
    //Scale:    X = 2, Y = 2

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            ActivateTrap();
        }
    }

    void ActivateTrap()
    {
        this.transform.Find("TrapSprite").GetComponent<SpriteRenderer>().enabled = false;
        // this.transform.localScale = new Vector3(0, 0, 0);
    }
}
