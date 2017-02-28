using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LocalizationManager : MonoBehaviour
{
	private const string NotFoundString = "LOCALIZED TEXT NOT FOUND";

	public static LocalizationManager instance;

	[SerializeField]
	private string language;

	private Dictionary<string, string> localizedText;

	void Awake ()
	{
		//Hourai Elixir
		if (instance != null && instance != this)
		{
			Destroy(gameObject);       
			return;
		}
		else
			instance = this;
		DontDestroyOnLoad(gameObject);

		if (!language.Equals(""))
			loadLocalizedText("Languages/" + language + ".json");
	}

	
	public void loadLocalizedText(string filename)
	{
		localizedText = new Dictionary<string, string>();
		string filePath = Path.Combine(Application.streamingAssetsPath, filename);
		if (File.Exists(filePath))
		{
			string jsonString = File.ReadAllText(filePath);
			LocalizationData loadedData = JsonUtility.FromJson<LocalizationData>(jsonString);
			for (int i = 0; i < loadedData.items.Length; i++)
			{
				localizedText.Add(loadedData.items[i].key, loadedData.items[i].value);
			}
		}
		else
			Debug.LogError("Cannot find language" + filename + " in StreamingAssets/Languages/");

	}

	public string getLocalizedValue(string key)
	{
		return (localizedText == null) ? "No language set" : getLocalizedValue(key, NotFoundString);
	}

	public string getLocalizedValue(string key, string defaultString)
	{
		return (localizedText == null) ? defaultString : (localizedText.ContainsKey(key) ? localizedText[key] : defaultString);
	}
}