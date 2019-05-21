using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HatBilliardsCue : MonoBehaviour
{
    [SerializeField]
    Transform target;
    [SerializeField]
    Animator rigAnimator;

    private void Start()
    {
        HatBilliardsBall.onHit += onHit;
    }

    void onHit()
    {
        rigAnimator.SetTrigger("Hit");
    }

    void Update ()
    {
        this.transform.LookAt (target);
    }
}
