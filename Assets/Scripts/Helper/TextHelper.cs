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
	public static string getLocalizedText(string key, string defaultValue)
	{
		return LocalizationManager.instance == null ? defaultValue : LocalizationManager.instance.getLocalizedValue(key, defaultValue);
	}

	/// <summary>
	/// Gets localized text with prefix "microgame.[ID]," added automatically
	/// </summary>
	/// <param name="key"></param>
	/// <param name="defaultValue"></param>
	/// <returns></returns>
	public static string getLocalizedMicrogameText(string key, string defaultValue)
	{
		Scene scene = MicrogameController.instance.gameObject.scene;
		return getLocalizedText("microgame." + scene.name.Substring(0, scene.name.Length - 1) + "." + key, defaultValue);
	}
}
