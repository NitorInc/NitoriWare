using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DropdownLanguageUpdater : MonoBehaviour
{

#pragma warning disable 0649   //Serialized Fields
    [SerializeField]
    private UnityEvent onLanguageChanged;
#pragma warning restore 0649

    string currentLanguage;

	void Start()
	{
        currentLanguage = TextHelper.getLoadedLanguage();
	}
	
	void Update()
	{
		if (currentLanguage != TextHelper.getLoadedLanguage())
        {
            onLanguageChanged.Invoke();
            currentLanguage = TextHelper.getLoadedLanguage();
        }
	}
}
