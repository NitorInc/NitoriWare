using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HatBilliardsHat : MonoBehaviour
{
    Animator animator;

    void Start ()
    {
        animator = GetComponent<Animator> ();
    }

    public void PingAway ()
    {
        animator.SetBool ("ping", true);
    }
}
