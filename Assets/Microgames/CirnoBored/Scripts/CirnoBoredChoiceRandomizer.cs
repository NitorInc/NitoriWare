using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CirnoBoredChoiceRandomizer : MonoBehaviour
{
    
	void Start ()
    {
        //Get X positions
        float[] childXs = new float[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            childXs[i] = transform.GetChild(i).position.x;
        }
        
        //Shuffle
        for (int i = 0; i < childXs.Length - 1; i++)
        {
            int choice = Random.Range(i, childXs.Length);
            if (choice != i)
            {
                var hold = childXs[i];
                childXs[i] = childXs[choice];
                childXs[choice] = hold;
            }
        }

        //Reapply X positions
        for (int i = 0; i < childXs.Length; i++)
        {
            var child = transform.GetChild(i);
            child.position = new Vector3(childXs[i], child.position.y, child.position.z);
        }
    }
}
