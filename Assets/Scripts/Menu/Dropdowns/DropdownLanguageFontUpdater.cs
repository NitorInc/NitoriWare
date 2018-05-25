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
        var languages = LanguagesData.instance.languages;
        Language language = languages[0];

        //Determine language index based on sibling position and selectable languages
        int index = 0;
        int objectIndex = transform.GetSiblingIndex() - 1;
        for (int i = 0; i < languages.Length; i++)
        {
            if (!languages[i].disableSelect)
            {
                if (index >= objectIndex)
                {
                    language = languages[i];
                    break;
                }
                index++;
            }

        }

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
