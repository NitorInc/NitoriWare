using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DropdownLanguageUpdater : MonoBehaviour
{

#pragma warning disable 0649
    [SerializeField]
    private UnityEvent onLanguageChanged;
#pragma warning restore 0649

    string currentLanguage;

	void Start()
	{
        currentLanguage = TextHelper.getLoadedLanguageID();
	}
	
	void Update()
	{
		if (currentLanguage != TextHelper.getLoadedLanguageID())
        {
            onLanguageChanged.Invoke();
            currentLanguage = TextHelper.getLoadedLanguageID();
        }
	}
}
