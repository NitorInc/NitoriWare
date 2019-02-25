using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using UnityEngine.Serialization;

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

    [System.Serializable]
    public struct Parameter
    {
        public string value;
        public bool isKey;
        public string keyDefaultString;
    }

    [FormerlySerializedAs("defaultTMProFallbackFont")]
    [SerializeField]
    private TMP_FontAsset defaultTmpFont;

    [FormerlySerializedAs("TMProFallbackOverrideFonts")]
    [SerializeField]
    private TMP_FontOverride[] tmpFontOverrides;
    
    [System.Serializable]
    public class TMP_FontOverride
    {
        [SerializeField]
        [Multiline]
        private string languages;
        public string Languages => languages;

        [FormerlySerializedAs("fallback")]
        [SerializeField]
        private TMP_FontAsset font;
        public TMP_FontAsset Font => font;

        [SerializeField]
        private bool useOverrideFontStyle;
        public bool UseOverrideFontStyle => useOverrideFontStyle;
        [SerializeField]
        private FontStyles overrideFontStyle;
        public FontStyles OverrideFontStyle => overrideFontStyle;
    }

    private Text textComponent;
	private TextMesh textMesh;
    private TMP_Text tmpText;
    private Language loadedLanguage;
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
        tmpText = GetComponent<TMP_Text>();
        loadedLanguage = null;
        initialText = getText();
        initialStyle = getStyle();
        initialFont = getFont();
        updateText();
    }

    private void LateUpdate()
    {
        if (loadedLanguage?.getLanguageID() != TextHelper.getLoadedLanguageID()
            && !(string.IsNullOrEmpty(loadedLanguage?.getLanguageID()) && string.IsNullOrEmpty(TextHelper.getLoadedLanguageID())))
        {
            bool updateAttributes = !string.IsNullOrEmpty(loadedLanguage?.getLanguageID());
            loadedLanguage = TextHelper.getLoadedLanguage();
            if (applyToTextString)
            {
                setText(initialText);
                updateText();
            }
            if (applyToFont)
            {
                if (shouldChangeFont())
                {
                    updateStyle();
                    updateFont();
                }
                else
                {
                    setStyle(initialStyle);
                    setFont(initialFont);
                }
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
		_key = key;
		updateText();
	}

	public void updateText()
	{
        if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(loadedLanguage?.getLanguageID()))
            return;

		string value;
		if (keyPrefix == Prefix.CurrentMicrogame)
			value = TextHelper.getLocalizedMicrogameText(key, getText(), parameters);
		else
			value = TextHelper.getLocalizedText(getPrefixedKey(), getText(), parameters);

		setText(value);
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
        if (tmpText != null)
            tmpText.text = text;

        SendMessage("OnTextLocalized", options: SendMessageOptions.DontRequireReceiver);
    }

	private string getText()
	{
		if (textComponent != null)
			return textComponent.text;
		if (textMesh != null)
			return textMesh.text;
        if (tmpText != null)
            return tmpText.text;

        return "";
    }

    private void setFont(Font font)
    {
        if (textComponent != null)
            textComponent.font = font;
        else if (textMesh != null)
            textMesh.font = font;
        if (tmpText  != null)
            setTMPFont();

        SendMessage("OnFontLocalized", options: SendMessageOptions.DontRequireReceiver);
    }

    private Font getFont()
    {
        if (textComponent != null)
            return textComponent.font;
        if (textMesh != null)
            return textMesh.font;
        //if (textMeshPro != null)
        //    return textMeshPro.font;
        //if (textMeshProUGUI != null)
        //    return textMeshProUGUI.font;
        return null;
    }

    void setTMPFont()
    {
        var font = getTMProFont();
        if (font != null)
        {
            // Save the font material before we change fonts
            var fontMaterial = tmpText.fontMaterial;
            
            tmpText.font = font;
            
            // Now to preserve the Material Preset, we have to apply the current font material's texture to the saved material
            fontMaterial.SetTexture("_MainTex", tmpText.font.material.mainTexture);
            // And set the fontMaterial back to the saved one
            tmpText.fontMaterial = fontMaterial;
            // It's just what we gotta do
        }
    }

    TMP_FontAsset getTMProFont()
    {
        var loadedLanguage = TextHelper.getLoadedLanguage();
        foreach (var fontOverride in tmpFontOverrides)
        {
            if (fontOverride.Languages.Contains(loadedLanguage.getLanguageID()))
            {
                if (fontOverride.UseOverrideFontStyle)
                {

                    if (tmpText != null)
                        tmpText.fontStyle = fontOverride.OverrideFontStyle;
                }

                return fontOverride.Font;
            }
        }

        if (loadedLanguage.tmpFont != null)
            return loadedLanguage.tmpFont;

        if (defaultTmpFont != null)
            return defaultTmpFont;

        return null;
    }

    public FontStyle getStyle()
    {
        if (textComponent != null)
            return textComponent.fontStyle;
        if (textMesh != null)
            return textMesh.fontStyle;
        //TODO TextMeshPro fontstyle support
        return FontStyle.Normal;
    }

    public void setStyle(FontStyle style)
    {
        if (textComponent != null)
            textComponent.fontStyle = style;
        else if (textMesh != null)
            textMesh.fontStyle = style;
        //TODO TextMeshPro fontstyle support
    }

    string getPrefixedKey() => key;

    void updateTextEffects()
    {
        //TODO Save me TextMesh Pro

        if (textComponent != null)
        {
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
        }

    }
}
