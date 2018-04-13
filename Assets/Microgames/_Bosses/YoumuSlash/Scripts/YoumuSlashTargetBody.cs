using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YoumuSlashTargetBody : MonoBehaviour
{
    [SerializeField]
    private YoumuSlashTargetSlice leftSlice;
    [SerializeField]
    private YoumuSlashTargetSlice rightSlice;

    [Header("Debug values")]
    [SerializeField]
    private Vector2 sliceAngleRange;
    [SerializeField]
    private Vector2 lauchRotSpeedRange;
    [SerializeField]
    private Animator rigAnimator;

    private float rotSpeed;

    private void Start()
    {
        rotSpeed = -MathHelper.randomRangeFromVector(lauchRotSpeedRange);
    }

    private void Update()
    {
        transform.localEulerAngles += Vector3.forward * rotSpeed * Time.deltaTime;

        //Debug behavior
        transform.position += (Vector3.down  + Vector3.right) * Time.deltaTime * 2f;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            setSlashedAngle(MathHelper.randomRangeFromVector(sliceAngleRange));
            rigAnimator.speed = 0f;
            enabled = false;
        }
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
