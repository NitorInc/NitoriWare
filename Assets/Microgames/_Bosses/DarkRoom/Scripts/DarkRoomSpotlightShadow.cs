using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkRoomSpotlightShadow : MonoBehaviour
{
    [SerializeField]
    private float cameraDistancePosMult = .1f;
    [SerializeField]
    private float cameraDistanceScaleMult = .1f;
    [SerializeField]
    private float cameraDistanceAlphaMult = -.1f;
    [SerializeField]
    private float yPosMult = .2f;
    [SerializeField]
    private float maxAlpha = 1f;
    [SerializeField]
    private SpriteRenderer[] inheritAlphaRenderers;
    [SerializeField]
    private bool yOnly;
    [SerializeField]
    private ShadowType shadowType;
    [SerializeField]
    private Transform overrideOriginPoint;
    [SerializeField]
    private float flickerFrequencyMult = 8f;
    [SerializeField]
    private float flickerAmpMult = 0f;
    [SerializeField]
    private float flickerLossFadeSpeed = .1f;
    [SerializeField]
    private SpriteRenderer matchSprite;

    private enum ShadowType
    {
        Cursor,
        Lantern,
        LanternCursorFade,
        PulseOnly
    }

    Vector3 initialPosition;
    float initialAlpha;
    Vector3 initialScale;
    private SpriteRenderer shadowRenderer;

    void Start ()
    {
        shadowRenderer = GetComponent<SpriteRenderer>();
        initialPosition = transform.position - transform.parent.position;
        initialAlpha = shadowRenderer.color.a;
        initialScale = transform.localScale;

        if (shadowType == ShadowType.Cursor)
            flickerAmpMult = 0f;
	}

    private void Update()
    {
        transform.position = transform.parent.position + initialPosition;
        transform.localScale = initialScale;
        setAlphas(initialAlpha, false);
    }

    void LateUpdate()
    {
        if (MicrogameController.instance.getVictoryDetermined() && !MicrogameController.instance.getVictory())
            flickerAmpMult = Mathf.MoveTowards(flickerAmpMult, 0f, Time.deltaTime * flickerLossFadeSpeed);
        if (matchSprite != null)
            shadowRenderer.sprite = matchSprite.sprite;

        var compTransform = overrideOriginPoint != null ? overrideOriginPoint : transform.parent;
        var mouseDiff = shadowType == ShadowType.Lantern
            ? (Vector2)(compTransform.position - DarkRoomLightEffect.lampTransformSingleton.position)
            : (Vector2)(compTransform.parent.position - DarkRoomLightEffect.cursorTransformSingleton.position);
        if (yOnly)
            mouseDiff.x = 0f;
        if (shadowType == ShadowType.PulseOnly)
            mouseDiff = Vector2.zero;
        
        //var t = Time.timeSinceLevelLoad * 3f * flickerFrequencyMult;
        //var a = -Mathf.Sin(t);
        //mouseDiff = mouseDiff.resize(mouseDiff.magnitude + (a * flickerAmpMult));

        var mouseDist = mouseDiff.magnitude;
        var cursorMult = 1f + DarkRoomEffectAnimationController.instance.cursorBoost;
        cursorMult = Mathf.Clamp01(cursorMult);
        var lanternMult = 1f + DarkRoomEffectAnimationController.instance.lampBoost;
        lanternMult = Mathf.Clamp01(lanternMult);

        var position = initialPosition;
        var addPos = (Vector3)mouseDiff.resize(mouseDist * cameraDistancePosMult);
        addPos.y *= yPosMult;
        position += addPos;
        transform.position += position - initialPosition;

        var scale = initialScale;
        var scaleFactor = 1f + (mouseDist * cameraDistanceScaleMult);
        var t = Time.timeSinceLevelLoad * 3f * flickerFrequencyMult;
        var a = -Mathf.Sin(t);
        scaleFactor += (a * flickerAmpMult);
        var holdX = transform.localScale.x;
        transform.localScale *= scaleFactor;
        if (yOnly)
            transform.localScale = new Vector3(holdX, transform.localScale.y, transform.localScale.z);

        var alpha = initialAlpha;
        alpha += mouseDist * cameraDistanceAlphaMult;
        alpha = Mathf.Min(alpha, maxAlpha);

        if (shadowType == ShadowType.LanternCursorFade)
        {
            alpha = Mathf.Lerp(maxAlpha, alpha, cursorMult);
            alpha *= lanternMult;
        }
        if (shadowType == ShadowType.Cursor)
            alpha *= cursorMult;
        setAlphas(alpha - initialAlpha, true);
    }

    void setAlphas(float alpha, bool add)
    {
        var c = shadowRenderer.color;
        if (add)
            c.a += alpha;
        else
            c.a = alpha;
        shadowRenderer.color = c;
        foreach (var rend in inheritAlphaRenderers)
        {
            c = rend.color;
            if (add)
                c.a += alpha;
            else
                c.a = alpha;
            rend.color = c;
        }
    }
}
