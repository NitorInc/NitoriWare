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
    private YoumuSlashSoundEffect launchSoundEffect;
    [SerializeField]
    private float leftPitch = 1f;
    [SerializeField]
    private float rightPitch = 1f;
    [SerializeField]
    private float hitOffsetMult = 2f;

    private YoumuSlashBeatMap.TargetBeat mapInstance;
    public YoumuSlashBeatMap.TargetBeat MapInstance => mapInstance;

    private bool isRight;
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
        importTargetTypeTraits(mapInstance);
        isRight = mapInstance.HitDirection == YoumuSlashBeatMap.TargetBeat.Direction.Right;
        if (isRight)
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);

        BroadcastMessage("onLaunch", MapInstance, SendMessageOptions.DontRequireReceiver);
    }

    void importTargetTypeTraits(YoumuSlashBeatMap.TargetBeat target)
    {
        body.RigAnimator.runtimeAnimatorController = target.TypeData.Animator;
        body.BaseImage.sprite = target.TypeData.Image;
        launchSoundEffect = target.TypeData.LaunchSoundEffect;
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
}
