using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class LanguageDropdown : MonoBehaviour
{

#pragma warning disable 0649   //Serialized Fields
    [SerializeField]
    private Dropdown dropdown;
#pragma warning restore 0649

    string[] languageFilenames;

    void Start()
    {
        if (LocalizationManager.instance == null)
        {
            enabled = false;
            return;
        }

        LocalizationManager.Language[] languages = LocalizationManager.instance.getAllLanguages();
        languages = (from language in languages where !language.disableSelect select language).ToArray();   //Narrow down to selectable languages

        languageFilenames = (from language in languages select language.getLanguageID()).ToArray();
        dropdown.ClearOptions();
        dropdown.AddOptions((from language in languages select new Dropdown.OptionData(language.languageName)).ToList());

        dropdown.value = findLanguageIndex(PrefsHelper.getPreferredLanguage());
    }

    int findLanguageIndex(string fileName)
    {
        var languages = LocalizationManager.instance.getAllLanguages();
        for (int i = 0; i < languages.Length; i++)
        {
            if (languages[i].getLanguageID() == fileName)
                return i;
        }
        return 0;
    }

    public void select(int item)
    {
        if (LocalizationManager.instance == null)
            return;

        string language = languageFilenames[item];
        if (language != TextHelper.getLoadedLanguage())
        {
            LocalizationManager.instance.setLanguage(language);
        }
    }
}
