using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YoumuSlashTargetAnimator : MonoBehaviour
{
    [SerializeField]
    private YoumuSlashTimingData timingData;
    [SerializeField]
    private float syncBufferNormalizedTime = .01f;

    private Animator rigAnimator;
    private YoumuSlashBeatMap.TargetBeat target;
    
	void Awake ()
    {
        rigAnimator = GetComponent<Animator>();
	}

    void onLaunch(YoumuSlashBeatMap.TargetBeat mapInstance)
    {
        target = mapInstance;
    }

    void onSlash(YoumuSlashTarget.SlashData slashData)
    {
        enabled = false;
    }


    void Update ()
    {
        //var timeSinceLaunch = (timingData.PreciseBeat - target.LaunchBeat) * timingData.BeatDuration;
        //var simulatedTimeSinceLaunch = Time.time - simulatedLaunchedTime;
        var goalNormalizedTime = (timingData.PreciseBeat - target.LaunchBeat) / 4f;
        var normalizedTime= rigAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime;
        if (!MathHelper.Approximately(normalizedTime, goalNormalizedTime, syncBufferNormalizedTime))
        {
            rigAnimator.Rebind();
            rigAnimator.Play("Launch", 0, goalNormalizedTime);
            print("Correcting animator");
        }
    }
}
