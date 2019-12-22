using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyTrapTrapmedium : MonoBehaviour {

    //Controls movement for people objects

    [Header("Reference to target (Jewel)")]
    [SerializeField]
    private GameObject target;

    //Stores initial height
    private float floor;
    //var for late starting
    private float latestart = 1;

    // Use this for initialization
    void Start () {
        //Nothing to do, need target to be initialized
    }

    // Update is called once per frame
    void Update ()
    {
        if (latestart > 0)
            latestart--;
        else if (latestart == 0)
        {
            //easy difficulty - start in the half screen opposed to the player's cursor position
            Vector2 newpos = transform.position;
            if (target.transform.position.x < 0)
                newpos.x = -4.70f;
            else
                newpos.x = 4.70f;
            Debug.Log(target.transform.position.x + " " + newpos.x);
            transform.position = newpos;

            latestart--;
        }
    }
}
