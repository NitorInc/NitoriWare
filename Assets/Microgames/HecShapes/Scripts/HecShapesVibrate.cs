using UnityEngine;

public class HecShapesVibrate : MonoBehaviour
{
    public bool vibrateOn;
    public float vibrateSpeed;
    public float vibrateRadius;

    Vector2 pivotPosition;
    Vector3 startPosition;
    Vector2 vibrateGoal;
    
    void Awake()
    {
        if (vibrateOn)
            StartVibrate();
    }
    
    void Update()
    {
        if (vibrateOn)
            UpdateVibrate();
    }
    
    void UpdateVibrate()
    {
        Vector2 diff = vibrateGoal - pivotPosition;
        if (diff.magnitude <= vibrateSpeed * Time.deltaTime)
        {
            pivotPosition = vibrateGoal;
            ResetVibrate();
        }
        else
        {
            pivotPosition +=
                MathHelper.getVector2FromAngle(MathHelper.getAngle(vibrateGoal - pivotPosition),
                vibrateSpeed * Time.deltaTime);
        }
        
        transform.position = (Vector3)pivotPosition + new Vector3(transform.position.x, transform.position.y, startPosition.z);
    }

    public void ResetVibrate()
    {
        vibrateGoal = MathHelper.getVector2FromAngle(Random.Range(0f, 360f), Random.Range(0f, vibrateRadius));
    }

    public void StartVibrate()
    {
        startPosition = transform.localPosition;
        vibrateOn = true;
    }

    public void StopVibrate()
    {
        vibrateOn = false;
    }
}
