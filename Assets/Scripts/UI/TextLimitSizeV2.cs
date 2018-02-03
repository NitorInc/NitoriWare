using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class TextLimitSizeV2 : MonoBehaviour
{
    [SerializeField]
    private Text textComponent;
    [SerializeField]
    private RectTransform rectTransform;
    [SerializeField]
    private RectTransform boundingBox;
    [SerializeField]
    protected int defaultFontSize;
    [SerializeField]
    private bool onlyUpdateOnTextChange = true;
    [SerializeField]
    private bool resetSizeFields;

    private string lastText;

    void Start()
    {
        lastText = "";

        if (textComponent == null)
            textComponent = GetComponent<Text>();
        if (rectTransform == null)
            rectTransform = (RectTransform)transform;
    }

    protected virtual void determineSizeFields()
    {
        defaultFontSize = getFontSize();
    }

    void LateUpdate()
    {
        if (resetSizeFields)
        {
            determineSizeFields();
            resetSizeFields = false;
        }
        if (!onlyUpdateOnTextChange || getText() != lastText)
            updateScale();
    }

    public void updateScale()
    {
        Vector2 sizeAtDefaultFont = getSize() * ((float)defaultFontSize / (float)getFontSize());
        Vector2 resizedSize = sizeAtDefaultFont;
        Vector2 maxSize = getMaxSize();
        setFontSize(defaultFontSize);

        if (sizeAtDefaultFont.x > maxSize.x || sizeAtDefaultFont.y > maxSize.y)
        {
            do
            {
                float mult = Mathf.Min(maxSize.x / resizedSize.x, maxSize.y / resizedSize.y);
                setFontSize(Mathf.FloorToInt((float)getFontSize() * mult));
                //Debug.Log(name + " fit");
                resizedSize *= mult;
            }
            while (resizedSize.x > maxSize.x + .001f || resizedSize.y > maxSize.y + .001f);
        }
        else
        {
            setFontSize(defaultFontSize);
        }

        lastText = getText();
    }

    Vector2 getMaxSize()
    {
        return boundingBox.sizeDelta;
    }

    string getText()
    {
        return textComponent.text;
    }

    int getFontSize()
    {
        return textComponent.fontSize;
    }

    void setFontSize(int fontSize)
    {
        textComponent.fontSize = fontSize;
    }

    Vector2 getSize()
    {
        return rectTransform.sizeDelta;
    }
}