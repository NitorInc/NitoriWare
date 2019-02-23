using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;
using System.Linq;

public class FontCharDebugText : MonoBehaviour
{
    public string languageId;

    [SerializeField]
    private string charsFolderPath;
    [SerializeField]
    private bool useAppropriateLanguageFont = true;
	
	void Start()
    {
        var fullCharFolderPath = Path.Combine(Application.dataPath, charsFolderPath);
        
        var localizedTextComponent = GetComponent<LocalizedText>();
        string text;

        if (languageId.Equals("All") || languageId.Equals("NonAsian"))
            text = File.ReadAllText(Path.Combine(fullCharFolderPath, languageId + "Chars.txt"));
        else
        {
            var language = LanguagesData.instance.languages.FirstOrDefault(a => a.getLanguageID().Equals(languageId));

            if (useAppropriateLanguageFont && language.tmpFont != null)
            {
                localizedTextComponent.setTMPFont(language.tmpFont);
            }

            text = File.ReadAllText(Path.Combine(fullCharFolderPath, language.getFileName() + "Chars.txt"));
        }

        GetComponent<TMP_Text>().text = $"{languageId}: ({localizedTextComponent.TMPText.font.name})\n" + text;
    }
}
