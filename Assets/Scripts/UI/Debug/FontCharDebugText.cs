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
	
	public void Initiate()
    {
        var fullCharFolderPath = Path.Combine(Application.dataPath, charsFolderPath);
        string charString;

        if (languageId.Equals("All") || languageId.Equals("NonAsian"))
            charString = File.ReadAllText(Path.Combine(fullCharFolderPath, languageId + "Chars.txt"));
        else
        {
            var language = LanguagesData.instance.languages.FirstOrDefault(a => a.getLanguageID().Equals(languageId));    
            charString = File.ReadAllText(Path.Combine(fullCharFolderPath, language.getFileName() + "Chars.txt"));
        }

        var textComponent = GetComponent<Text>();
        var font = textComponent.font;
        
        textComponent.text = $"{languageId}: ({font.name})\n" + charString;

        string missingChars = "";

#if UNITY_EDITOR

        missingChars = new string(
            charString
                .Where(a => !font.HasCharacter(a))
                .ToArray());

#endif

        if (missingChars.Length > 0)
            Debug.LogWarning($"{languageId}: {font.name} has missing characters: {string.Join("", missingChars)}");
        else
            Debug.Log($"{languageId}: {font.name} has no missing characters.");
    }
}
