using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThoughtBubble : MonoBehaviour
{

	// Use this for initialization
	void Start ()
    {
        //choose cards
        int sumithought1 = Random.Range(3, 6);

        print(sumithought1);

        int sumithought2 = sumithought1 - 2;
        print(sumithought2);

        //draw cards
    }
	
	// Update is called once per frame
	void Update ()
    {

    }
}
