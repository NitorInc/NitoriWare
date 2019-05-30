using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class YoumuSlashTargetSpawner : MonoBehaviour
{
    public delegate void TargetLaunchDelegate(YoumuSlashBeatMap.TargetBeat target);
    public delegate void SongStartDelegate();
    public static TargetLaunchDelegate OnTargetLaunch;

    [SerializeField]
    private YoumuSlashTimingData timingData;
    [SerializeField]
    private float targetInvokeWindow = .1f;

    private Queue<YoumuSlashBeatMap.TargetBeat> upcomingTargets;

    private bool watchForNextTarget = false;

    private void Awake()
    {
        OnTargetLaunch = null;
    }

    void Start ()
    {
        upcomingTargets = new Queue<YoumuSlashBeatMap.TargetBeat>(timingData.BeatMap.TargetBeats);
        YoumuSlashTimingController.onMusicStart += enableSpawning;
        YoumuSlashPlayerController.onFail += onFail;
    }

    void onFail()
    {
        watchForNextTarget = false;
        enabled = false;
    }

    void enableSpawning()
    {
        watchForNextTarget = true;
    }

    void FixedUpdate()
    {
        if (!watchForNextTarget || !upcomingTargets.Any())
            return;

        var nextTarget = upcomingTargets.Peek();
        var timeToNextTarget = (nextTarget.LaunchBeat - timingData.PreciseBeat) * timingData.BeatDuration;
        timeToNextTarget = Mathf.Max(timeToNextTarget, 0f);
        if (timeToNextTarget <= targetInvokeWindow
            || timeToNextTarget <= Time.fixedDeltaTime)
        {
            Invoke("launchNextTarget", timeToNextTarget);
            YoumuSlashSoundEffectPlayer.instance.playScheduled(nextTarget.TypeData.LaunchSoundEffect, nextTarget.HitDirection,
                timeToNextTarget);
            watchForNextTarget = false;
        }
    }

    void launchNextTarget()
    {
        var target = upcomingTargets.Dequeue();
        spawnTarget(target);
        OnTargetLaunch(target);

        watchForNextTarget = true;
    }

    void spawnTarget(YoumuSlashBeatMap.TargetBeat target)
    {
        if (target.LaunchBeat % 1f == 0f
            && timingData.LastProcessedBeat < (int)target.LaunchBeat)   //If whole number, force TimingController to call OnBeat, to make sure beat is called before launch
        {
            YoumuSlashTimingController.onBeat(timingData.LastProcessedBeat + 1);
        }

        var newTargetInstance = Instantiate(target.TypeData.Prefab, transform.position, Quaternion.identity).GetComponent<YoumuSlashTarget>();
        newTargetInstance.initiate(target);
    }
}
