using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SplashLanguageWarning : MonoBehaviour
{
    private Text textComponent;

	void Awake()
    {
        textComponent = GetComponent<Text>();
	}
	
	void Update()
    {
        var language = TextHelper.getLoadedLanguage();
        if (!string.IsNullOrEmpty(language.getLanguageID()) && !LocalizationManager.instance.isLoadedLanguageComplete())
        {
            textComponent.enabled = true;
            enabled = false;
        }
            
	}
}
