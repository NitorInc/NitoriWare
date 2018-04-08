using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeineMath_Term : MonoBehaviour {

    public void setValue(int value)
    {
        //Add new instances of the object to match the value
        //NOTE: Should not be called more than once
        for (int i = 1; i < value; i++)
        {
            float newx = transform.position.x - (0.6f * i) - (0.25f * Mathf.Floor(i / 5)); //Every fifth icon, add a longer space
            float newy = transform.position.y;
            Vector3 newposition = new Vector3(newx, newy, 0);
            Object.Instantiate(this, newposition, Quaternion.identity);
        }
    }

}
