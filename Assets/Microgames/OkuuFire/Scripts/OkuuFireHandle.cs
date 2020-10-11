using System.Collections.Generic;
using UnityEngine;

public class OkuuFireHandle : MonoBehaviour
{
    // Current status of the machine.
    public float completion;

    public float maxSpeed = 1;
    public float maxDistance = 20;
    public float turnLimit = 45;

    public float maxSoundLinger = 0.2F;

    // Grabbable guide object to track mouse position.
    public MouseGrabbable guide;
    public OkuuFirePulsate nob;

    public OkuuFireHeatGauge gauge;
    public SpriteRenderer clockwiseArrow;
    public SpriteRenderer anticlockwiseArrow;
    public SineWave indicatorMovement;

    // List of transforms which include a IOkuuFireMechanism component.
    public List<Transform> mechanisms;
    public OkuuFireCranker cranker;

    private OkuuFireCrank crank;
    private AudioSource crankSound;

    private float minAngle;
    private float maxAngle;
    private float reach;
    private bool canMove;
    private float soundLinger;

    void Start()
    {
        crank = GetComponentInParent<OkuuFireCrank>();
        canMove = true;

        float rotations = crank.rotations;
        reach = 360 * rotations;

        Vector3 crankPoint = MainCameraSingleton.instance.WorldToScreenPoint(crank.transform.position);
        Vector3 handlePoint = MainCameraSingleton.instance.WorldToScreenPoint(transform.position);

        // Calculate angle of the handle from the crank.
        Vector2 offset = new Vector2(handlePoint.x - crankPoint.x, handlePoint.y - crankPoint.y);
        float angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
        if (angle < 0)
            angle = (180 - Mathf.Abs(angle)) + 180;
        minAngle = angle;
        cranker.SetStartAngle(angle);

        // Calculate max cumulative angle
        maxAngle = minAngle + reach;

        crankSound = GetComponent<AudioSource>();
        crankSound.Pause();

        MoveMechanism();
        ShowArrow(gauge.GetTargetOffset());
    }

    void Update()
    {
        float deltaCompletion = 0;

        // Check if the handle has been grabbed.
        if (canMove && guide.grabbed)
        {
            // Get the points at the centre of the crank and the centre of the mouse guide.
            Vector3 targetPoint = MainCameraSingleton.instance.WorldToScreenPoint(guide.transform.position);
            Vector3 crankPoint = MainCameraSingleton.instance.WorldToScreenPoint(crank.transform.position);

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
            if (deltaAngle > turnLimit)
                deltaAngle = turnLimit;
            else if (deltaAngle < -turnLimit)
                deltaAngle = -turnLimit;

            // Determine target angle.
            float targetAngle = currentAngle + deltaAngle;
            if (targetAngle < minAngle)
                targetAngle = minAngle;
            else if (targetAngle > maxAngle)
                targetAngle = maxAngle;
            cranker.Rotate(targetAngle);

            // Calculate the new completion amount for the whole machine.
            deltaCompletion = ((targetAngle - minAngle) / reach) - completion;
            // Constrain based on time and max speed.
            deltaCompletion = deltaCompletion * Time.deltaTime * maxSpeed;

            float newCompletion = completion + deltaCompletion;
            if (newCompletion > 1F)
                newCompletion = 1F;
            else if (newCompletion < 0F)
                newCompletion = 0F;

            // Set completion.
            completion = newCompletion;

            // Move the machine.
            MoveMechanism();
        }
        else
        {
            // Make sure the guide remains on the handle when not in use.
            ResetGuide();
        }

        if (canMove && !guide.grabbed)
            indicatorMovement.enabled = true;
        else
            indicatorMovement.enabled = false;

        // Play sound
        float speed = Mathf.Abs(deltaCompletion / Time.deltaTime);
        if (Mathf.Abs(deltaCompletion) > 0.0001)
        {
            // Reset
            soundLinger = maxSoundLinger;

            // Volume/pitch control
            crankSound.volume = (soundLinger / maxSoundLinger) * speed * 3;
            crankSound.pitch = speed + 0.8F;

            // Play
            if (!crankSound.isPlaying)
                crankSound.Play();
        }
        else
        {
            // Decay
            soundLinger -= Time.deltaTime;

            // Volume/pitch control
            float newVolume = soundLinger / maxSoundLinger;
            crankSound.volume = newVolume;

            // Pause
            if (soundLinger <= 0)
                crankSound.Pause();
        }
    }

    public void MoveMechanism()
    {
        foreach (Transform transform in mechanisms)
        {
            IOkuuFireMechanism mechanism = transform.GetComponent<IOkuuFireMechanism>();
            mechanism.Move(completion);
        }
    }

    public void OnGrab()
    {
        nob.Pulse = false;

        HideArrow();
    }

    public void OnRelease()
    {
        if (canMove)
        {
            nob.Pulse = true;
        }

        ShowArrow(gauge.GetTargetOffset());
    }

    void ShowArrow(float offset)
    {
        if (!gauge.InTargetZone())
        {
            if (offset > 0)
            {
                clockwiseArrow.enabled = true;
                anticlockwiseArrow.enabled = false;
            }
            else
            {
                clockwiseArrow.enabled = false;
                anticlockwiseArrow.enabled = true;
            }
        }
    }

    void HideArrow()
    {
        clockwiseArrow.enabled = false;
        anticlockwiseArrow.enabled = false;
    }

    void ResetGuide()
    {
        if (guide.transform.position != transform.position)
            guide.transform.position = transform.position;
    }

    float GetCurrentAngle()
    {
        return minAngle + (reach * completion);
    }

    void Victory()
    {
        canMove = false;
        nob.Pulse = false;
    }
}
