using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YoumuSlashTargetAnimator : MonoBehaviour
{
    [SerializeField]
    private YoumuSlashTimingData timingData;
    [SerializeField]
    private float syncBufferTime = .075f;

    private Animator rigAnimator;
    private YoumuSlashBeatMap.TargetBeat target;
    private float simulatedLaunchedTime;
    
	void Awake ()
    {
        rigAnimator = GetComponent<Animator>();
	}

    void onLaunch(YoumuSlashBeatMap.TargetBeat mapInstance)
    {
        target = mapInstance;
        simulatedLaunchedTime = Time.time;
    }

    void onSlash(YoumuSlashTarget.SlashData slashData)
    {
        enabled = false;
    }


    void Update ()
    {
        var timeSinceLaunch = (timingData.PreciseBeat - target.LaunchBeat) * timingData.BeatDuration;
        var simulatedTimeSinceLaunch = Time.time - simulatedLaunchedTime;
        if (!MathHelper.Approximately(timeSinceLaunch, simulatedTimeSinceLaunch, syncBufferTime))
        {
            var normalizedTime = (timingData.PreciseBeat - target.LaunchBeat) / 4f;
            rigAnimator.Rebind();
            rigAnimator.Play("Launch", 0, normalizedTime);
            simulatedLaunchedTime = Time.time - simulatedTimeSinceLaunch;
        }
    }
}
