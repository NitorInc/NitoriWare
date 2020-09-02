using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[CreateAssetMenu(menuName = "Control/Languages Data")]
public class LanguagesData : ScriptableObjectSingleton<LanguagesData>
{
    [Header("Update this list manually.")]
    [Header("Update language content in Localization Updater.")]

    [SerializeField]
    private Language[] _languages;
    public Language[] languages => _languages;

    public Language FindLanguage(string language)
    {
        foreach (Language checkLanguage in languages)
        {
            if (checkLanguage.getLanguageID().Equals(language, System.StringComparison.OrdinalIgnoreCase))
                return checkLanguage;
        }
        Debug.Log("Language " + language + " not found. Using English");
        return languages[0];
    }

    public string getInLanguageName(string language)
    {
        return FindLanguage(language).languageName;
    }
}
