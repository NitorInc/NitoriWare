using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Runtime.InteropServices.WindowsRuntime;

[CreateAssetMenu(menuName = "Control/Languages Data")]
public class LanguagesData : ScriptableObjectSingleton<LanguagesData>
{
    [Header("Update this list manually.")]
    [Header("Update language content in Localization Updater.")]

    [SerializeField]
    private Language[] _languages;
    public Language[] languages => _languages;

    public Language FindLanguage(string language, bool defaultToEnglish = true)
    {
        foreach (Language checkLanguage in languages)
        {
            if (checkLanguage.getLanguageID().Equals(language, System.StringComparison.OrdinalIgnoreCase))
                return checkLanguage;
        }
        if (defaultToEnglish)
        {
            Debug.Log("Language " + language + " not found. Using English");
            return languages[0];
        }
        else
            return null;
    }

    public string getInLanguageName(string language)
    {
        return FindLanguage(language).languageName;
    }
}
