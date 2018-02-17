using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MamiPoserKoishi : MonoBehaviour {
    // Koishi's fade-out animation

    [Header("Delay before Koishi starts fading out")]
    [SerializeField]
    private float fadeDelay = 0.4f;

    [Header("Fade duration in seconds")]
    [SerializeField]
    private float fadeDuration = 0.4f;

    private bool animationStarted = false;
    private bool animationFinished = false;

    private float animationStartTime;

    void Update () {
        if (animationFinished)
            return;

        if (!animationStarted)
        {
            // Animation not started yet; check if it should be started
            if (GetComponent<MamiPoserCharacter>().isChoseWrongExpression)
            {
                animationStarted = true;
                animationStartTime = Time.time;
            }
            return;
        }

        // Calculate the time since Koishi was clicked
        float timeDelta = Time.time - animationStartTime;

        if (timeDelta < fadeDelay)
        {
            // Delay before starting the animation
            return;
        }

        float alpha;
        if (timeDelta < fadeDelay + fadeDuration)
        {
            // Animation in progress; calculate Koishi's transparency
            alpha = 1 - (timeDelta - fadeDelay) / fadeDuration;
        }
        else
        {
            // Animation finished; make Koishi fully transparent
            alpha = 0;
            animationFinished = true;
        }

        // Set alpha for all the child sprites
        foreach (SpriteRenderer sprite in GetComponentsInChildren<SpriteRenderer>())
        {
            Color color = sprite.color;
            color.a = alpha;
            sprite.color = color;
        }
    }
}
