using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;

public class UIFontCharTester : MonoBehaviour
{
    [SerializeField]
    private Font font;
    [SerializeField]
    private string charsPath;


    void Start()
    {
        // Format is {id, filename}
        var languageInfos = LanguagesData.instance.languages.Select(a => new string[]{ a.getLanguageID(), a.getFileName()}).ToList();
        languageInfos.Add(new string[] { "NonAsian", "NonAsian" });
        languageInfos.Add(new string[] { "All", "All" });

        foreach (var languageInfo in languageInfos)
        {
            var charString = File.ReadAllText(Path.Combine(Application.dataPath, charsPath, $"{languageInfo[1]}Chars.txt"));
            var missingChars = charString.Where(a => !font.HasCharacter(a));
            //var missingChars = charString.Where(a => !font.characterInfo.Where(aa => aa.index == (int)a).Any());
            if (missingChars.Any())
            {
                Debug.LogWarning($"{languageInfo[0]}: {font.name} has missing characters: {string.Join("", missingChars)}");
            }
            else
            {
                Debug.Log($"{languageInfo[0]}: {font.name} has no missing characters!");
            }
        }
    }
}
