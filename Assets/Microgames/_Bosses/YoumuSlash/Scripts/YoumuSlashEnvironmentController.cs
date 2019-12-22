using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class YoumuSlashEnvironmentController : MonoBehaviour
{
    [SerializeField]
    private YoumuSlashTimingData timingData;

    private Animator animator;
    private float lastTargetBeat = -1f;
    
	void Start ()
    {
        animator = GetComponent<Animator>();
        YoumuSlashTargetSpawner.OnTargetLaunch += onTargetLaunched;
        YoumuSlashPlayerController.onAttack += onAttack;
        YoumuSlashPlayerController.onFail += onFail;
        YoumuSlashPlayerController.onGameplayEnd += onGameplayEnd;
        YoumuSlashTimingController.onBeat += onBeat;
	}

    void onTargetLaunched(YoumuSlashBeatMap.TargetBeat target)
    {
        if (target.TypeData.LaunchEffect.ToString().EndsWith("Burst"))
        {
            animator.SetInteger("BurstLevel", getBurstValue(target.TypeData.LaunchEffect));
            setTrigger("Burst");
        }
        lastTargetBeat = target.LaunchBeat;
    }

    void onBeat(int beat)
    {
        // Check for off-beat notifications
        var nextTarget = timingData.BeatMap.getNextLaunchingTarget(beat + .01f);

        if (nextTarget != null
            && nextTarget.LaunchBeat - beat < 1f
            && nextTarget.LaunchBeat % 1f > 0f
            && !timingData.BeatMap.TargetBeats.Any(a => a.LaunchBeat == (float)beat))
        {
            setTrigger(
                nextTarget.HitDirection == YoumuSlashBeatMap.TargetBeat.Direction.Right
                ? "OffbeatRight"
                : "OffbeatLeft");
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
            case (YoumuSlashTargetType.Effect.SingleBurst):
                return 4;
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
