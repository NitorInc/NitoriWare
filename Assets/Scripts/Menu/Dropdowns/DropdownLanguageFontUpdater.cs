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
        Font overrideFont = LocalizationManager.instance.getAllLanguages()[transform.GetSiblingIndex() - 1].overrideFont;
        if (overrideFont != null)
            textComponent.font = overrideFont;
	}
	
	void Update ()
    {
		
	}
}
