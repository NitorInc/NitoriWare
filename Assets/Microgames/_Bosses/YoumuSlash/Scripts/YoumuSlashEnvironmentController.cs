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
        animator.Play("TutorialBeat" + (target.HitDirection == YoumuSlashBeatMap.TargetBeat.Direction.Right ? "Right" : "Left"));
    }
	
	void onAttack(YoumuSlashBeatMap.TargetBeat target)
    {
        if (target != null)
            animator.Play("ArrowGlow" + (target.HitDirection == YoumuSlashBeatMap.TargetBeat.Direction.Right ? "Right" : "Left"));
    }
}
