using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class TextMeshLimitSize : TextLimitSize
{

#pragma warning disable 0649   //Serialized Fields
    [SerializeField]
    private bool useWorldScaling;
    [SerializeField]
    private TextMesh textMesh;
    [SerializeField]
    private Renderer textRenderer;
#pragma warning restore 0649

	void Awake()
	{
        if (textMesh == null)
            textMesh = GetComponent<TextMesh>();
        if (textRenderer == null)
            textRenderer = textMesh.GetComponent<Renderer>();
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
            size.x /= scaleMult.x;
            size.y /= scaleMult.y;
            return size;
        }
    }
}
