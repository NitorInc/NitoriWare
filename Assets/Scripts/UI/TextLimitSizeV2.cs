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

    ContentSizeFitter fitter;
    private CanvasTextOutline outline;  //Force update when size is changed

    private string lastText;

    void Start()
    {
        lastText = "";

        if (textComponent == null)
            textComponent = GetComponent<Text>();
        if (rectTransform == null)
            rectTransform = (RectTransform)transform;
        if (outline == null)
            outline = GetComponent<CanvasTextOutline>();
        if (fitter == null)
            fitter = GetComponent<ContentSizeFitter>();
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
        {
            updateScale();

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

    }

    public void updateScale()
    {
        setFontSize(defaultFontSize);

        if (fitter != null)
        {
            if (fitter.horizontalFit == ContentSizeFitter.FitMode.PreferredSize)
                fitter.SetLayoutHorizontal();
            if (fitter.verticalFit == ContentSizeFitter.FitMode.PreferredSize)
                fitter.SetLayoutVertical();
        }
        

        //Vector2 sizeAtDefaultFont = getSize() * ((float)defaultFontSize / (float)getFontSize());
        var generator = new TextGenerator();
        var generatorSettings = textComponent.GetGenerationSettings(rectTransform.rect.size);
        Vector2 sizeAtDefaultFont = new Vector2(generator.GetPreferredWidth(getText(), generatorSettings),
            generator.GetPreferredHeight(getText(), generatorSettings));

        //if (fitter != null)
        //{
        //    if (fitter.horizontalFit != ContentSizeFitter.FitMode.PreferredSize)
        //    {
        //        sizeAtDefaultFont.x = 0f;
        //        print(sizeAtDefaultFont);
        //    }
        //    if (fitter.verticalFit != ContentSizeFitter.FitMode.PreferredSize)
        //        sizeAtDefaultFont.y = 0f;
        //}
        //print(sizeAtDefaultFont);

        Vector2 resizedSize = sizeAtDefaultFont;
        Vector2 maxSize = getMaxSize();

        //setFontSize(defaultFontSize);

        if (sizeAtDefaultFont != Vector2.zero && (sizeAtDefaultFont.x > maxSize.x || sizeAtDefaultFont.y > maxSize.y))
        {
            do
            {
                float mult = Mathf.Min(maxSize.x / resizedSize.x, maxSize.y / resizedSize.y);
                setFontSize(Mathf.FloorToInt((float)getFontSize() * mult) - 1);
                //Debug.Log(name + " fit");
                resizedSize *= mult;
                //resizedSize = getSize();
                //print(resizedSize);
                //print(getSize());
                //print(maxSize);
            }
            while (resizedSize.x > maxSize.x + .001f || resizedSize.y > maxSize.y + .001f);
        }
        //else
        //{
        //    setFontSize(defaultFontSize);
        //}

        lastText = getText();
    }

    Vector2 getMaxSize()
    {
        var boundSize = boundingBox.sizeDelta;
        if (fitter != null)
        {
            if (fitter.horizontalFit != ContentSizeFitter.FitMode.PreferredSize)
                boundSize.x = Mathf.Infinity;
            if (fitter.verticalFit != ContentSizeFitter.FitMode.PreferredSize)
                boundSize.y = Mathf.Infinity;
        }
        return boundSize;
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