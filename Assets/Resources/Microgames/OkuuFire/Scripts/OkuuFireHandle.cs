using System.Collections.Generic;
using UnityEngine;

public class OkuuFireHandle : MonoBehaviour
{
    // Current status of the machine.
    public float completion;

    public float maxSpeed = 1;
    public float maxDistance = 10;

    // Grabbable guide object to track mouse position.
    public MouseGrabbable guide;
    // List of transforms which include a IOkuuFireMechanism component.
    public List<Transform> mechanisms;

    void Update()
    {
        // Check if the handle has been grabbed.
        if (this.guide.grabbed)
        {
            // Get the points at the centre of the handle and the centre of the mouse guide.
            Vector3 targetPoint = Camera.main.WorldToScreenPoint(this.guide.transform.localPosition);
            Vector3 handlePoint = Camera.main.WorldToScreenPoint(this.transform.localPosition);

            // Distance of mouse from centre of handle affects turn speed.
            float distance = Vector3.Distance(targetPoint, handlePoint);
            if (distance > this.maxDistance)
                distance = this.maxDistance;
            float distanceMultiplier = distance / this.maxDistance;

            // Calculate direction of the mouse from the handle.
            Vector2 offset = new Vector2(targetPoint.x - handlePoint.x, targetPoint.y - handlePoint.y);
            float angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;

            // Favour mechanically reasonable configurations.
            float vector = 1F - Mathf.Abs(Mathf.Abs(angle) - 90F) / 90F;
            if (angle < 0F)
                vector = -vector;

            // Calculate the new completion amount for the whole machine.
            float deltaCompletion = Time.deltaTime * this.maxSpeed * distanceMultiplier * vector;
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

    void ResetGuide()
    {
        if (this.guide.transform.position != this.transform.position)
            this.guide.transform.position = this.transform.position;
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
}
