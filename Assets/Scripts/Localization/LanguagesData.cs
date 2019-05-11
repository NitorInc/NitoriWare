﻿using System.Collections;
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

    [System.Serializable]
    public class LanguageTMPFont
    {
        public string idName;
        public TMP_FontAsset fontAsset;
    }

    [SerializeField]
    private LanguageTMPFont[] _languageTMPFonts;
    public LanguageTMPFont[] languageTMPFonts => _languageTMPFonts;

    public Language FindLanguage(string language)
    {
        foreach (Language checklanguage in languages)
        {
            if (checklanguage.getLanguageID().Equals(language, System.StringComparison.OrdinalIgnoreCase))
                return checklanguage;
        }
        Debug.Log("Language " + language + " not found. Using English");
        return languages[0];
    }

    public string getInLanguageName(string language)
    {
        return FindLanguage(language).languageName;
    }
}
