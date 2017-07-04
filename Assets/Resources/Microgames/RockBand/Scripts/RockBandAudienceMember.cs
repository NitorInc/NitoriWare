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
    [SerializeField]
    private SpriteRenderer spriteRenderer;
#pragma warning restore 0649

    private Vector3 startPosition;
    private float hopStartTime, hopHeight, hopWait, hopDuration;

	void Start()
	{
        startPosition = transform.position;
        resetHop();
        hopStartTime = Random.Range(-hopDuration, 0f);
        spriteRenderer.sortingOrder += getNumberFromName();

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
        
        transform.position = startPosition + (Vector3.up * height);

        if (timeSinceHopStart > hopDuration + hopWait)
        {
            resetHop();
        }
    }

    int getNumberFromName()
    {
        if (!name.Contains(")"))
            return 0;
        return int.Parse(name.Split('(')[1].Split(')')[0]);
    }
}
