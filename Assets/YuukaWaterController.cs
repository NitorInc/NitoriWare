using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YuukaWaterController : MonoBehaviour {

    public int requiredCompletion = 3;
    int completionCounter = 0;

    public delegate void VictoryAction();
    public static event VictoryAction OnVictory;

	public void Notify() {
        completionCounter++;
        if (completionCounter >= requiredCompletion) {
            MicrogameController.instance.setVictory(true, true);
            OnVictory();
        }    
    }

    private void OnDestroy() {
        OnVictory = null;
    }
}
