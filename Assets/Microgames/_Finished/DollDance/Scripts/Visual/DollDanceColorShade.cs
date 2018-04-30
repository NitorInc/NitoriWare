using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class DollDanceColorShade : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer[] spriteRenderers;
    public float lightness = 1f;

    private float lastLightness;

    void Start ()
    {
        updateColor();
	}

    private void LateUpdate()
    {
        if (lightness != lastLightness)
            updateColor();
    }

    public void updateColor()
    {
        foreach (SpriteRenderer spriteRenderer in spriteRenderers)
        {
            var color = spriteRenderer.color;
            color.r = color.g = color.b = lightness;
            spriteRenderer.color = color;
        }
        lastLightness = lightness;
    }
}
