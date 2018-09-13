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
	}

    void onTargetLaunched(YoumuSlashBeatMap.TargetBeat target)
    {
        if (target.HitEffect == YoumuSlashBeatMap.TargetBeat.Effect.Burst)
            setTrigger("Burst");
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
