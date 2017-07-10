using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{

#pragma warning disable 0649   //Serialized Fields
    [SerializeField]
    private Animator[] animators;
#pragma warning restore 0649

    [SerializeField]
    private bool _shifting;
    public bool shifting
    {
        get {return _shifting; }
        set { _shifting = value; }
    }
	
    public void shiftMenu(int menu)
    {
        foreach (Animator animator in animators)
        {
            animator.SetInteger("menu", menu);
        }
        _shifting = true;
    }
}
