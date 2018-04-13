using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YoumuSlashTarget : MonoBehaviour
{
    [SerializeField]
    private YoumuSlashTargetBody body;

    private YoumuSlashBeatMap.TargetBeat mapInstance;
    
	public void initiate(YoumuSlashBeatMap.TargetBeat mapInstance)
    {
        this.mapInstance = mapInstance;
        mapInstance.launchInstance = this;
        if (mapInstance.HitDirection == YoumuSlashBeatMap.TargetBeat.Direction.Right)
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
	}

    public void slash(float angle)
    {
        body.Slash(angle);
        mapInstance.slashed = true;
    }
}
