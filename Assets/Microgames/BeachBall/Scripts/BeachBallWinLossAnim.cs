using System;
using UnityEngine;

public class BeachBallWinLossAnim : MonoBehaviour
{
    public String WinTriggerName = "WinTrigger";
    public String LossTriggerName = "LossTrigger";

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();

        var eventWrapper = GameObject.Find(BeachBallWinLossEventWrapper.DefaultGameObjectName)
            .GetComponent<BeachBallWinLossEventWrapper>();

        //Add event listeners
        eventWrapper.OnWin += (sender, args) =>
        {
            animator.SetTrigger(WinTriggerName);
        };
        eventWrapper.OnLoss += (sender, args) =>
        {
            animator.SetTrigger(LossTriggerName);
        };
    }

    void Update()
    {

    }
}
