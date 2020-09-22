using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BandConductorVerticalGesture : BandConductorHandGesture
{
    [SerializeField]
    private float extent = .5f;
    [SerializeField]
    private float aboutFaceThreshold = .25f;
    [SerializeField]
    private float verticalProgressMult = .25f;
    [SerializeField]
    private float horizontalProgressLossMult = .5f;
    [SerializeField]
    private float maxSpeed = 10f;
    [SerializeField]
    private bool isHorizontal;

    [SerializeField]
    private float progressMult = .25f;
    [SerializeField]
    private float dampenMultA = 1f;
    [SerializeField]
    private float dampenMultB = 0f;
    [SerializeField]
    private float dampOverdrawCooldown = .1f;

    [SerializeField]
    private float extentNotePlayGoal = .4f;
    [SerializeField]
    private BandConductorNotePlayer notePlayer;

    private float dampOverDrawTimer = 0f;
    private float lastExtent = 0f;

    bool movingUp = true;
    //private Vector3 lastMousePosition;

    //private void Start()
    //{
    //    ResetGesture();
    //}

    public override void ResetGesture()
    {
        base.ResetGesture();
        movingUp = true;
        //lastMousePosition = CameraHelper.getCursorPosition();
        lastExtent = 0f;
    }

    public override void Update()
    {
        //var mousePosition = CameraHelper.getCursorPosition();
        var progressDiff = GetCorrectAxis() * verticalProgressMult
            * (MicrogameController.instance.session.BeatsRemaining > maxBeatsLeftForInput ? 0f : 1f);
        progressDiff = Mathf.MoveTowards(progressDiff, 0f, Mathf.Abs(GetIncorrectAxis() * horizontalProgressLossMult));
        progressDiff = Mathf.Min(progressDiff, maxSpeed * Time.deltaTime);

        if (GetCorrectAxis() != 0f && progressDiff == 0f)
            dampOverDrawTimer = dampOverdrawCooldown;
        if (dampOverDrawTimer > 0f)
        {
            dampOverDrawTimer -= Time.deltaTime;
            progressDiff = 0f;
        }

        if (progressDiff > 0f && (movingUp || GetLinearPosition() < -aboutFaceThreshold))
        {
            var mult = Mathf.Lerp(dampenMultA, dampenMultB, Mathf.InverseLerp(-extent, extent, GetLinearPosition()));
            progressDiff *= mult;

            var holdPos = GetLinearPosition();
            var newPos = Mathf.MoveTowards(GetLinearPosition(), extent, Mathf.Abs(progressDiff));
            SetLinearPosition(GetLinearPosition() + (newPos - holdPos));
            progress += (newPos - holdPos) * progressMult;
            movingUp = true;
        }
        else if (progressDiff < 0f && (!movingUp || GetLinearPosition() > aboutFaceThreshold))
        {
            var mult = Mathf.Lerp(dampenMultA, dampenMultB, Mathf.InverseLerp(extent, -extent, GetLinearPosition()));
            progressDiff *= mult;

            var holdPos = GetLinearPosition();
            var newPos = Mathf.MoveTowards(GetLinearPosition(), -extent, Mathf.Abs(progressDiff));
            SetLinearPosition(GetLinearPosition() + (newPos - holdPos));
            progress -= (newPos - holdPos) * progressMult;
            movingUp = false;
        }


        if (Mathf.Abs(GetLinearPosition()) > extentNotePlayGoal && lastExtent != Mathf.Sign(GetLinearPosition()))
        {
            notePlayer.PlayNote();
            lastExtent = Mathf.Sign(GetLinearPosition());
        }
    }

    public override Vector3 getStartPosition() => Vector3.zero;

    public override float GetConductorAnimationPosition()
    {
        return Mathf.InverseLerp(-extent, extent, -GetLinearPosition());
    }

    float GetLinearPosition()
    {
        if (isHorizontal)
            return transform.localPosition.x;
        else
            return transform.localPosition.y;
    }

    void SetLinearPosition(float newPos)
    {
        if (isHorizontal)
            transform.localPosition = new Vector3(newPos, transform.localPosition.y, transform.localPosition.z);
        else
            transform.localPosition = new Vector3(transform.localPosition.x, newPos, transform.localPosition.z);
    }

    public float GetCorrectAxis()
    {
        if (isHorizontal)
            return Input.GetAxis("Mouse X");
        else
            return Input.GetAxis("Mouse Y");
    }

    public float GetIncorrectAxis()
    {
        if (isHorizontal)
            return Input.GetAxis("Mouse Y");
        else
            return Input.GetAxis("Mouse X");
    }
}
