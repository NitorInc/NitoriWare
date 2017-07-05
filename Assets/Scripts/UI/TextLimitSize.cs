using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public abstract class TextLimitSize : MonoBehaviour
{

#pragma warning disable 0649   //Serialized Fields
    [SerializeField]
    private Vector2 maxSize;
    [SerializeField]
    protected int defaultFontSize;
    [SerializeField]
    private bool onlyUpdateOnTextChange = true;
#pragma warning restore 0649

    private string lastText;
    
    void Start()
    {
        if (!Application.isPlaying)
        {
            defaultFontSize = getFontSize();
            maxSize = getSize();
        }
        lastText = "";
    }

    void LateUpdate()
    {
        if (!onlyUpdateOnTextChange || getText() != lastText)
            updateScale();
    }

    public virtual void updateScale()
    {
        Vector2 size = getSize() * ((float)defaultFontSize / (float)getFontSize());

        if (size.x > maxSize.x || size.y > maxSize.y)
            setFontSize((int)(Mathf.Floor(defaultFontSize * Mathf.Min(maxSize.x / size.x, maxSize.y / size.y))));
        else
            setFontSize(defaultFontSize);

        lastText = getText();
    }

    protected abstract string getText();

    protected abstract int getFontSize();

    protected abstract void setFontSize(int fontSize);

    protected abstract Vector2 getSize();
}