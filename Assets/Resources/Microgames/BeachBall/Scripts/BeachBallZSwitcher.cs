using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeachBallZSwitcher : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Switch()
    {
        transform.position += new Vector3(0, 0, 2);
    }
    public void Revert()
    {
        transform.position -= 2* new Vector3(0, 0, 2);
    }
}
