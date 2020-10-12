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
    [SerializeField]
    private CommandDisplayLerpY yPositionLerpComponent;
    
    private RuntimeAnimatorController initialAnimatorController;

    void Awake()
    {
        initialAnimatorController = animator.runtimeAnimatorController;
    }

    public void PlayCommand(Microgame.Session session, string text, MicrogameCommandSettings commandSettings)
    {
        textComponent.text = text;
        animator.runtimeAnimatorController = commandSettings.AnimatorOverride == null
            ? initialAnimatorController : commandSettings.AnimatorOverride;
        if (yPositionLerpComponent != null)
            yPositionLerpComponent.RestYPosition = commandSettings.restYPosition;
        animator.SetTrigger("play");
    }
    public void PlayCurrentMicrogameCommand()
    {
        var currentSession = microgamePlayer.CurrentMicrogameSession;
        PlayCommand(currentSession, currentSession.GetLocalizedCommand(), currentSession.GetCommandSettings());
    }

    public string getText()
    {
        return textComponent.text;
    }

}
