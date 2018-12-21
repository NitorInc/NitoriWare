using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YoumuSlashTargetBody : MonoBehaviour
{
    [SerializeField]
    private YoumuSlashTargetSlice leftSlice;
    [SerializeField]
    private YoumuSlashTargetSlice rightSlice;
    [SerializeField]
    private Animator rigAnimator;

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

    public void freezeLaunchAnimation()
    {
        rigAnimator.speed = 0f;
        rotSpeed = 0f;
    }

    public void slash(float angle)
    {
        setSlashedAngle(angle);
        enabled = false;
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
