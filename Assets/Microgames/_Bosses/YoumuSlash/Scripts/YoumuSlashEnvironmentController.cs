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
        YoumuSlashTargetSpawner.OnTargetLaunch += onTargetLaunched;
        animator = GetComponent<Animator>();
	}

    void onTargetLaunched(YoumuSlashBeatMap.TargetBeat target)
    {
        animator.Play("TutorialBeat" + (target.HitDirection == YoumuSlashBeatMap.TargetBeat.Direction.Right ? "Right" : "Left"));
    }
	
	void Update ()
    {
		
	}
}
