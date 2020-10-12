using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BandConductorCircleGesture : BandConductorHandGesture
{
    [SerializeField]
    private float radius = .5f;
    [SerializeField]
    private float power = 5f;
    [SerializeField]
    private float anglePerNote = 120f;
    [SerializeField]
    private float angleProgressMult = .05f;
    [SerializeField]
    private BandConductorNotePlayer notePlayer;
    [SerializeField]
    private bool isClockWise;

    private float currentAngle = 90f;
    private int notesPlayed = 0;

    public override void ResetGesture()
    {
        base.ResetGesture();
        currentAngle = 270f;
        notesPlayed = 0;
    }

    public override void Update()
    {
        var normalAngle = currentAngle + (isClockWise ? -90f : 90f);
        var mouseVector = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"))
            * (MicrogameController.instance.session.BeatsRemaining > maxBeatsLeftForInput ? 0f : 1f);
        var projection = Vector3.Project(mouseVector, MathHelper.getVector2FromAngle(normalAngle, 1f));

        if ( MathHelper.Approximately(
            MathHelper.trueMod(((Vector2)projection).getAngle(), 360f),
            MathHelper.trueMod(normalAngle, 360f),
            1f))
        {
            currentAngle += projection.magnitude * power * (isClockWise ? -1f : 1f);
            Debug.DrawLine(transform.position, transform.position + projection);
        }
        else
            Debug.DrawLine(transform.position, transform.position + projection, Color.red);

        transform.localPosition = MathHelper.getVector2FromAngle(currentAngle, radius);
        
        var angleProgress = (currentAngle - 270f) * (isClockWise ? -1f : 1f);
        progress = angleProgress * angleProgressMult;

        if (angleProgress > anglePerNote * (notesPlayed + 1))
        {
            notePlayer.PlayNote();
            notesPlayed++;
        }

    }

    public override float GetConductorAnimationPosition()
    {
        return Mathf.InverseLerp(radius, -radius, transform.localPosition.x);
    }

    public override Vector3 getStartPosition()
    {
        return Vector3.down * radius;
    }
}
