using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KagerouCutMain : MonoBehaviour {

    [SerializeField]
    public GameObject[] furballs;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		bool ballsLeft = false;
        foreach (GameObject ball in furballs){
            if (ball != null){
                ballsLeft = true;
                break;
            }
        }
        if (!ballsLeft) {
            print("Winner!");
            MicrogameController.instance.setVictory(victory: true, final: true);
        }
	}
}
