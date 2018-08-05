using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class YoumuSlashAlphaControl : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer[] spriteRenderers;
    [Range(0f, 1f)]
    public float alpha = 1f;

    private float lastAlpha;

    void Start()
    {
        updateAlpha();
    }

    private void LateUpdate()
    {
        if (alpha != lastAlpha)
            updateAlpha();
    }

    public void updateAlpha()
    {
        foreach (SpriteRenderer spriteRenderer in spriteRenderers)
        {
            var color = spriteRenderer.color;
            color.a = alpha;
            spriteRenderer.color = color;
        }
        lastAlpha = alpha;
    }
}