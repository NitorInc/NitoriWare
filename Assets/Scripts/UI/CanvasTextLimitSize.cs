using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class CanvasTextLimitSize : TextLimitSize
{

#pragma warning disable 0649
    [SerializeField]
    private Text textComponent;
    [SerializeField]
    private RectTransform rectTransform;
#pragma warning restore 0649

    private CanvasTextOutline outline;  //Force update when size is changed

    void Awake()
    {
        if (textComponent == null)
            textComponent = GetComponent<Text>();
        if (rectTransform == null)
            rectTransform = (RectTransform)transform;
        if (outline == null)
            outline = GetComponent<CanvasTextOutline>();
    }

    public override void updateScale()
    {
        var fitter = GetComponent<ContentSizeFitter>();
        if (fitter != null)
        {
            fitter.SetLayoutHorizontal();
            fitter.SetLayoutVertical();
        }
        
        base.updateScale();
        if (outline != null)
        {
            var children = outline.getChildTexts();
            if (children != null)
            {
                outline.updateAttributes = true;
                outline.LateUpdate();
            }
        }
    }

    protected override string getText()
    {
        return textComponent.text;
    }

    protected override int getFontSize()
    {
        return textComponent.fontSize;
    }

    protected override void setFontSize(int fontSize)
    {
        textComponent.fontSize = fontSize;
    }

    protected override Vector2 getSize()
    {
        return rectTransform.sizeDelta;
    }
}
