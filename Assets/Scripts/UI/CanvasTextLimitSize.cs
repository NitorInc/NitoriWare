using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class CanvasTextLimitSize : TextLimitSize
{

    private static bool canvasesUpdated;

#pragma warning disable 0649   //Serialized Fields
    [SerializeField]
    private Text textComponent;
    [SerializeField]
    private RectTransform rectTransform;
#pragma warning restore 0649

    void Awake()
    {
        canvasesUpdated = false;
        if (textComponent == null)
            textComponent = GetComponent<Text>();
        if (rectTransform == null)
            rectTransform = (RectTransform)transform;
    }


    void Update()
    {
        canvasesUpdated = false;
    }

    public override void updateScale()
    {
        if (!canvasesUpdated)
        {
            Canvas.ForceUpdateCanvases();
            canvasesUpdated = true;
        }
        base.updateScale();
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
