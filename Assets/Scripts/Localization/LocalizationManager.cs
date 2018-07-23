using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;

public class LocalizationManager : MonoBehaviour
{
	private const string NotFoundString = "LOCALIZED TEXT NOT FOUND";
    public const string DefaultLanguage = "English";

	public static LocalizationManager instance;

    public Dictionary<TMP_FontAsset, List<TMP_FontAsset>> modifiedFallbacks;

    [SerializeField]
    private string forceLanguage;
    
    private Language loadedLanguage;
	private SerializedNestedStrings localizedText;
    private string languageString;

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

        modifiedFallbacks = new Dictionary<TMP_FontAsset, List<TMP_FontAsset>>();
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

    private void OnDestroy()
    {
        foreach (var fallbackPair in modifiedFallbacks)
        {
            fallbackPair.Key.fallbackFontAssets = fallbackPair.Value;
        }
        modifiedFallbacks = null;
    }

    public void setForcedLanguage(string language)
    {
        forceLanguage = language;
    }
    
	public void setLanguage(string language)
	{
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

        loadedLanguage = language;
        languageString = "";
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
            if (!loadedLanguage.incomplete)
			Debug.LogWarning("Language " + getLoadedLanguageID() + " is not marked as incomplete but does not have a value for key " + key);
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
}