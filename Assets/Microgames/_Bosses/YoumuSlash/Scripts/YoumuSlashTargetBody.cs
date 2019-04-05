using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class YoumuSlashTargetBody : MonoBehaviour
{
    [SerializeField]
    private YoumuSlashTargetSlice leftSlice;
    [SerializeField]
    private YoumuSlashTargetSlice rightSlice;
    [SerializeField]
    private Animator rigAnimator;
    public Animator RigAnimator => rigAnimator;
    [SerializeField]
    private Image baseImage;
    public Image BaseImage => baseImage;

    public YoumuSlashTargetSlice LeftSlice => leftSlice;
    public YoumuSlashTargetSlice RightSlice => rightSlice;

    [SerializeField]
    private Vector2 lauchRotSpeedRange;

    private float rotSpeed;

    private void Start()
    {
        rotSpeed = -MathHelper.randomRangeFromVector(lauchRotSpeedRange);
        transform.localEulerAngles = Vector3.forward * Random.Range(0f, 360f);
    }

    private void Update()
    {
        transform.localEulerAngles += Vector3.forward * rotSpeed * Time.deltaTime;
    }

    public void onSlashActivate(float slashSpeed)
    {
        rigAnimator.SetFloat("LaunchSpeed", 0f);
        rigAnimator.SetFloat("SlashSpeed", slashSpeed);
        rigAnimator.SetTrigger("Slash");
        rotSpeed = 0f;
    }

    public void onSlashDelay(float angle, Vector3 maskOffset)
    {
        setSlashedAngle(angle);
        enabled = false;

        LeftSlice.getImageTransform().position += maskOffset;
        LeftSlice.getMaskTransform().position -= maskOffset;
        RightSlice.getImageTransform().position += maskOffset;
        RightSlice.getMaskTransform().position -= maskOffset;

        leftSlice.setImageActive(baseImage.sprite);
        rightSlice.setImageActive(baseImage.sprite);
        baseImage.enabled = false;
    }

    void setSlashedAngle(float angle)
    {
        Vector3 rotVector = Vector3.forward * angle;
        applySliceSlash(leftSlice, rotVector);
        applySliceSlash(rightSlice, rotVector);
        transform.localEulerAngles = Vector3.zero;
    }

    void applySliceSlash(YoumuSlashTargetSlice slice, Vector3 slashRotation)
    {
        slice.transform.localEulerAngles = slashRotation;
        slice.getImageTransform().localEulerAngles = transform.localEulerAngles - slashRotation;
        slice.setFalling(true);
    }
}
