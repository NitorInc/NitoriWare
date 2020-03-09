using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkRoomEffectAnimationController : MonoBehaviour
{
    public static DarkRoomEffectAnimationController instance;
    
    public float lampBoost = 0f;
    public float cursorBoost = 0f;
    public float radiusBoost = 0f;
    public float walkSpeed = 1f;

    private Animator animator;
    
	void Awake ()
    {
        instance = this;
        animator = GetComponent<Animator>();
	}
	
	void Update ()
    {
        animator.SetFloat("WalkSpeed",walkSpeed);
	}
}
