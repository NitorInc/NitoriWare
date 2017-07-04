using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockBandAudienceMember : MonoBehaviour
{

#pragma warning disable 0649   //Serialized Fields
    [SerializeField]
    private AnimationCurve hopCurve;
    [SerializeField]
    private Vector2 hopHeightRandomBounds, hopDurationRandomBounds, hopWaitRandomBounds;
#pragma warning restore 0649

    private Vector3 startPosition;
    private float hopStartTime, hopHeight, hopWait, hopDuration;

	void Start()
	{
        startPosition = transform.position;
        resetHop();
        hopStartTime = Random.Range(-hopDuration, 0f);

        updateHop();
	}

    void resetHop()
    {
        hopStartTime = Time.time;
        hopHeight = MathHelper.randomRangeFromVector(hopHeightRandomBounds);
        hopWait = MathHelper.randomRangeFromVector(hopWaitRandomBounds);
        hopDuration = MathHelper.randomRangeFromVector(hopDurationRandomBounds);
    }
	
	void Update()
	{
        updateHop();
	}

    void updateHop()
    {
        float timeSinceHopStart = Time.time - hopStartTime,
            height = timeSinceHopStart <= hopDuration ? (hopCurve.Evaluate(timeSinceHopStart / hopDuration) * hopHeight) : 0f;
        Debug.Log(timeSinceHopStart);
        Debug.Log(hopStartTime);
        transform.position = startPosition + (Vector3.up * height);

        if (timeSinceHopStart > hopDuration + hopWait)
        {
            resetHop();
        }
    }
}
