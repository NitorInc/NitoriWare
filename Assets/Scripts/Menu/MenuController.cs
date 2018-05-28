using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{

#pragma warning disable 0649
    [SerializeField]
    private Animator[] animators;
#pragma warning restore 0649

    [SerializeField]
    private bool _transitioning;
    public bool transitioning
    {
        get {return _transitioning; }
        set { _transitioning = value; }
    }
	
    public void shiftMenu(int menu)
    {
        foreach (Animator animator in animators)
        {
            animator.SetInteger("menu", menu);
        }
        _transitioning = true;
    }
}
