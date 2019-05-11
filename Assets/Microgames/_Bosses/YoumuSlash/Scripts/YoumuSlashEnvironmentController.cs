using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YoumuSlashEnvironmentController : MonoBehaviour
{
    [SerializeField]
    private YoumuSlashTimingController timingData;

    private Animator animator;
    
	void Start ()
    {
        animator = GetComponent<Animator>();
        YoumuSlashTargetSpawner.OnTargetLaunch += onTargetLaunched;
        YoumuSlashPlayerController.onAttack += onAttack;
        YoumuSlashPlayerController.onFail += onFail;
        YoumuSlashPlayerController.onGameplayEnd += onGameplayEnd;
	}

    void onTargetLaunched(YoumuSlashBeatMap.TargetBeat target)
    {
        if (target.TypeData.LaunchEffect.ToString().EndsWith("Burst"))
        {
            animator.SetInteger("BurstLevel", getBurstValue(target.TypeData.LaunchEffect));
            setTrigger("Burst");
        }
    }
    
    void onGameplayEnd()
    {
        setTrigger("GameplayEnd");
    }

    void onFail()
    {
        animator.SetTrigger("Fail");
    }

    int getBurstValue(YoumuSlashTargetType.Effect effect)
    {
        switch (effect)
        {
            case (YoumuSlashTargetType.Effect.SlowBurst):
                return 1;
            case (YoumuSlashTargetType.Effect.FastBurst):
                return 2;
            case (YoumuSlashTargetType.Effect.RapidBurst):
                return 3;
            default:
                return 0;
        }

    }
	
	void onAttack(YoumuSlashBeatMap.TargetBeat target)
    {
        if (target != null)
        {
            setTrigger("Hit" + (target.HitDirection == YoumuSlashBeatMap.TargetBeat.Direction.Right ? "Right" : "Left"));
        }
    }

    void setTrigger(string name)
    {
        animator.ResetTrigger(name);
        animator.SetTrigger(name);
    }
}
