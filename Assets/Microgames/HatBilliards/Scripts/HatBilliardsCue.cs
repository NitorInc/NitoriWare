using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HatBilliardsCue : MonoBehaviour
{
    [SerializeField]
    Transform target;

    void Update ()
    {
        this.transform.LookAt (target);
    }
}
