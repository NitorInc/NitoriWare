using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DatingSimCharacterAnimation : MonoBehaviour
{
    private Animator rigAnimator;

    void Start()
    {
        rigAnimator = GetComponent<Animator>();
        DatingSimOptionController.OnWinning += WinAnimation;
        DatingSimOptionController.OnLosing += LossAnimation;
    }

    void WinAnimation()
    {
        rigAnimator.SetInteger("State", 1);
    }

    void LossAnimation()
    {
        rigAnimator.SetInteger("State", 2);
    }

}
