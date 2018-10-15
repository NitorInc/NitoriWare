using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReimoDodgePlayer : MonoBehaviour {


    void OnTriggerEnter2D(Collider2D other)
    {
        print("Player was hit!");
    }
}
