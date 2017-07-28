using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropdownLanguageFontUpdater : MonoBehaviour
{
    [SerializeField]
    private Text textComponent;

	void Start ()
    {
        var language = LocalizationManager.instance.getAllLanguages()[transform.GetSiblingIndex() - 1];
        if (language.overrideFont != null)
        {
            textComponent.font = language.overrideFont;
            if (language.forceUnbold)
                textComponent.fontStyle = FontStyle.Normal;
        }
	}
	
	void Update ()
    {
		
	}
}
