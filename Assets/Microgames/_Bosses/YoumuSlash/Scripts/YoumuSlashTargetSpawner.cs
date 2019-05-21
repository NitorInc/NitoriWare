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
        CancelInvoke();
        enabled = false;
    }

    void enableSpawning()
    {
        spawningEnabled = true;
        invokeNextSpawn();
    }

    void Update()
    {
        if (!spawningEnabled || !upcomingTargets.Any())
            return;
        //Safety in case timing flubs up
        else if (timingData.CurrentBeat >= upcomingTargets.Peek().LaunchBeat)
        {
            print("I gotchu");
            CancelInvoke();
            var target = upcomingTargets.Dequeue();
            spawnTarget(target);
            OnTargetLaunch(target);
            invokeNextSpawn();
        }
    }

        void invokeNextSpawn()
    {
        var currentBeat = timingData.PreciseBeat;
        var spawnBeat = upcomingTargets.Peek().LaunchBeat;
        Invoke("spawnNextTarget", (spawnBeat - currentBeat) * timingData.BeatDuration);
    }

    void spawnNextTarget()
    {
        var target = upcomingTargets.Dequeue();
        spawnTarget(target);
        OnTargetLaunch(target);

        if (upcomingTargets.Any())
            invokeNextSpawn();
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
