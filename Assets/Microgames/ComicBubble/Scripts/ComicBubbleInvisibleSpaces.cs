using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

//HACK hotfix for Thai and other languages with linebreak issues
public class ComicBubbleInvisibleSpaces : MonoBehaviour
{
    TextMeshProUGUI textComponent;

    [SerializeField]
    [Multiline]
    private string languages;

    private void Awake()
    {
        textComponent = GetComponent<TextMeshProUGUI>();
    }

    void OnTextLocalized()
    {
        if (languages.Split('\n').Contains(TextHelper.getLoadedLanguage().getLanguageID()))
        {
            string currentText = textComponent.text;
            string newText = "";
            int tries = 500;
            foreach (var character in currentText)
            {
                newText += "\u200B" + character;
                tries--;
                if (tries < 0)
                    break;
            }
            if (!string.IsNullOrEmpty(newText))
            {
                print(newText);
                textComponent.text = newText.Substring(1);
            }
        }
    }
}
