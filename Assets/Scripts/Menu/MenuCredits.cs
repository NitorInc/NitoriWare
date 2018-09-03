using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCredits : MonoBehaviour
{

#pragma warning disable 0649
    [SerializeField]
    private Animator animator;
#pragma warning restore 0649

	void Update()
	{
        animator.SetFloat("Speed", GameMenu.shifting ? 0f : 1f);
	}
}
