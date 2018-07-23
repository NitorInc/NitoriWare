using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

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

    [SerializeField]
    private TMP_FontAsset defaultTMProFallbackFont;
    [SerializeField]
    private FallbackOverride[] TMProFallbackOverrideFonts;

    [System.Serializable]
    public class FallbackOverride
    {
        [SerializeField]
        [Multiline]
        private string languages;
        public string Languages => languages;

        [SerializeField]
        private TMP_FontAsset fallback;
        public TMP_FontAsset Fallback => fallback;
    }

    private Text textComponent;
	private TextMesh textMesh;
    private TextMeshPro textMeshPro;
    private TextMeshProUGUI textMeshProUGUI;
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
        textMeshPro = GetComponent<TextMeshPro>();
        textMeshProUGUI = GetComponent<TextMeshProUGUI>();
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
        if (textMeshPro != null)
            textMeshPro.text = text;
        if (textMeshProUGUI != null)
            textMeshProUGUI.text = text;
    }

	private string getText()
	{
		if (textComponent != null)
			return textComponent.text;
		if (textMesh != null)
			return textMesh.text;
        if (textMeshPro != null)
            return textMeshPro.text;
        if (textMeshProUGUI != null)
            return textMeshProUGUI.text;

        return "";
    }

    private void setFont(Font font)
    {
        if (textComponent != null)
            textComponent.font = font;
        else if (textMesh != null)
            textMesh.font = font;
        if (textMeshPro != null)
            setTMPFontFallback(textMeshPro.font);
        if (textMeshProUGUI != null)
            setTMPFontFallback(textMeshProUGUI.font);
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

    void setTMPFontFallback(TMP_FontAsset font)
    {
        var fallback = getTMProFallback();
        if (fallback != null && !font.fallbackFontAssets.Contains(fallback))
        {
            if (!LocalizationManager.instance.modifiedFallbacks.ContainsKey(font))
                LocalizationManager.instance.modifiedFallbacks.Add(font, new List<TMP_FontAsset>(font.fallbackFontAssets));
            else
                font.fallbackFontAssets = new List<TMP_FontAsset>(LocalizationManager.instance.modifiedFallbacks[font]);
            font.fallbackFontAssets.Add(fallback);
        }
    }

    TMP_FontAsset getTMProFallback()
    {
        var loadedLanguage = TextHelper.getLoadedLanguage();
        foreach (var fallbackOverride in TMProFallbackOverrideFonts)
        {
            if (fallbackOverride.Languages.Split('\n').Contains(loadedLanguage.languageName))
            {
                return fallbackOverride.Fallback;
            }
        }

        if (loadedLanguage.tmproFallback != null)
            return loadedLanguage.tmproFallback;

        if (defaultTMProFallbackFont != null)
            return defaultTMProFallbackFont;

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
