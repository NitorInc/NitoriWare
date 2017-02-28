using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LocalizationManager : MonoBehaviour
{
	public const string NotFoundString = "LOCALIZED TEXT NOT FOUND";

	public static LocalizationManager instance;

	[SerializeField]
	private string _fileName;

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

		loadLocalizedText(_fileName);
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
			Debug.LogError("Cannot find file " + filename + " in StreamingAssets");
	}

	public string getLocalizedValue(string key)
	{
		return localizedText.ContainsKey(key) ? localizedText[key] : NotFoundString;
	}
}