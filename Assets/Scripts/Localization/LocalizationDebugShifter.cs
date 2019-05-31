using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LocalizationDebugShifter : MonoBehaviour {

    int languageCycleIndex = -1;
    void Update ()
    {
		if (Input.GetKeyDown(KeyCode.L))
        {
            var manager = LocalizationManager.instance;

            if (languageCycleIndex < 0)
                languageCycleIndex = LanguagesData.instance.languages
                    .Select(a => a.getFileName())
                    .ToList()
                    .IndexOf(manager.getLoadedLanguageID());

            var languages = LanguagesData.instance.languages;
            var currentLanguageName = languages[languageCycleIndex++].getLanguageID();
            if (LocalizationManager.instance != null)
                manager.setLanguage(currentLanguageName);
            else
                manager.setForcedLanguage(currentLanguageName);
            if (languageCycleIndex >= languages.Count())
                languageCycleIndex = 0;
            print("Language cycled to " + currentLanguageName);
        }
    }

    /// <summary>
    /// Loads the language directly after the currently loaded language, loops back around when it reaches the end
    /// </summary>
    public void shiftLanguage()
    {

    }
}
