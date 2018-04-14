using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YoumuSlashTarget : MonoBehaviour
{
    [SerializeField]
    private YoumuSlashTargetBody body;
    [SerializeField]
    private AudioClip launchClip;
    [SerializeField]
    private AudioClip slashClip;
    [SerializeField]
    private float launchPan = .5f;
    [SerializeField]
    private float slashPan = .5f;
    [SerializeField]
    private float leftPitch = 1f;
    [SerializeField]
    private float rightPitch = 1f;

    private YoumuSlashBeatMap.TargetBeat mapInstance;
    private bool isRight;
    
	public void initiate(YoumuSlashBeatMap.TargetBeat mapInstance)
    {
        this.mapInstance = mapInstance;
        mapInstance.launchInstance = this;
        isRight = mapInstance.HitDirection == YoumuSlashBeatMap.TargetBeat.Direction.Right;
        if (isRight)
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);

        MicrogameController.instance.playSFX(launchClip,
            panStereo: launchPan * (isRight ? 1f : -1f),
            pitchMult: isRight ? rightPitch : leftPitch);
	}

    public void slash(float angle)
    {
        body.slash(angle);
        mapInstance.slashed = true;

        MicrogameController.instance.playSFX(slashClip,
            panStereo: slashPan * (isRight ? 1f : -1f),
            pitchMult: isRight ? rightPitch : leftPitch);
    }
}
