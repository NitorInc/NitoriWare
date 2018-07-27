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
        var newTargetInstance = Instantiate(target.Prefab, transform.position, Quaternion.identity).GetComponent<YoumuSlashTarget>();
        newTargetInstance.initiate(target);
    }
}
