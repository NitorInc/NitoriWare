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
    private ShadowType shadowType;

    private enum ShadowType
    {
        Cursor,
        Lantern,
        LanternCursorFade,
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
	}

    private void Update()
    {
        transform.position = transform.parent.position + initialPosition;
        transform.localScale = initialScale;
        setAlphas(initialAlpha, false);
    }

    void LateUpdate()
    {
        var mouseDiff = shadowType == ShadowType.Lantern
            ? (Vector2)(transform.position - DarkRoomLightEffect.lampTransformSingleton.position)
            : (Vector2)(transform.position - DarkRoomLightEffect.cursorTransformSingleton.position);

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
        transform.localScale *= scaleFactor;

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
