using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SagumeLie_SagumePosition : MonoBehaviour {

    [SerializeField]
    private bool neutral;

    [SerializeField]
    private bool positive;

    [SerializeField]
    private bool negative;
    [SerializeField]
    private Vector3 offset;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if ((neutral && !MicrogameController.instance.getVictoryDetermined()) 
            || (positive && MicrogameController.instance.getVictory() && MicrogameController.instance.getVictoryDetermined()) 
            || (negative && !MicrogameController.instance.getVictory() && MicrogameController.instance.getVictoryDetermined()))
        {
            transform.position = new Vector3(4.5f, -1f, transform.position.z)
                + offset;
        }
        else
        {
            transform.position = new Vector3(15f, -0.2f, transform.position.z);
        }
    }
}
