using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DatingSimCharacterAnimation : MonoBehaviour
{
    private Animator rigAnimator;

    void Start()
    {
        rigAnimator = GetComponent<Animator>();
    }

    void onResult(bool victory)
    {
        rigAnimator.SetInteger("State", victory ? 1 : 2);
    }

}
