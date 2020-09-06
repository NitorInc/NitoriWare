using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class FontCharDebugTextGen : MonoBehaviour
{
    [Header("---Analyze font compatibility with languages---")]
    [SerializeField]
    private Font fontToTest;
    [Header("-----------------------------------------------")]

    [Header("---Analyze language compatibility with fonts---")]
    [SerializeField]
    private string languageIdToTest;
    [Header("-----------------------------------------------")]

    [SerializeField]
    private float textXSeparation;
    [SerializeField]
    private GameObject textPrefab;
    
	void Start ()
    {
        var languageIds = new List<string>();
        languageIds.Add("NonAsian");
        languageIds.AddRange(LanguagesData.instance.languages
            //.OrderByDescending(a => a.isAsian)
            .Select(a => a.getLanguageID())
            .ToList());
        languageIds.Add("All");

        if (fontToTest != null)
        {
            SetFontDynamic(fontToTest, false);
            for (int i = 0; i < languageIds.Count; i++)
            {
                var languageId = languageIds[i];
                var newText = Instantiate(textPrefab, transform.position + (Vector3.right * textXSeparation * i), Quaternion.identity);
                newText.transform.position = transform.position + (Vector3.right * textXSeparation * i);
                newText.GetComponent<Text>().font = fontToTest;
                newText.transform.SetParent(transform);
                newText.GetComponent<FontCharDebugText>().languageId = languageId;
                newText.name = languageId;
                newText.GetComponent<FontCharDebugText>().Initiate();
            }
            SetFontDynamic(fontToTest, true);
        }
        else if (!string.IsNullOrEmpty(languageIdToTest))
        {
            var language = LanguagesData.instance.FindLanguage(languageIdToTest);
            if (language != null)
            {
                for (int i = 0; i < TMPFontsData.instance.fonts.Length; i++)
                {
                    var fontData = TMPFontsData.instance.fonts[i];
                    var fontAsset = fontData.LoadFontAsset();
                    SetFontDynamic(fontAsset.sourceFontFile, false);
                    if (fontAsset.sourceFontFile == null)
                    {
                        Debug.LogWarning("font " + fontData.assetName + " has no base font");
                        continue;
                    }
                    var newText = Instantiate(textPrefab, transform.position + (Vector3.right * textXSeparation * i), Quaternion.identity);
                    newText.transform.position = transform.position + (Vector3.right * textXSeparation * i);
                    newText.GetComponent<Text>().font = fontAsset.sourceFontFile;
                    newText.transform.SetParent(transform);
                    newText.GetComponent<FontCharDebugText>().languageId = languageIdToTest;
                    newText.name = fontData.assetName;
                    newText.GetComponent<FontCharDebugText>().Initiate();
                    SetFontDynamic(fontAsset.sourceFontFile, true);
                }
            }
            else
                Debug.LogWarning("language " + languageIdToTest + " not found");
        }
	}

    void SetFontDynamic(Font font, bool dynamic)
    {
#if UNITY_EDITOR

        // Get around HasCharacter always returning true in dynamic font mode
        var fontPath = UnityEditor.AssetDatabase.GetAssetPath(font);
        var fontData = (UnityEditor.TrueTypeFontImporter)UnityEditor.AssetImporter.GetAtPath(fontPath);

        if (!dynamic && fontData.fontTextureCase == UnityEditor.FontTextureCase.Dynamic)
            fontData.fontTextureCase = UnityEditor.FontTextureCase.Unicode;
        else if (dynamic)
            fontData.fontTextureCase = UnityEditor.FontTextureCase.Dynamic;

        print(fontData.fontTextureCase);
        fontData.SaveAndReimport();
#endif
    }
}
