using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;
using System.Linq;
using UnityEngine.UI;

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

            if (useAppropriateLanguageFont)
            {
                if (language.tmpFont != null)
                {
                    localizedTextComponent.setTMPFont(language.tmpFont);
                }
                if (language.overrideFont != null)
                {
                    localizedTextComponent.setTextFont(language.overrideFont);
                }
            }
            
            text = File.ReadAllText(Path.Combine(fullCharFolderPath, language.getFileName() + "Chars.txt"));
        }

        string fontName = "";
        if (localizedTextComponent.TextComponent != null)
        {
            fontName = localizedTextComponent.TextComponent.font.name;
            GetComponent<Text>().text = $"{languageId}: ({fontName})\n" + text;
        }
        if (localizedTextComponent.TMPText != null)
        {
            fontName = localizedTextComponent.TMPText.font.name;
            GetComponent<TMP_Text>().text = $"{languageId}: ({fontName})\n" + text;
        }
        
    }
}
