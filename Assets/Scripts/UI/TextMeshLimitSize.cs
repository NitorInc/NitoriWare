using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class TextMeshLimitSize : TextLimitSize
{

#pragma warning disable 0649
    [SerializeField]
    private bool useWorldScaling;
    [SerializeField]
    private TextMesh textMesh;
    [SerializeField]
    private Renderer textRenderer;
#pragma warning restore 0649

    private TextOutline outline;    //Force update when resized

    void Awake()
    {
        if (textMesh == null)
            textMesh = GetComponent<TextMesh>();
        if (textRenderer == null)
            textRenderer = textMesh.GetComponent<Renderer>();
        if (outline == null)
            outline = GetComponent<TextOutline>();
    }

    public override void updateScale()
    {
        setFontSize(defaultFontSize);
        base.updateScale();
    }

    protected override string getText()
    {
        return textMesh.text;
    }

    protected override int getFontSize()
    {
        return textMesh.fontSize;
    }

    protected override void setFontSize(int fontSize)
    {
        textMesh.fontSize = fontSize;
    }

    protected override Vector2 getSize()
    {
        if (useWorldScaling)
            return textRenderer.bounds.extents * 2f;
        else
        {
            Vector2 size = textRenderer.bounds.extents * 2f,
                scaleMult = transform.lossyScale;
            if (scaleMult.x == 0f || scaleMult.y == 0f)
                return Vector2.zero;
            size.x /= scaleMult.x;
            size.y /= scaleMult.y;
            return size;
        }
    }
}
