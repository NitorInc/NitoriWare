using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;

public class TextHelper
{
	private static Dictionary<string, string> variables;

	/// <summary>
	/// Gets localized text and checks for redundancies, use this for most purposes
	/// </summary>
	/// <param name="key"></param>
	/// <param name="defaultValue"></param>
	public static string getLocalizedText(string key, string defaultValue)
	{
		if (key.StartsWith("var."))
			return getVariable(key, defaultValue);
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

	/// <summary>
	/// Returns the text without displaying warnings when a key isn't found, for debug purpose
	/// </summary>
	/// <param name="key"></param>
	/// <param name="defaultValue"></param>
	/// <returns></returns>
	public static string getLocalizedTextNoWarnings(string key, string defaultValue)
	{
		return LocalizationManager.instance == null ? defaultValue : LocalizationManager.instance.getLocalizedValueNoWarnings(key, defaultValue);
	}

	/// <summary>
	/// Replaces all instances of text formatted {key} with their appropriate values, works with variables
	/// </summary>
	/// <param name="text"></param>
	/// <returns></returns>
	public static string fillInParameters(string text)
	{
		if (variables == null)
			variables = new Dictionary<string, string>();

		int tries = 100;
		while (true)
		{
			string match = Regex.Match(text, @"\{[\w\.]+\}").ToString();

			if (!string.IsNullOrEmpty(match) && tries > 0)
			{
				match = match.Replace("{", "").Replace("}", "");
				if (match.StartsWith("var."))
					text = text.Replace(match, getVariable(match, "(TEXT NOT FOUND)"));
				else
					text = text.Replace(match, getLocalizedText(match, "(TEXT NOT FOUND)"));
			}
			else
				break;

			tries--;
		}
		return text;
	}

	public static void setVariable(string key, string value)
	{
		if (variables.ContainsKey(key))
			variables[key] = value;
		else
			variables.Add(key, value);
	}

	/// <summary>
	/// Returns the text variable with the given key, must be set beforehand or els the default value will be returned. Can contain key parameters formatted {key}
	/// </summary>
	/// <param name="key"></param>
	/// <param name="defaultValue"></param>
	/// <returns></returns>
	public static string getVariable(string key, string defaultValue)
	{
		return fillInParameters(variables.ContainsKey(key) ? variables[key] : defaultValue);
	}
}
