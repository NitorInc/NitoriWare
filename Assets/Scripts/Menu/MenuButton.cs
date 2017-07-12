using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour
{

#pragma warning disable 0649	//Serialized Fields
    [SerializeField]
    private Animator buttonAnimator;
#pragma warning restore 0649

	void Start()
	{
		
	}
	
	void LateUpdate()
	{
        buttonAnimator.SetBool("MouseHeld", Input.GetMouseButton(0));
        buttonAnimator.SetBool("MouseDown", Input.GetMouseButtonDown(0));
    }
}
