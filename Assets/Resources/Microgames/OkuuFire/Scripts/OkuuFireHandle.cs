using System.Collections.Generic;
using UnityEngine;

public class OkuuFireHandle : MonoBehaviour
{
    // Current status of the machine.
    public float completion;

    public float maxSpeed = 1;
    public float maxDistance = 20;
    public float turnLimit = 45;

    // Grabbable guide object to track mouse position.
    public MouseGrabbable guide;
    // List of transforms which include a IOkuuFireMechanism component.
    public List<Transform> mechanisms;
    public OkuuFireCranker cranker;

    private OkuuFireCrank crank;

    private float minAngle;
    private float maxAngle;
    private float reach;

    void Start()
    {
        this.crank = this.GetComponentInParent<OkuuFireCrank>();

        float rotations = this.crank.rotations;
        this.reach = 360 * rotations;

        Vector3 crankPoint = Camera.main.WorldToScreenPoint(this.crank.transform.position);
        Vector3 handlePoint = Camera.main.WorldToScreenPoint(this.transform.position);

        // Calculate angle of the handle from the crank.
        Vector2 offset = new Vector2(handlePoint.x - crankPoint.x, handlePoint.y - crankPoint.y);
        float angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
        if (angle < 0)
            angle = (180 - Mathf.Abs(angle)) + 180;
        this.minAngle = angle;
        this.cranker.SetStartAngle(angle);

        // Calculate max cumulative angle
        this.maxAngle = this.minAngle + reach;
    }

    void Update()
    {
        // Check if the handle has been grabbed.
        if (this.guide.grabbed)
        {
            // Get the points at the centre of the crank and the centre of the mouse guide.
            Vector3 targetPoint = Camera.main.WorldToScreenPoint(this.guide.transform.position);
            Vector3 crankPoint = Camera.main.WorldToScreenPoint(this.crank.transform.position);
            Vector3 handlePoint = Camera.main.WorldToScreenPoint(this.transform.position);

            // Calculate direction of the mouse from the crank.
            Vector2 offset = new Vector2(targetPoint.x - crankPoint.x, targetPoint.y - crankPoint.y);
            float angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
            if (angle < 0)
                angle = (180 - Mathf.Abs(angle)) + 180;

            // Calculate angle change from previous.
            float currentAngle = GetCurrentAngle();
            float deltaAngle = angle - (currentAngle % 360);
            if (deltaAngle > 180)
                deltaAngle = deltaAngle - 360;
            else if (deltaAngle < -180)
                deltaAngle = deltaAngle + 360;
            
            // Limit change.
            if (deltaAngle > this.turnLimit)
                deltaAngle = this.turnLimit;
            else if (deltaAngle < -this.turnLimit)
                deltaAngle = -this.turnLimit;

            // Determine target angle.
            float targetAngle = currentAngle + deltaAngle;
            if (targetAngle < this.minAngle)
                targetAngle = this.minAngle;
            else if (targetAngle > this.maxAngle)
                targetAngle = this.maxAngle;
            this.cranker.Rotate(targetAngle);

            // Calculate the new completion amount for the whole machine.
            float deltaCompletion = ((targetAngle - this.minAngle) / this.reach) - this.completion;
            // Constrain based on time and max speed.
            deltaCompletion = deltaCompletion * Time.deltaTime * this.maxSpeed;

            float newCompletion = this.completion + deltaCompletion;
            if (newCompletion > 1F)
                newCompletion = 1F;
            else if (newCompletion < 0F)
                newCompletion = 0F;

            // Set completion and move the machine.
            this.completion = newCompletion;
            this.MoveMechanism();
        }
        else
        {
            // Make sure the guide remains on the handle when not in use.
            this.ResetGuide();
        }
    }

    public void MoveMechanism()
    {
        foreach (Transform transform in this.mechanisms)
        {
            IOkuuFireMechanism mechanism = transform.GetComponent<IOkuuFireMechanism>();
            mechanism.Move(this.completion);
        }
    }
    
    public void OnRelease()
    {
        this.ResetGuide();
    }

    void ResetGuide()
    {
        if (this.guide.transform.position != this.transform.position)
            this.guide.transform.position = this.transform.position;
    }

    float GetCurrentAngle()
    {
        return this.minAngle + (this.reach * this.completion);
    }
}
