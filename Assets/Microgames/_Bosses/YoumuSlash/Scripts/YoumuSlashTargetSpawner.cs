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

    private Queue<YoumuSlashBeatMap.TargetBeat> upcomingTargets;

    private bool spawningEnabled = false;

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
        spawningEnabled = false;
        enabled = false;
    }

    void enableSpawning()
    {
        spawningEnabled = true;
    }

    void Update()
    {
        if (!spawningEnabled || !upcomingTargets.Any())
            return;
        else if (timingData.CurrentBeat >= upcomingTargets.Peek().LaunchBeat)
        {
            var target = upcomingTargets.Dequeue();
            spawnTarget(target);
            OnTargetLaunch(target);
        }
    }

    void spawnTarget(YoumuSlashBeatMap.TargetBeat target)
    {
        if (target.LaunchBeat % 1f == 0f
            && timingData.LastProcessedBeat < (int)target.LaunchBeat)   //If whole number, force TimingController to call OnBeat, to make sure beat is called before launch
        {
            YoumuSlashTimingController.onBeat(timingData.LastProcessedBeat + 1);
        }

        var newTargetInstance = Instantiate(target.Prefab, transform.position, Quaternion.identity).GetComponent<YoumuSlashTarget>();
        if (target.OverrideAnimator != null)
            newTargetInstance.overrideAnimatorController(target.OverrideAnimator);
        if (target.OverrideImage != null)
            newTargetInstance.overrideImage(target.OverrideImage);
        if (target.OverrideSound != null)
            newTargetInstance.overrideSound(target.OverrideSound);
        newTargetInstance.initiate(target);
    }
}
