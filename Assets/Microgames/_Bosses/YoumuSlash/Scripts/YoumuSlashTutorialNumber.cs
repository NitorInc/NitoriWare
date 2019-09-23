using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YoumuSlashTutorialNumber : MonoBehaviour
{
    private const string ParamName = "Pulse";

    [SerializeField]
    YoumuSlashTimingData timingData;
    [SerializeField]
    private YoumuSlashBeatMap.TargetBeat.Direction direction;
    [SerializeField]
    private Sprite[] stageSprites;
    [SerializeField]
    private YoumuSlashTutorialNumber otherSideNumber;

    private Animator animator;
    private SpriteRenderer spriteRenderer;
    //private bool skipNextBeat;
    private int MaxStage => stageSprites.Length - 1;
    private const int maxNaturalStage = 2;

    private float lastTriggeredBeat = -5f;
    public float LastTriggeredBeat => lastTriggeredBeat;
    private float lastIncrementedBeat = -5f;

    int stage = 0;
    public int getStage() => stage;
    public void setStage(int value)
    {
        stage = Mathf.Min(value, MaxStage);
        animator.ResetTrigger(ParamName);
        animator.SetTrigger(ParamName);
        spriteRenderer.sprite = stageSprites[stage];
    }
    
    void Start ()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        YoumuSlashTargetSpawner.OnTargetLaunch += onTargetLaunch;
	}

    void onTargetLaunch(YoumuSlashBeatMap.TargetBeat target)
    {
        if (!gameObject.activeInHierarchy || target.HitDirection != direction)
            return;

        var beat = target.LaunchBeat;

        CancelInvoke();

        if (otherSideNumber.getStage() > 0 && beat - otherSideNumber.LastTriggeredBeat <= 1.01f)  //If other side is not on stage 0, disable that and set this to next stage
        {
            setStage(otherSideNumber.getStage() + 1);
            otherSideNumber.setStage(0);
            otherSideNumber.CancelInvoke();
        }
        else if (beat - LastTriggeredBeat <= 1.01f)
        {
            setStage(stage + 1);   //Trigger next stage by default, max is handled naturally
        }
        else
            setStage(1);

        lastTriggeredBeat = lastIncrementedBeat = beat;
        invokeNextIncrement();
    }

    //Called after assigning stage when not on beat
    //void handleOffbeat()
    //{
    //    skipNextBeat = true;
    //    if (stage > 0)
    //        Invoke("offBeatIncrement", timingData.BeatDuration);
    //}

    //void offBeatIncrement()
    //{
    //    if (stage < maxNaturalStage)
    //        setStage(stage + 1);
    //    else
    //        setStage(0);
    //    handleOffbeat();
    //}

    void increment()
    {
        if (!gameObject.activeInHierarchy)
            return;
        lastIncrementedBeat++;
        if (timingData.BeatMap.getNextLaunchingTarget(lastIncrementedBeat).LaunchBeat <= lastIncrementedBeat)   //Don't increment if a new target is set to launch this beat
            return;

        //Increment or reset stage normally
        if (stage >= maxNaturalStage)
            setStage(0);
        else if (stage > 0)
        {
            setStage(stage + 1);
            invokeNextIncrement();
        }

    }

    void invokeNextIncrement()
    {
        Invoke("increment", timingData.BeatDuration);
    }
}
