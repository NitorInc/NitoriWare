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
    private bool skipNextBeat;
    private int MaxStage => stageSprites.Length - 1;
    private const int maxNaturalStage = 2;

    int stage = 0;
    public int getStage() => stage;
    public void setStage(int value)
    {
        stage = Mathf.Min(value, MaxStage);
        animator.ResetTrigger(ParamName);
        animator.SetTrigger(ParamName);
        spriteRenderer.sprite = stageSprites[stage];

        CancelInvoke("offBeatIncrement");
        skipNextBeat = false;
    }
    
    void Start ()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        YoumuSlashTimingController.onBeat += onBeat;
        YoumuSlashTargetSpawner.OnTargetLaunch += onTargetLaunch;
	}

    void onTargetLaunch(YoumuSlashBeatMap.TargetBeat target)
    {
        if (!gameObject.activeInHierarchy)
            return;
        if (target.HitDirection != direction)
            return;

        if (otherSideNumber.getStage() > 0)  //If other side is not on stage 0, disable that and set this to next stage
        {
            setStage(otherSideNumber.getStage() + 1);
            otherSideNumber.setStage(0);
        }
        else   //Otherwise we increment it ourselves
        {
            setStage(stage + 1);   //Trigger next stage by default, max is handled naturally
        }

        if (target.LaunchBeat % 1f > 0f)
            handleOffbeat();    //Set up off-beat increment loop if not on whole number beat
    }

    //Called after assigning stage when not on beat
    void handleOffbeat()
    {
        skipNextBeat = true;
        if (stage > 0)
            Invoke("offBeatIncrement", timingData.BeatDuration);
    }

    void offBeatIncrement()
    {
        if (stage < maxNaturalStage)
            setStage(stage + 1);
        else
            setStage(0);
        handleOffbeat();
    }

    void onBeat(int beat)
    {
        if (!gameObject.activeInHierarchy)
            return;
        if (skipNextBeat)   //Beat skipped if off-beat increment occured after last beat
        {
            skipNextBeat = false;
            return;
        }
        if (timingData.BeatMap.getNextLaunchingTarget((float)beat).LaunchBeat <= (float)beat)   //Don't increment if a new target is set to launch this beat
            return;

        //Increment or reset stage normally
        if (stage >= maxNaturalStage)
            setStage(0);
        else if (stage > 0)
            setStage(stage + 1);
    }
}
