[System.Serializable]
public class LocalizationLanguage
{
	public LocalizationLanguageItem[] items;
}

[System.Serializable]
public class LocalizationLanguageItem
{
	public string key;
	public string value;
}