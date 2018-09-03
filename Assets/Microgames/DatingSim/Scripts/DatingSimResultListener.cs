using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DatingSimResultListener : MonoBehaviour
{
	void Update ()
    {
        //Check for win or loss
        if (MicrogameController.instance.getVictoryDetermined())
        {
            SendMessage("onResult", MicrogameController.instance.getVictory());
            enabled = false;
        }
    }
}
