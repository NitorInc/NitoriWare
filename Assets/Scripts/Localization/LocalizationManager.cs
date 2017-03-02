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

	private Dictionary<string, string> localizedText;

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
		loadLocalizedText("Languages/" + language + ".json");
	}

	
	public void loadLocalizedText(string filename)
	{
		localizedText = new Dictionary<string, string>();
		string filePath = Path.Combine(Application.streamingAssetsPath, filename);
		if (!File.Exists(filePath))
		{
			filePath = filePath.Replace(language, "English");
			Debug.Log("Language " + language + " not found. Using English");
		}
		if (File.Exists(filePath))
		{
			string jsonString = File.ReadAllText(filePath);
			LocalizationData loadedData = JsonUtility.FromJson<LocalizationData>(jsonString);
			for (int i = 0; i < loadedData.items.Length; i++)
			{
				localizedText.Add(loadedData.items[i].key, loadedData.items[i].value);
			}
			Debug.Log("Language loaded successfully: " + language);
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
		if (!localizedText.ContainsKey(key))
		{
			Debug.LogWarning("Language " + getLanguage() + " does not have a value for key " + key);
			return defaultString;
		}
		return TextHelper.fillInParameters(localizedText[key]);
	}

	public string getLocalizedValueNoWarnings(string key, string defaultString)
	{
		return (localizedText == null) ? defaultString : (localizedText.ContainsKey(key) ? TextHelper.fillInParameters(localizedText[key]) : defaultString);
	}
}