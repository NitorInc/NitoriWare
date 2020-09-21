using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CommandDisplay : MonoBehaviour
{
    [SerializeField]
    private MicrogamePlayer microgamePlayer;
    [SerializeField]
    private TextMeshPro textComponent;
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
        animator.SetTrigger("play");
    }

    public void play(Microgame.Session   session, string command, AnimatorOverrideController animationOverride)
        => play(command, animationOverride);

    public void play ()
    {
        AnimatorOverrideController controller = null;
        play(controller);
    }

    public void play(AnimatorOverrideController animationOverride)
    {
        play(getText(), animationOverride);
    }

    public void PlayCurrentMicrogameCommand()
    {
        var currentSession = microgamePlayer.CurrentMicrogameSession;
        play(currentSession.GetLocalizedCommand(), currentSession.GetCommandAnimatorOverride());
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
