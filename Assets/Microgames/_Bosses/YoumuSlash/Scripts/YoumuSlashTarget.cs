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
    public YoumuSlashBeatMap.TargetBeat MapInstance => mapInstance;

    private bool isRight;
    private AudioSource sfxSource;
    private float slashAngle;
    
	public void initiate(YoumuSlashBeatMap.TargetBeat mapInstance)
    {
        this.mapInstance = mapInstance;
        mapInstance.launchInstance = this;
        isRight = mapInstance.HitDirection == YoumuSlashBeatMap.TargetBeat.Direction.Right;
        if (isRight)
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);

        sfxSource = GetComponent<AudioSource>();
        sfxSource.panStereo = launchPan * (isRight ? 1f : -1f);
        sfxSource.pitch = (isRight ? rightPitch : leftPitch) * Time.timeScale;
        sfxSource.PlayOneShot(launchClip);
    }

    public void slash(float angle, float effectActivationTime)
    {
        mapInstance.slashed = true;
        slashAngle = angle;
        Invoke("activateSlashEffect", effectActivationTime);
        body.freezeLaunchAnimation();
    }

    public void activateSlashEffect()
    {
        body.slash(slashAngle);

        sfxSource.panStereo = slashPan * (isRight ? 1f : -1f);
        sfxSource.pitch = (isRight ? rightPitch : leftPitch) * Time.timeScale;
        sfxSource.PlayOneShot(slashClip);
    }
}
