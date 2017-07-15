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
    [SerializeField]
    private Sprite failSprite;
#pragma warning restore 0649

    private Vector3 startPosition;
    private float hopStartTime, hopHeight, hopWait, hopDuration, flipCooldown;
    private int victoryStatus = 0;

	void Start()
	{
        startPosition = transform.position;

        resetHop();
        hopStartTime = Time.time - Random.Range(0f, hopDuration + hopWait);
        updateHop();

        spriteRenderer.sprite = sprites[Random.Range(0, sprites.Length)];
        spriteRenderer.sortingOrder += getNumberFromName();

        if (MathHelper.randomBool())
            flip();
        resetFlip();
	}

    void resetHop()
    {
        if (victoryStatus == -1)
            hopHeightRandomBounds = Vector2.zero;

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
        if (victoryStatus == 0)
            checkVictoryStatus();
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

    void checkVictoryStatus()
    {
        if (MicrogameController.instance.getVictoryDetermined())
        {
            if (MicrogameController.instance.getVictory())
            {
                hopWaitRandomBounds = Vector2.zero;
                hopWait = 0f;
                hopHeightRandomBounds *= 1.5f;
                flipCooldownBounds /= 2f;
                flipCooldown /= 2f;
                //if (Time.time - hopStartTime >= hopDuration)
                //    resetHop();
                victoryStatus = 1;
            }
            else
            {
                float brightness = .5f;
                spriteRenderer.color = new Color(brightness, brightness, brightness, 1f);
                if (Time.time - hopStartTime <= hopDuration)
                {
                    hopHeight = transform.position.y - startPosition.y;
                    hopStartTime = Time.time - (hopDuration / 2f);
                }
                else
                {
                    enabled = false;
                }
                flipCooldown = float.PositiveInfinity;
                victoryStatus = -1;
                spriteRenderer.sprite = failSprite;
            }
        }
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
