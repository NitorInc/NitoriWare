using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YoumuSlashTutorialNumber : MonoBehaviour
{
    private const string ParamName = "Pulse";

    [SerializeField]
    private YoumuSlashBeatMap.TargetBeat.Direction direction;
    [SerializeField]
    private Sprite[] stageSprites;

    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private bool skipBeat;

    int stage = 0;
    private void setStage(int value)
    {
        stage = value;
        if (value != 0 && value < stageSprites.Length)
        {
            animator.ResetTrigger(ParamName);
            animator.SetTrigger(ParamName);
            spriteRenderer.sprite = stageSprites[value];
        }
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
        
        switch(stage)
        {
            case (0):   //Start 1 under normal launch
                setStage(1);
                break;
            case (1):   //Force to 2 if we're already in stage 1 (usually means target is half-beat)
                setStage(2);
                break;
            default:
                break;
        }
        if (target.LaunchBeat % 1f != 0f)
            skipBeat = true;
    }

    void onBeat(int beat)
    {
        if (!gameObject.activeInHierarchy)
            return;
        if (skipBeat)   //Beat skipped if half-beat was activated after last beat
        {
            skipBeat = false;
            return;
        }
        
        switch (stage)
        {
            case (1):   //Move on to 2 if we're at 1
                setStage(2);
                break;
            case (2):   //Reset if we're at 2
                setStage(0);
                break;
            default:
                break;
        }
    }
}
