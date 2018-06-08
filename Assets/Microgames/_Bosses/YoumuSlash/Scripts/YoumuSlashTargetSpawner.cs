using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class YoumuSlashTargetSpawner : MonoBehaviour
{
    [SerializeField]
    private YoumuSlashTimingData timingData;

    private Queue<YoumuSlashBeatMap.TargetBeat> upcomingTargets;

    private bool spawningEnabled = false;

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
            spawnTarget(upcomingTargets.Dequeue());
        }
    }

    void spawnTarget(YoumuSlashBeatMap.TargetBeat target)
    {
        var newTargetInstance = Instantiate(target.Prefab, transform.position, Quaternion.identity).GetComponent<YoumuSlashTarget>();
        newTargetInstance.initiate(target);
    }
}
