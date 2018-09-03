using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class LanguageDropdown : MonoBehaviour
{

#pragma warning disable 0649
    [SerializeField]
    private Dropdown dropdown;
#pragma warning restore 0649

    private Language[] languages;
    private string[] languageFilenames;

    void Start()
    {
        if (LocalizationManager.instance == null)
        {
            enabled = false;
            return;
        }

        languages = LanguagesData.instance.languages;
        languages = (from language in languages where !language.disableSelect select language).ToArray();   //Narrow down to selectable languages and sort alphabetically

        languageFilenames = (from language in languages select language.getLanguageID()).ToArray();
        dropdown.ClearOptions();
        dropdown.AddOptions((from language in languages select new Dropdown.OptionData(language.languageName)).ToList());

        dropdown.value = findLanguageIndex(PrefsHelper.getPreferredLanguage());
    }

    int findLanguageIndex(string fileName)
    {
        //TODO Clean this shit up, better way of handling non-selectable languages
        var currentLanguage = LanguagesData.instance.FindLanguage(fileName);
        if (currentLanguage.disableSelect && !string.IsNullOrEmpty(currentLanguage.getLanguageID()))
        {
            for (int i = 0; i < languages.Length; i++)
            {
                if (languages[i].overrideFileName.Equals(currentLanguage.getLanguageID()))
                {
                    Debug.Log(languages[i].getLanguageID());
                    currentLanguage = languages[i];
                    break;
                }
            }
        }

        for (int i = 0; i < languages.Length; i++)
        {
            if (languages[i].getLanguageID() == currentLanguage.getLanguageID())
                return i;
        }
        return 0;
    }

    public void select(int item)
    {
        if (LocalizationManager.instance == null)
            return;

        string language = languageFilenames[item];
        if (language != TextHelper.getLoadedLanguageID())
        {
            LocalizationManager.instance.setLanguage(language);
        }
    }
}
