using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BroomRaceRing : MonoBehaviour
{

    [SerializeField]
    private Animator rigAnimator;

    public void activate()
    {
        rigAnimator.SetTrigger("Activate");
    }

}
