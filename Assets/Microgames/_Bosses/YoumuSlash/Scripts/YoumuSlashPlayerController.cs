using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YoumuSlashPlayerController : MonoBehaviour
{
    [SerializeField]
    private YoumuSlashTimingData timingData;
    [SerializeField]
    private Animator rigAnimator;

    [Header("Timing window in seconds for hitting an object")]
    [SerializeField]
    private Vector2 hitTimeFudge;

    [Header("Debug values")]
    [SerializeField]
    private bool autoSlash;
    [SerializeField]
    private Vector2 sliceAngleRange;

    bool awaitingSlash;
    YoumuSlashBeatMap.TargetBeat nextTarget;

    private void Start()
    {
        YoumuSlashTimingController.onBeat += onBeat;
    }

    void onBeat(int beat)
    {
        nextTarget = timingData.BeatMap.getFirstActiveTarget((float)beat, hitTimeFudge.y);
        if (!awaitingSlash)
        {
            if (nextTarget != null && beat + 1 >= (int)nextTarget.HitBeat)
            {
                setFacingRight(nextTarget.HitDirection == YoumuSlashBeatMap.TargetBeat.Direction.Right);
                rigAnimator.SetTrigger("Beat");
                rigAnimator.SetBool("Prep", false);
                awaitingSlash = true;
            }
            else
            {
                rigAnimator.SetTrigger("Beat");
                rigAnimator.SetBool("Prep", false);
                //setFacingRight(true);
            }
        }

        //if (autoSlash)
        //    attemptSlash(YoumuSlashBeatMap.TargetBeat.Direction.Any);
    }

    void setFacingRight(bool facingRight)
    {
        //TODO Update to spin proper object
        transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * (facingRight ? -1f : 1f),
            transform.localScale.y, transform.localScale.z);
    }

    bool isFacingRight()
    {
        return transform.localEulerAngles.x < 0f;
    }

    void Update ()
    {
        handleInput();
	}

    void handleInput()
    {
        var directionPressed = YoumuSlashBeatMap.TargetBeat.Direction.Any;
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            directionPressed = YoumuSlashBeatMap.TargetBeat.Direction.Left;
        else if (Input.GetKey(KeyCode.RightArrow))
            directionPressed = YoumuSlashBeatMap.TargetBeat.Direction.Right;

        if (directionPressed != YoumuSlashBeatMap.TargetBeat.Direction.Any)
        {
            attemptSlash(directionPressed);
        }

    }

    void attemptSlash(YoumuSlashBeatMap.TargetBeat.Direction direction)
    {
        var hitTarget = timingData.BeatMap.getFirstHittableTarget(timingData.CurrentBeat,
            hitTimeFudge.x / timingData.BeatDuration, hitTimeFudge.y / timingData.BeatDuration, direction);
        if (hitTarget != null)
        {
            hitTarget.launchInstance.slash(MathHelper.randomRangeFromVector(sliceAngleRange));
        }
        awaitingSlash = false;
    }
}
