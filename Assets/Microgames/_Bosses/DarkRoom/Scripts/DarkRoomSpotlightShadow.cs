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
    private bool isCusorLight;
    [SerializeField]
    private bool isLantern;

    Vector3 initialPosition;
    float initialAlpha;
    Vector3 initialScale;
    private SpriteRenderer shadowRenderer;

    void Start ()
    {
        shadowRenderer = GetComponent<SpriteRenderer>();
        initialPosition = transform.localPosition;
        initialAlpha = shadowRenderer.color.a;
        initialScale = transform.localScale;
	}
	
	void Update ()
    {
        var mouseDiff = (Vector2)(transform.position - DarkRoomLightEffect.cursorTransformSingleton.position);
        var mouseDist = mouseDiff.magnitude;
        var cursorMult = 1f + DarkRoomEffectAnimationController.instance.cursorBoost;
        cursorMult = Mathf.Clamp01(cursorMult);
        var lanternMult = 1f + DarkRoomEffectAnimationController.instance.lampBoost;
        lanternMult = Mathf.Clamp01(lanternMult);

        var position = initialPosition;
        var addPos = (Vector3)mouseDiff.resize(mouseDist * cameraDistancePosMult);
        addPos.y *= yPosMult;
        position += addPos;
        transform.localPosition = position;

        var scale = initialScale;
        var scaleFactor = 1f + (mouseDist * cameraDistanceScaleMult);
        transform.localScale = scale * scaleFactor;

        var alpha = initialAlpha;
        alpha += mouseDist * cameraDistanceAlphaMult;
        alpha = Mathf.Min(alpha, maxAlpha);

        if (isLantern)
        {
            alpha = Mathf.Lerp(maxAlpha, alpha, cursorMult);
            alpha *= lanternMult;
        }
        if (isCusorLight)
            alpha *= cursorMult;

        var c = shadowRenderer.color;
        c.a = alpha;
        shadowRenderer.color = c;
        foreach (var rend in inheritAlphaRenderers)
        {
            c = rend.color;
            c.a = alpha;
            rend.color = c;
        }
    }
}
