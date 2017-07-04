using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockBandAudienceMember : MonoBehaviour
{

#pragma warning disable 0649   //Serialized Fields
    [SerializeField]
    private AnimationCurve hopCurve;
    [SerializeField]
    private Vector2 hopHeightRandomBounds, hopDurationRandomBounds, hopWaitRandomBounds, flipCooldownBounds;
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private Sprite[] sprites;
#pragma warning restore 0649

    private Vector3 startPosition;
    private float hopStartTime, hopHeight, hopWait, hopDuration, flipCooldown;

	void Start()
	{
        startPosition = transform.position;

        resetHop();
        hopStartTime = Time.time - Random.Range(-(hopDuration + hopWait), 0f);
        updateHop();

        spriteRenderer.sprite = sprites[Random.Range(0, sprites.Length)];
        spriteRenderer.sortingOrder += getNumberFromName();

        if (MathHelper.randomBool())
            flip();
        resetFlip();
	}

    void resetHop()
    {
        hopStartTime = Time.time;
        hopHeight = MathHelper.randomRangeFromVector(hopHeightRandomBounds);
        hopWait = MathHelper.randomRangeFromVector(hopWaitRandomBounds);
        hopDuration = MathHelper.randomRangeFromVector(hopDurationRandomBounds);
    }

    void resetFlip()
    {
        flip();
        flipCooldown = MathHelper.randomRangeFromVector(flipCooldownBounds);
    }
	
	void Update()
	{
        updateHop();
        updateFlip();
	}

    void updateHop()
    {
        float timeSinceHopStart = Time.time - hopStartTime,
            height = timeSinceHopStart <= hopDuration ? (hopCurve.Evaluate(timeSinceHopStart / hopDuration) * hopHeight) : 0f;

        transform.position = startPosition + (Vector3.up * height);

        if (timeSinceHopStart > hopDuration + hopWait)
            resetHop();
    }

    void updateFlip()
    {
        flipCooldown -= Time.deltaTime;
        if (flipCooldown <= 0f)
            resetFlip();
    }

    int getNumberFromName()
    {
        if (!name.Contains(")"))
            return 0;
        return int.Parse(name.Split('(')[1].Split(')')[0]);
    }

    void flip()
    {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }
}
