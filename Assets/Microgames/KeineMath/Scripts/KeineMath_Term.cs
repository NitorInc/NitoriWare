using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeineMath_Term : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void setValue(int value)
    {
        for (int i = 1; i < value; i++)
        {
            float newx = transform.position.x - (1f * i);
            float newy = transform.position.y;
            Vector3 newposition = new Vector3(newx, newy, 0);
            Object.Instantiate(this, newposition, Quaternion.identity);
        }
    }
}
