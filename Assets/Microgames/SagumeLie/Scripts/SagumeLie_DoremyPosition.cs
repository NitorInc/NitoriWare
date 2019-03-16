using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SagumeLie_DoremyPosition : MonoBehaviour {

    [SerializeField]
    private bool neutral;

    [SerializeField]
    private bool positive;

    [SerializeField]
    private bool negative;

    // Use this for initialization
    void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
        if ((neutral && !MicrogameController.instance.getVictoryDetermined())
            || (positive && MicrogameController.instance.getVictory() && MicrogameController.instance.getVictoryDetermined())
            || (negative && !MicrogameController.instance.getVictory() && MicrogameController.instance.getVictoryDetermined()))
        {
            transform.position = new Vector3(-2f, 1f, transform.position.z);
        } else
        {
            transform.position = new Vector3(-10f, 1f, transform.position.z);
        }
	}
}
