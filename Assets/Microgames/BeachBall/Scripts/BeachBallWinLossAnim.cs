using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeachBallWinLossAnim : MonoBehaviour
{
    public AnimationClip WinAnimation;
    public AnimationClip LossAnimation;

    private Animation anim;

    void Start()
    {
        anim = GetComponent<Animation>();

        var eventWrapper = GameObject.Find(BeachBallWinLossEventWrapper.DefaultGameObjectName)
            .GetComponent<BeachBallWinLossEventWrapper>();

        //Add event listeners
        eventWrapper.OnWin += (sender, args) =>
        {
            anim.clip = WinAnimation;
            anim.Play();
        };
        eventWrapper.OnLoss += (sender, args) =>
        {
            anim.clip = LossAnimation;
            anim.Play();
        };
    }

    void Update()
    {

    }
}
