using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodCutSideScript : MonoBehaviour {
    public bool inPosition = false;
    public float meatDistance = 9999;
    public GameObject meat;

    public int framesWithNoMeat = 0;
    // Use this for initialization
    void Start () {
		
	}

    public void OnTriggerStay2D(Collider2D collision)
    {
        //Give up two frames to ensure that there really is no more meat then assign the new meat
        if (collision.gameObject.name.Contains("MeatRoot") && framesWithNoMeat > 2)
        {
            meat = collision.gameObject;
            framesWithNoMeat = 0;
        }
    }

    void FixedUpdate()
    {
        if (meat != null)
        {
            //check distance away from meat
            float distanceCheck = transform.position.x - meat.transform.position.x;

            //if position away from meat is never assigned, assign the distance away from the meat
            if (meatDistance == 9999)
            {
                meatDistance = distanceCheck;
            }

            //move the line if it's not on its assigned distance away from the meat
            if (distanceCheck != meatDistance)
            {
                transform.position = new Vector3(meatDistance + meat.transform.position.x, transform.position.y, transform.position.z);
            }
            framesWithNoMeat = 0;
        }
        else
        {
            framesWithNoMeat += 1;
        }
    }
}
