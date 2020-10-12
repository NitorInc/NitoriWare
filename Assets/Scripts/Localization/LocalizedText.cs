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
    private Parameter[] parameters = { };

    public string key
    {
        get { return _key; }
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
    [Multiline]
    private string tmproFontFallbackList = "";
    [SerializeField]
    [Multiline]
    private string tmproFontBlacklist = "";

    private Text textComponent;
    public Text TextComponent => textComponent;
    private TextMesh textMesh;
    public TextMesh TextMeshComponent => textMesh;
    private TMP_Text tmpText;
    public TMP_Text TMPText => tmpText;


    private Language loadedLanguage;
    private string initialText;
    private LocalizedTextFontData initialFontData;
    private LocalizedTextFontData currentFontData;

    private class LocalizedTextFontData
    {
        public Font font;
        public TMP_FontAsset tmpFontAsset;
        public FontStyle fontStyle;
        public FontStyles tmpFontStyle;
    }


    private enum Prefix
    {
        None,
        CurrentMicrogame
    }

    private void Awake()
    {
        textComponent = GetComponent<Text>();
        textMesh = GetComponent<TextMesh>();
        tmpText = GetComponent<TMP_Text>();
        loadedLanguage = null;
        initialText = getText();

        initialFontData = getFontData();
        currentFontData = getFontData();
    }

    void Start()
    {
        if (TextHelper.getLoadedLanguage() != null)
            UpdateLanguage(TextHelper.getLoadedLanguage());
        LocalizationManager.onLanguageChanged += UpdateLanguage;
    }

    private void OnDestroy()
    {
        LocalizationManager.onLanguageChanged -= UpdateLanguage;
    }

    private void UpdateLanguage(Language language)
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
                    setFontFromData(initialFontData);
                    setStyleFromData(initialFontData);
                }
            }

            if (updateAttributes)
                updateTextEffects();
        }
    }

    private LocalizedTextFontData getFontData()
    {
        var newFont = new LocalizedTextFontData();

        if (textComponent != null)
        {
            newFont.font = textComponent.font;
            newFont.fontStyle = textComponent.fontStyle;
        }
        else if (textMesh != null)
        {
            newFont.font = textMesh.font;
            newFont.fontStyle = textMesh.fontStyle;
        }
        if (tmpText != null)
        {
            newFont.tmpFontAsset = tmpText.font;
            newFont.tmpFontStyle = tmpText.fontStyle;
        }

        return newFont;
    }

    bool shouldChangeFont() => true;
    //bool shouldChangeFont() => !applyToTextString || !getText().Equals(initialText);

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

    public void updateStyle()
    {
        if (loadedLanguage.forceUnbold)
        {
            // Subtract Bold enum data if it's being used
            if (textComponent != null || textMesh != null)  // Normal font
            {
                if (initialFontData.fontStyle == FontStyle.Bold || initialFontData.fontStyle == FontStyle.BoldAndItalic)
                    setTextStyle(initialFontData.fontStyle - (int)FontStyle.Bold);
            }
            if (tmpText != null && tmpText.isUsingBold)    // TMP font
            {
                if (tmpText.isUsingBold)
                    setTMPStyle(tmpText.fontStyle - (int)FontStyles.Bold);
            }
        }
        else
            setStyleFromData(initialFontData);
    }

    public void setText(string text)
    {
        if (textComponent != null)
            textComponent.text = text;
        else if (textMesh != null)
            textMesh.text = text;
        if (tmpText != null)
            tmpText.text = text;
        SendMessage("OnTextLocalized", options: SendMessageOptions.DontRequireReceiver);
    }

    public string getText()
    {
        if (textComponent != null)
            return textComponent.text;
        if (textMesh != null)
            return textMesh.text;
        if (tmpText != null)
            return tmpText.text;
        return null;
    }

    public void updateFont()
    {
        setTextFont(getFontForLanguage(loadedLanguage));

        setTMPFont(getTMProFontForLanguage(loadedLanguage));
    }

    public Font getFontForLanguage(Language language)
    {
        return loadedLanguage.overrideFont == null ? initialFontData.font : loadedLanguage.overrideFont;
    }

    public TMP_FontAsset getTMProFontForLanguage(Language language)
    {
        if (initialFontData.tmpFontAsset != null && LocalizationManager.instance.isTMPFontCompatibleWithLanguage(initialFontData.tmpFontAsset.name))
            return initialFontData.tmpFontAsset;
        
        foreach (var fallbackFont in tmproFontFallbackList.Split('\n'))
        {
            if (LocalizationManager.instance.isTMPFontCompatibleWithLanguage(fallbackFont))
            {
                var fontData = LocalizationManager.instance.loadedFonts
                    .FirstOrDefault(a => a.fontAsset.name.Equals(fallbackFont));
                if (fontData != null)
                    return fontData.fontAsset;
            }
        }

        var fallback = LocalizationManager.instance.getFallBackFontForCurrentLanguage(blacklist:tmproFontBlacklist.Split('\n'));
        if (fallback != null)
            return fallback;

        if (tmpText != null)
            return tmpText.font;

        return null;
    }

    private void setFontFromData(LocalizedTextFontData fontData)
    {
        setTextFont(fontData.font);
        setTMPFont(fontData.tmpFontAsset);
    }

    public void setTextFont(Font font)
    {
        if (textComponent != null)
            textComponent.font = font;
        else if (textMesh != null)
            textMesh.font = font;
        else
            return;
        currentFontData.font = font;
        SendMessage("OnFontLocalized", options: SendMessageOptions.DontRequireReceiver);
    }

    public void setTMPFont(TMP_FontAsset fontAsset)
    {
        if (tmpText == null)
            return;

        // Save the font material before we change fonts
        var fontMaterial = tmpText.fontMaterial;

        tmpText.font = fontAsset;

        // Now to preserve the Material Preset, we have to apply the current font material's texture to the saved material
        fontMaterial.SetTexture("_MainTex", tmpText.font.material.mainTexture);
        // And set the fontMaterial back to the saved one
        tmpText.fontMaterial = fontMaterial;
        // It's just what we gotta do

        currentFontData.tmpFontAsset = fontAsset;
        SendMessage("OnFontLocalized", options: SendMessageOptions.DontRequireReceiver);
    }

    private void setStyleFromData(LocalizedTextFontData fontData)
    {
        setTextStyle(fontData.fontStyle);
        setTMPStyle(fontData.tmpFontStyle);
    }

    public void setTextStyle(FontStyle style)
    {
        if (textComponent != null)
            textComponent.fontStyle = style;
        else if (textMesh != null)
            textMesh.fontStyle = style;
        currentFontData.fontStyle = style;
    }

    public void setTMPStyle(FontStyles tmpStyle)
    {
        if (tmpText != null)
            tmpText.fontStyle = tmpStyle;
        currentFontData.tmpFontStyle = tmpStyle;
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
