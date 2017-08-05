﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocalizedText : MonoBehaviour
{
	[SerializeField]
    private Prefix keyPrefix;
    [SerializeField]
    private bool applyToTextString = true;
    [SerializeField]
    private bool applyToFont = true;
	[SerializeField]
	private string _key;
    [SerializeField]
    private Parameter[] parameters;

	public string key
	{
		get {return _key;}
		set { _key = value; updateText(); }
    }

    private TextLimitSize limitSize;    //Force update when text is changed

    [System.Serializable]
    public struct Parameter
    {
        public string value;
        public bool isKey;
        public string keyDefaultString;
    }

    private Text textComponent;
	private TextMesh textMesh;
    private LocalizationManager.Language loadedLanguage;
    private string initialText;
    private Font initialFont;
    private FontStyle initialStyle;

	private enum Prefix
	{
		None,
		CurrentMicrogame
	}

	void Start ()
	{
		textComponent = GetComponent<Text>();
		textMesh = GetComponent<TextMesh>();
        limitSize = GetComponent<TextLimitSize>();
        loadedLanguage = new LocalizationManager.Language();
        initialText = getText();
        updateText();
        initialStyle = getStyle();
        initialFont = getFont();
    }

    private void LateUpdate()
    {
        if (loadedLanguage.getLanguageID() != TextHelper.getLoadedLanguageID())
        {
            bool updateAttributes = !string.IsNullOrEmpty(loadedLanguage.getLanguageID());
            loadedLanguage = TextHelper.getLoadedLanguage();
            if (applyToTextString)
            {
                setText(initialText);
                updateText();
            }
            if (applyToFont && shouldChangeFont())
            {
                updateStyle();
                updateFont();
            }

            if (updateAttributes)
                updateTextEffects();
        }
    }

    bool shouldChangeFont()
    {
        if (!applyToTextString)
            return true;
        else
            return !getText().Equals(initialText);
    }

    /// <summary>
    /// Sets the key to load from and reloads the text with the new key
    /// </summary>
    /// <param name="key"></param>
    public void setKey(string key)
	{
		this._key = key;
		updateText();
	}

	public void updateText()
	{
        if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(loadedLanguage.getLanguageID()))
            return;

		string value;
		if (keyPrefix == Prefix.CurrentMicrogame)
			value = TextHelper.getLocalizedMicrogameText(key, getText(), parameters);
		else
			value = TextHelper.getLocalizedText(getPrefixedKey(), getText(), parameters);

		setText(value);

        //if (limitSize != null)
        //{
        //    var component = GetComponent<TextLimitSize>();
        //    component.updateScale();
        //    //if (textComponent != null)
        //    //    ((CanvasTextLimitSize)limitSize).updateScale();
        //    //else if (textMesh != null)
        //    //    ((TextMeshLimitSize)limitSize).updateScale();
        //}
    }

    public void updateFont()
    {
        setFont(loadedLanguage.overrideFont == null ? initialFont : loadedLanguage.overrideFont);
    }

    public void updateStyle()
    {
        if (loadedLanguage.forceUnbold)
        {
            bool italicized = initialStyle == FontStyle.Italic || initialStyle == FontStyle.BoldAndItalic;
            setStyle(italicized ? FontStyle.Italic : FontStyle.Normal);
        }
    }

	private void setText(string text)
	{
		if (textComponent != null)
			textComponent.text = text;
		else if (textMesh != null)
			textMesh.text = text;
	}

	private string getText()
	{
		if (textComponent != null)
			return textComponent.text;
		if (textMesh != null)
			return textMesh.text;
		return "";
    }

    private void setFont(Font font)
    {
        if (textComponent != null)
            textComponent.font = font;
        else if (textMesh != null)
            textMesh.font = font;
    }

    private Font getFont()
    {
        if (textComponent != null)
            return textComponent.font;
        if (textMesh != null)
            return textMesh.font;
        return null;
    }

    public FontStyle getStyle()
    {
        if (textComponent != null)
            return textComponent.fontStyle;
        if (textMesh != null)
            return textMesh.fontStyle;
        return FontStyle.Normal;
    }

    public void setStyle(FontStyle style)
    {
        if (textComponent != null)
            textComponent.fontStyle = style;
        else if (textMesh != null)
            textMesh.fontStyle = style;
    }

	string getPrefixedKey()
	{
		switch(keyPrefix)
		{
			//Handled seperately
			//case (Prefix.CurrentMicrogame):
			//	return "microgame." + gameObject.scene.name.Substring(0, gameObject.scene.name.Length - 1) + ".";
			default:
				return key;
		}
	}

    void updateTextEffects()
    {
        //TODO Save me TextMesh Pro

        if (textComponent != null)
        {
            //var fitter = GetComponent<CanvasTextLimitSize>();
            //if (fitter != null)
            //    fitter.updateScale();
            var outline = GetComponent<CanvasTextOutline>();
            if (outline != null)
            {
                outline.updateAttributes = true;
                outline.LateUpdate();
            }
        }
        if (textMesh != null)
        {
            var fitter = GetComponent<TextMeshLimitSize>();
            if (fitter != null)
                fitter.updateScale();
            var outline = GetComponent<TextOutline>();
         //   if (outline != null)
         //       outline.LateUpdate();
        }

    }
}
