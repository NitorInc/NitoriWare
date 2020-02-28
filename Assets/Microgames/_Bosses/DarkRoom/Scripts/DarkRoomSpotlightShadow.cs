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
        var mouseDiff = (Vector2)(transform.position - CameraHelper.getCursorPosition());
        var mouseDist = mouseDiff.magnitude;

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
        var c = shadowRenderer.color;
        c.a = alpha;
        shadowRenderer.color = c;
    }
}
