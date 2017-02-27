using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LocalizationManager : MonoBehaviour
{
	public static LocalizationManager instance;

	private Dictionary<string, string> localizedText;

	void Awake ()
	{
		//Hourai Elixir
		if (instance != null && instance != this)
			Destroy(gameObject);
		else
			instance = this;
		DontDestroyOnLoad(gameObject);
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
}
