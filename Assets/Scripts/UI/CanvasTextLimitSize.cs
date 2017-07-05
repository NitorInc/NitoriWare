using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class CanvasTextLimitSize : MonoBehaviour
{
    private static bool canvasUpdated;

#pragma warning disable 0649   //Serialized Fields
    [SerializeField]
    private Vector2 maxSize;
    [SerializeField]
    private Text textComponent;
    [SerializeField]
    private RectTransform rectTransform;
    [SerializeField]
    private int defaultFontSize;
    [SerializeField]
    private bool onlyUpdateOnTextChange = true;
#pragma warning restore 0649

    private string lastText;

    void Awake()
    {
        canvasUpdated = false;
        if (textComponent == null)
            textComponent = GetComponent<Text>();
        if (rectTransform == null)
             rectTransform = (RectTransform)transform;
        if (maxSize == Vector2.zero)
            maxSize = rectTransform.sizeDelta;

        if (!Application.isPlaying)
        {
            if (textComponent != null)
                defaultFontSize = textComponent.fontSize;
            else
                enabled = false;
        }
    }

    void Start()
    {
        lastText = "";
    }

    void Update()
    {
        canvasUpdated = false;
    }

    void LateUpdate()
    {
        if (onlyUpdateOnTextChange && textComponent.text == lastText)
            return;

        if (!canvasUpdated)
        {
            Canvas.ForceUpdateCanvases();
            canvasUpdated = true;
        }
        updateScale();
    }

    public void updateScale()
    {
        Vector2 unscaledSize = rectTransform.sizeDelta * (float)defaultFontSize / (float)textComponent.fontSize;

        if (unscaledSize.x > maxSize.x || unscaledSize.y > maxSize.y)
            textComponent.fontSize = (int)(Mathf.Floor(defaultFontSize * Mathf.Min(maxSize.x / unscaledSize.x, maxSize.y / unscaledSize.y)));
        else
            textComponent.fontSize = defaultFontSize;

        lastText = textComponent.text;
    }
}