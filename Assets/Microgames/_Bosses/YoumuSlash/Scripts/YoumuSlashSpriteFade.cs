using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YoumuSlashSpriteFade : MonoBehaviour
{
    [SerializeField]
    private YoumuSlashTimingData timingData;

    private float currentSpeed = 0f;
    private SpriteRenderer fadeRenderer;
    
	void Start ()
    {
        fadeRenderer = GetComponent<SpriteRenderer>();
	}
	
	void Update ()
    {
        if (currentSpeed != 0f)
        {
            float goalAlpha = currentSpeed > 0f ? 1f : 0f;
            float newAlpha = Mathf.MoveTowards(getAlpha(), goalAlpha, Time.deltaTime * Mathf.Abs(currentSpeed));
            setAlpha(newAlpha);
            if (newAlpha == goalAlpha)
                currentSpeed = 0f;
        }
	}

    public void fadeIn(float beatDuration)
    {
        setFade(1f, beatDuration);
    }

    public void fadeOut(float beatDuration)
    {
        setFade(-1f, beatDuration);
    }

    void setFade(float direction, float beatDuration)
    {
        if (beatDuration > 0f)
        {
            float totalDuration = beatDuration * timingData.BeatDuration;
            float alphaDiff = direction > 0f ? (1f - getAlpha()) : getAlpha();
            currentSpeed = (alphaDiff / totalDuration) * direction;
        }
        else
            setAlpha(direction > 0f ? 1f : 0f);
    }

    float getAlpha()
    {
        return fadeRenderer.color.a;
    }

    void setAlpha(float alpha)
    {
        Color color = fadeRenderer.color;
        color.a = alpha;
        fadeRenderer.color = color;
    }
}
