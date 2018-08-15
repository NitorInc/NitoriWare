using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SekibankiNeckWheelCode : MonoBehaviour
{

    
    private float Sekibanki3RightDeterminer = 5f;

    private void Start()
    {
        Sekibanki3RightDeterminer = Random.Range(0f, 10f);
        if (Sekibanki3RightDeterminer >= 6f)
        {
            
            
            foreach (Transform child in transform)
                        {
                child.BroadcastMessage("SekibankiNeck3ChangeDirection");
                        }

        }
        else
        {
            
        }
    }
}


