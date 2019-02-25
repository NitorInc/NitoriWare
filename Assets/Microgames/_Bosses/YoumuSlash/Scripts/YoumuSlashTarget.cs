using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YoumuSlashTarget : MonoBehaviour
{
    [SerializeField]
    private YoumuSlashTimingData timingData;
    [SerializeField]
    private YoumuSlashTargetBody body;
    [SerializeField]
    private YoumuSlashHitEffectController hitEffects;
    [SerializeField]
    private AudioClip launchClip;
    [SerializeField]
    private float launchPan = .5f;
    [SerializeField]
    private float slashPan = .5f;
    [SerializeField]
    private float leftPitch = 1f;
    [SerializeField]
    private float rightPitch = 1f;
    [SerializeField]
    private float hitOffsetMult = 2f;

    private YoumuSlashBeatMap.TargetBeat mapInstance;
    public YoumuSlashBeatMap.TargetBeat MapInstance => mapInstance;

    private bool isRight;
    private AudioSource sfxSource;
    private float slashAngle;
    private float slashTimeOffset;

    public class SlashData
    {
        public YoumuSlashBeatMap.TargetBeat target;
        public float angle;
        public float timeOffset;
    }

    
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

    public void slash(float angle, float effectActivationTime, float timeOffset)
    {
        mapInstance.slashed = true;
        slashAngle = angle;
        Invoke("activateSlashEffect", effectActivationTime);

        var timeUntilNextBeat = timingData.BeatDuration - timeOffset;
        var slashSpeed = 1f / (timeUntilNextBeat / timingData.BeatDuration);    //Speed slash animation should go to compensate for time offset (for yuyuko food)
        slashSpeed = Mathf.Pow(slashSpeed, .5f);
        body.onSlashActivate(slashSpeed);

        slashTimeOffset = timeOffset;

        var slashData = new SlashData
        {
            angle = angle,
            timeOffset = timeOffset,
            target = mapInstance
        };
        BroadcastMessage("onSlash", slashData, SendMessageOptions.DontRequireReceiver);
    }

    public void activateSlashEffect()
    {
        var distanceOffset = Vector3.down * slashTimeOffset * hitOffsetMult;
        body.onSlashDelay(slashAngle, distanceOffset);
    }

    public void overrideAnimatorController(RuntimeAnimatorController animatorController)
    {
        body.RigAnimator.runtimeAnimatorController = animatorController;
    }

    public void overrideImage(Sprite sprite)
    {
        body.BaseImage.sprite = sprite;
    }

    public void overrideSound(AudioClip overrideClip)
    {
        hitEffects.NormalClip = overrideClip;
    }
}
