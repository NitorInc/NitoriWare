using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BroomRaceRing : MonoBehaviour
{

    [SerializeField]
    private Animator rigAnimator;
    [SerializeField]
    private float ringActivateSpinSpeed = 2f;

    public void activate()
    {
        rigAnimator.SetTrigger("Activate");
        rigAnimator.SetFloat("SpinSpeed", ringActivateSpinSpeed);
    }

}
