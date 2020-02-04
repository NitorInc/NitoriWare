﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;
using System.Linq;

public class LocalizationManager : MonoBehaviour
{
	private const string NotFoundString = "LOCALIZED TEXT NOT FOUND";
    public const string DefaultLanguage = "English";

	public static LocalizationManager instance;

    [SerializeField]
    private string forceLanguage;
    [SerializeField]
    private string fontAssetsDirectory;

    public delegate void LanguageChangedDelegate(Language language);
    public static LanguageChangedDelegate onLanguageChanged;

    private Language loadedLanguage;
	private SerializedNestedStrings localizedText;
    private string languageString;
    private SerializedNestedStrings.StringData languageFontMetadata;
    private bool loadedLanguageIsComplete;
    public bool isLoadedLanguageComplete() => loadedLanguageIsComplete;

	public void Awake ()
    {
		if (instance != null)
		{
			if (instance != this)
				Destroy(gameObject);
			return;
		}
		else
			instance = this;
		if (transform.parent == null)
			DontDestroyOnLoad(gameObject);

        onLanguageChanged = null;
        loadedLanguage = new Language();

        string languageToLoad;
        string preferredLanguage = PrefsHelper.getPreferredLanguage();
        if (!string.IsNullOrEmpty(forceLanguage))
            languageToLoad = forceLanguage;
        else if (!string.IsNullOrEmpty(preferredLanguage))
            languageToLoad = preferredLanguage;
        else
            languageToLoad = Application.systemLanguage.ToString();
		setLanguage(languageToLoad);
	}

    public void setForcedLanguage(string language)
    {
        print("setting forced language to " + language);
        forceLanguage = language;
    }

    public string getForcedLanguage() => forceLanguage;
    
	public void setLanguage(string language)
	{
        print("setting language to " + language);
        var lang = LanguagesData.instance.FindLanguage(language);
        StartCoroutine(loadLanguage(lang));
    }

    IEnumerator loadLanguage(Language language)
    {
        System.DateTime started = System.DateTime.Now;
        string filePath = Path.Combine(Application.streamingAssetsPath, "Languages/" + language.getFileName());
        languageString = "";
        if (filePath.Contains("://"))
        {
            WWW www = new WWW(filePath);
            yield return www;
            languageString = www.text;
        }
        else
            languageString = File.ReadAllText(filePath);

        localizedText = SerializedNestedStrings.deserialize(languageString);

        System.TimeSpan timeElapsed = System.DateTime.Now - started;
        Debug.Log("Language " + language.getFileName() + " loaded in " + timeElapsed.TotalMilliseconds + "ms");
        PrefsHelper.setPreferredLanguage(language.getLanguageID());
        languageFontMetadata = localizedText.getSubData("meta.font");

        loadedLanguageIsComplete = false;
        loadedLanguage = language;
        languageString = "";
        loadedLanguageIsComplete = getLocalizedValue("generic.complete", "N").Equals("Y", System.StringComparison.OrdinalIgnoreCase);

        if (onLanguageChanged != null)
            onLanguageChanged(language);
    }

    public string getLoadedLanguageID()
	{
		return string.IsNullOrEmpty(loadedLanguage.getFileName()) ? "" : loadedLanguage.getFileName();
	}

    public Language getLoadedLanguage()
    {
        return loadedLanguage;
    }

	public string getLocalizedValue(string key)
	{
		return (localizedText == null) ? "No language set" : getLocalizedValue(key, NotFoundString);
	}

	public string getLocalizedValue(string key, string defaultString)
	{
		if (localizedText == null) 
			return defaultString;
		string value = (string)localizedText[key];
		if (string.IsNullOrEmpty(value))
		{
            if (loadedLanguageIsComplete)
			Debug.LogWarning("Language " + getLoadedLanguageID() + " is marked as complete but does not have a value for key " + key);
			    return defaultString;
		}
		value = value.Replace("\\n", "\n");
		return value;
	}

	public string getLocalizedValueNoWarnings(string key, string defaultString)
	{
		if (localizedText == null)
			return defaultString;
		string value = (string)localizedText[key];
		if (string.IsNullOrEmpty(value))
			return defaultString;
        value = value.Replace("\\n", "\n");
		return value;
	}

    public static bool parseFontCompabilityString(Language language, string value)
    {
        if (string.IsNullOrEmpty(value))
            return false;

        value = value.ToUpper();
        if (language.getLanguageID().Equals("ChineseSimplified")
            || language.getLanguageID().Equals("Chinese"))
        {
            return value.Equals("S") || value.Equals("Y");
        }
        else if (language.getLanguageID().Equals("ChineseTraditional"))
        {
            return value.Equals("T") || value.Equals("Y");
        }
        else
            return value.Equals("Y");
    }

    public bool isTMPFontCompatibleWithLanguage(TMP_FontAsset font)
    {
        var languageFont = LanguagesData.instance.languageTMPFonts.FirstOrDefault(a => a.fontAsset == font);
        if (languageFont == null)
            return false;
        if (languageFontMetadata.subData.ContainsKey(languageFont.idName))
            return parseFontCompabilityString(loadedLanguage, languageFontMetadata.subData[languageFont.idName].value);
        else
            return false;
    }
    

    public TMP_FontAsset getFallBackFontForCurrentLanguage(TMP_FontAsset[] blacklist = null)
    {
        if (blacklist == null)
            blacklist = new TMP_FontAsset[0];

        return LanguagesData.instance.languageTMPFonts
            .FirstOrDefault(a =>
                a.fontAsset != null
                && !blacklist.Contains(a.fontAsset)
                && languageFontMetadata.subData.ContainsKey(a.idName)
                && parseFontCompabilityString(loadedLanguage, languageFontMetadata.subData[a.idName].value))
            .fontAsset;
    }

}