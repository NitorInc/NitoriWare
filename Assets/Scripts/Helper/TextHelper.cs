using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TextHelper
{
	/// <summary>
	/// Gets localized text and checks for redundancies, use this for most purposes
	/// </summary>
	/// <param name="key"></param>
	/// <param name="defaultValue"></param>
	public static string getLocalizedText(string key, string defaultValue, LocalizedText.Parameter[] parameters = null)
    {
        string value = LocalizationManager.instance == null ? defaultValue : LocalizationManager.instance.getLocalizedValue(key, defaultValue);
        if (parameters != null && parameters.Length > 0)
        {
            string[] parameterStrings = new string[parameters.Length];
            for (int i = 0; i < parameters.Length; i++)
            {
                LocalizedText.Parameter parameter = parameters[i];
                parameterStrings[i] = parameter.isKey ? TextHelper.getLocalizedText(parameter.value, parameter.keyDefaultString) : parameter.value;
            }
            value = string.Format(value, parameterStrings);
        }
        return value;
    }

	/// <summary>
	/// Gets localized text with prefix "microgame.[ID]." added automatically
	/// </summary>
	/// <param name="key"></param>
	/// <param name="defaultValue"></param>
	/// <returns></returns>
	public static string getLocalizedMicrogameText(string key, string defaultValue, LocalizedText.Parameter[] parameters = null)
	{
        var microgameId = MicrogameController.instance.microgameId;
		return getLocalizedText("microgame." + microgameId + "." + key, defaultValue, parameters);
	}

	/// <summary>
	/// Returns the text without displaying warnings when a key isn't found, for debug purpose
	/// </summary>
	/// <param name="key"></param>
	/// <param name="defaultValue"></param>
	/// <returns></returns>
	public static string getLocalizedTextNoWarnings(string key, string defaultValue, LocalizedText.Parameter[] parameters = null)
	{
		string value = LocalizationManager.instance == null ? defaultValue : LocalizationManager.instance.getLocalizedValueNoWarnings(key, defaultValue);
        if (parameters != null && parameters.Length > 0)
        {
            string[] parameterStrings = new string[parameters.Length];
            for (int i = 0; i < parameters.Length; i++)
            {
                LocalizedText.Parameter parameter = parameters[i];
                parameterStrings[i] = parameter.isKey ? TextHelper.getLocalizedText(parameter.value, parameter.keyDefaultString) : parameter.value;
            }
            value = string.Format(value, parameterStrings);
        }
        return value;
    }

    /// <summary>
    /// Shortcut to LocalizationManager.instance.getLanguageID() with null check
    /// </summary>
    /// <returns></returns>
    public static string getLoadedLanguageID()
    {
        return LocalizationManager.instance == null ? "" : LocalizationManager.instance.getLoadedLanguageID();
    }

    /// <summary>
    /// Shortcut to LocalizationManager.instance.getLanguage() with null check
    /// </summary>
    /// <returns></returns>
    public static Language getLoadedLanguage()
    {
        return LocalizationManager.instance == null ? new Language() : LocalizationManager.instance.getLoadedLanguage();
    }
}
