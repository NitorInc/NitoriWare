using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LocalizationManager : MonoBehaviour
{
	private const string NotFoundString = "LOCALIZED TEXT NOT FOUND";

	public static LocalizationManager instance;

	[SerializeField]
	private string forceLanguage;
	private string language;

	private SerializedNestedStrings localizedText;

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

		language = (forceLanguage == "" ? Application.systemLanguage.ToString() : forceLanguage);
		loadLocalizedText("Languages/" + language + "");
	}

    public void setForcedLanguage(string language)
    {
        forceLanguage = language;
    }

	
	public void loadLocalizedText(string filename)
	{
		string filePath = Path.Combine(Application.streamingAssetsPath, filename);
		if (!File.Exists(filePath))
		{
			filePath = filePath.Replace(language, "English");
			Debug.Log("Language " + language + " not found. Using English");
			language = "English";
		}
		if (File.Exists(filePath))
		{
			System.DateTime started = System.DateTime.Now;
			localizedText = SerializedNestedStrings.deserialize(File.ReadAllText(filePath));
			System.TimeSpan timeElapsed = System.DateTime.Now - started;
			Debug.Log("Language " + language + " loaded successfully. Deserialization time: " + timeElapsed.TotalMilliseconds + "ms");
		}
		else
			Debug.LogError("No English json found!");

	}

	public string getLanguage()
	{
		return language;
	}

	public void setLanguage(string language)
	{
		//TODO make loading async and revisit this
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
			Debug.LogWarning("Language " + getLanguage() + " does not have a value for key " + key);
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
		if (key.Split('.')[0].Equals("multiline"))
			value = value.Replace("\\n", "\n");
		return value;
	}
}