using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KaguyaMemory_KaguyaEndAnimation : MonoBehaviour {

    public bool isWin = true;
    public bool isLose = true;
    public bool finishDetected = false;

    public static Animator anim;

    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        if (finishDetected == false)
        {
            if (isWin == true)
            {
                anim.SetBool("Win", true);
                finishDetected = true;
            }
            else if (isLose == true)
            {
                anim.SetBool("Lose", true);
                finishDetected = true;
            }
            
        }
        
    }
}
