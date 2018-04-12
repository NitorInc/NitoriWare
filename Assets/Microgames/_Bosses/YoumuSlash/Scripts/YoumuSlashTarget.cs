using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YoumuSlashTarget : MonoBehaviour
{
    [Header("Debug value until sword slice angle is implemented")]
    [SerializeField]
    private Vector2 sliceAngleRange;
    [SerializeField]
    private YoumuSlashTargetSlice leftSlice;
    [SerializeField]
    private YoumuSlashTargetSlice rightSlice;
    

    private void Update()
    {
        //Debug behavior
        transform.position += (Vector3.down  + Vector3.right) * Time.deltaTime * 2f;
        transform.localEulerAngles += Vector3.forward * -300f * Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            setSlashedAngle(MathHelper.randomRangeFromVector(sliceAngleRange));
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
