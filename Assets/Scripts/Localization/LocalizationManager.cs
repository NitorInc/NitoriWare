using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LocalizationManager : MonoBehaviour
{
	private const string NotFoundString = "LOCALIZED TEXT NOT FOUND";
    public const string DefaultLanguage = "English";

	public static LocalizationManager instance;

    [SerializeField]
    private Language[] languages;
    [SerializeField]
    private string forceLanguage;
    
    private Language loadedLanguage;
	private SerializedNestedStrings localizedText;
    private string languageString;

    [System.Serializable]
    public struct Language
    {
        [SerializeField]
        private string languageID;
        public string languageName;
        public bool incomplete;
        public bool disableSelect;
        public string overrideFileName;
        public Font overrideFont;

        public string getFileName()
        {
            return string.IsNullOrEmpty(overrideFileName) ? languageID : overrideFileName;
        }

        public string getLanguageID()
        {
            return languageID;
        }
    }

	public void Awake ()
    {
        loadedLanguage = new Language();
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
        forceLanguage = language;
    }
    
	public void setLanguage(string language)
	{
        StartCoroutine(loadLanguage(checkForLanguage(language)));
    }

    Language checkForLanguage(string language)
    {
        foreach (Language checklanguage in languages)
        {
            if (checklanguage.getLanguageID().Equals(language, System.StringComparison.OrdinalIgnoreCase))
                return checklanguage;
        }
        Debug.Log("Language " + language + " not found. Using English");
        return languages[0];
    }

    public string getInLanguageName(string language)
    {
        return checkForLanguage(language).languageName;
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
        Debug.Log("Language " + language.getLanguageID() + " loaded in " + timeElapsed.TotalMilliseconds + "ms");
        PrefsHelper.setPreferredLanguage(language.getLanguageID());

        loadedLanguage = language;
        languageString = "";
    }

    public Language[] getAllLanguages()
    {
        return languages;
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