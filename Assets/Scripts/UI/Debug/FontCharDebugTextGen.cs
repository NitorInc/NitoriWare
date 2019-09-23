using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class FontCharDebugTextGen : MonoBehaviour
{
    [Header("---Put font here---")]
    [SerializeField]
    private Font font;
    [Header("-------------------")]

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


        for (int i = 0; i < languageIds.Count; i++)
        {
            var languageId = languageIds[i];
            var newText = Instantiate(textPrefab, transform.position + (Vector3.right * textXSeparation * i), Quaternion.identity);
            newText.transform.position = transform.position + (Vector3.right * textXSeparation * i);
            newText.GetComponent<Text>().font = font;
            newText.transform.SetParent(transform);
            newText.GetComponent<FontCharDebugText>().languageId = languageId;
            newText.name = languageId;
        }
	}
}
