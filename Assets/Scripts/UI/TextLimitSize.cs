using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public abstract class TextLimitSize : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField]
    private Vector2 maxSize;
    [SerializeField]
    protected int defaultFontSize;
    [SerializeField]
    private bool onlyUpdateOnTextChange = true, resetSizeFields;
#pragma warning restore 0649

    private string lastText;
    
    void Start()
    {
        lastText = "";
    }
    
    protected virtual void determineSizeFields()
    {
        defaultFontSize = getFontSize();
        maxSize = getSize();
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

    public virtual void updateScale()
    {
        Vector2 sizeAtDefaultFont = getSize() * ((float)defaultFontSize / (float)getFontSize());
        Vector2 resizedSize = sizeAtDefaultFont;
        setFontSize(defaultFontSize);
        
        if (sizeAtDefaultFont.x > maxSize.x || sizeAtDefaultFont.y > maxSize.y)
        {
            do
            {
                float mult = Mathf.Min(maxSize.x / resizedSize.x, maxSize.y / resizedSize.y);
                setFontSize(Mathf.FloorToInt((float)getFontSize() * mult));
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

    protected abstract string getText();

    protected abstract int getFontSize();

    protected abstract void setFontSize(int fontSize);

    protected abstract Vector2 getSize();
}