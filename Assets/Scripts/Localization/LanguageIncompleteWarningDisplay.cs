using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LanguageIncompleteWarningDisplay : MonoBehaviour
{
    private Text textComponent;
    private TextMeshPro textMeshPro;
    private TextMeshProUGUI textMeshProUGUI;

    void Awake()
    {
        textComponent = GetComponent<Text>();
        textMeshPro = GetComponent<TextMeshPro>();
        textMeshProUGUI = GetComponent<TextMeshProUGUI>();
    }

    void OnTextLocalized()
    {
        bool enableText = TextHelper.getLoadedLanguage() == null || !LocalizationManager.instance.isLoadedLanguageComplete();

        if (textComponent != null)
            textComponent.enabled = enableText;
        if (textMeshPro != null)
            textMeshPro.enabled = enableText;
        if (textMeshProUGUI != null)
            textMeshProUGUI.enabled = enableText;
    }
}
