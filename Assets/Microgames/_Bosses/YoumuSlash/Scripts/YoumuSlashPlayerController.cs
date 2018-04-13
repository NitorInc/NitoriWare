using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YoumuSlashPlayerController : MonoBehaviour
{
    [SerializeField]
    private YoumuSlashTimingData timingData;

    [Header("Timing window in seconds for hitting an object")]
    [SerializeField]
    private Vector2 hitTimeFudge;

    [Header("Debug values")]
    [SerializeField]
    private bool autoSlash;
    [SerializeField]
    private Vector2 sliceAngleRange;

    private void Start()
    {
        YoumuSlashTimingController.onBeat += onBeat;
    }

    void onBeat(int beat)
    {
        if (autoSlash)
            attemptSlash(YoumuSlashBeatMap.TargetBeat.Direction.Any);
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
    }
}
