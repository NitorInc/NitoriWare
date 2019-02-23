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
        string charString;

        if (languageId.Equals("All") || languageId.Equals("NonAsian"))
            charString = File.ReadAllText(Path.Combine(fullCharFolderPath, languageId + "Chars.txt"));
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
            
            charString = File.ReadAllText(Path.Combine(fullCharFolderPath, language.getFileName() + "Chars.txt"));
        }

        string fontName = "";
        if (localizedTextComponent.TextComponent != null)
        {
            fontName = localizedTextComponent.TextComponent.font.name;
            GetComponent<Text>().text = $"{languageId}: ({fontName})\n" + charString;
        }
        if (localizedTextComponent.TMPText != null)
        {
            fontName = localizedTextComponent.TMPText.font.name;
            GetComponent<TMP_Text>().text = $"{languageId}: ({fontName})\n" + charString;

            var fontAsset = localizedTextComponent.TMPText.font;

            var missingChars = new List<char>();
            if (!fontAsset.HasCharacters(charString, out missingChars))
            {
                for (int i = 0; i < missingChars.Count; i++)
                {
                    var chr = missingChars[i];
                    foreach (var fallback in fontAsset.fallbackFontAssets)
                    {
                        // angry message to the future WHY DOESN'T HASCHARACTER WORK ON FALLBACKS??
                        if (fallback.characterDictionary.ContainsKey((int)chr))
                        {
                            missingChars.RemoveAt(i);
                            i--;
                            break;
                        }
                    }
                }
            }
            if (missingChars.Any())
            {
                Debug.LogWarning($"{languageId}: {fontName} has missing characters: {string.Join("", missingChars)}");
            }
        }
        
    }
}
