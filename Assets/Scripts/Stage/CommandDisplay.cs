using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommandDisplay : MonoBehaviour
{
    
    [SerializeField]
    private Text textComponent;
    [SerializeField]
    private Animator animator;
    
    private RuntimeAnimatorController initialAnimatorController;

    void Awake()
    {
        initialAnimatorController = animator.runtimeAnimatorController;
    }

    public void play(string command, AnimatorOverrideController animationOverride = null)
    {
        setText(command, animationOverride);
        animator.SetBool("play", true);
    }

    public void play(AnimatorOverrideController animationOverride = null)
    {
        play(getText(), animationOverride);
    }

    public string getText()
    {
        return textComponent.text;
    }

    public void setText(string text, AnimatorOverrideController animationOverride = null)
    {
        textComponent.text = text;
        animator.runtimeAnimatorController = animationOverride == null ? initialAnimatorController : animationOverride;
    }
}
