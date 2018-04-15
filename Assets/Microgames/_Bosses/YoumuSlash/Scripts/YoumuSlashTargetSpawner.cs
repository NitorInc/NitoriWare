using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class YoumuSlashTargetSpawner : MonoBehaviour
{
    [SerializeField]
    private YoumuSlashTimingData timingData;

    private Queue<YoumuSlashBeatMap.TargetBeat> upcomingTargets;
    private YoumuSlashBeatMap.TargetBeat nextTarget;
    
	void Start ()
    {
        upcomingTargets = new Queue<YoumuSlashBeatMap.TargetBeat>(timingData.BeatMap.TargetBeats);
        YoumuSlashTimingController.onMusicStart += InvokeNextTarget;
    }

    void InvokeNextTarget()
    {
        if (!upcomingTargets.Any())
            return;

        nextTarget = upcomingTargets.Dequeue();
        Invoke("spawnTarget", (nextTarget.LaunchBeat - timingData.CurrentBeat) * timingData.BeatDuration);
    }

    void spawnTarget()
    {
        var newTarget = Instantiate(nextTarget.Prefab, transform.position, Quaternion.identity).GetComponent<YoumuSlashTarget>();
        newTarget.initiate(nextTarget);
        InvokeNextTarget();
    }
}
