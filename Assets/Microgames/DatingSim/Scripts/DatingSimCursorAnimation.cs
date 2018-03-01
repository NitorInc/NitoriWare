using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DatingSimCursorAnimation : MonoBehaviour
{

    Animator anim;
    
	void Start ()
    {
        anim = GetComponent<Animator>();
	}

    void onResult(bool victory)
    {
        if (!victory)
            anim.Play("Break", 0, 0.0f);
    }
}
