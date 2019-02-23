using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FontCharDebugTextGen : MonoBehaviour
{
    [SerializeField]
    private float textXSeparation;
    [SerializeField]
    private GameObject textPrefab;
    
	void Start ()
    {
        var languageIds = LanguagesData.instance.languages.Select(a => a.getLanguageID()).ToList();
        languageIds.Add("All");
        languageIds.Add("NonAsian");

        for (int i = 0; i < languageIds.Count; i++)
        {
            var languageId = languageIds[i];
            var newText = Instantiate(textPrefab, transform.position + (Vector3.right * textXSeparation * i), Quaternion.identity);
            newText.transform.position = transform.position + (Vector3.right * textXSeparation * i);
            newText.GetComponent<FontCharDebugText>().languageId = languageId;
            newText.name = languageId;
        }
	}
}
