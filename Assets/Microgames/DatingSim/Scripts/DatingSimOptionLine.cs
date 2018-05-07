using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DatingSimOptionLine : MonoBehaviour
{
    TMP_Text textComp;
    DatingSimCursorAnimation cursor;
    private bool right;
    private int index;

    public Color defaultColor;
    public Color greyColor;

    public void initialize(DatingSimCharacters.CharacterOption option)
    {
        right = DatingSimHelper.getOptionIsRight(option);
        index = DatingSimHelper.getOptionIndex(option, right);

        DatingSimHelper.getSelectedCharacter();

        if (textComp == null)
            textComp = GetComponentInChildren<TMP_Text>();
        if (cursor == null)
        {
            cursor = GetComponentInChildren<DatingSimCursorAnimation>();
            cursor.gameObject.SetActive(false);
        }
        SetText(DatingSimHelper.getSelectedCharacter().getLocalizedOptionDialogue(right, index, false));

        ShowCursor(false);
    }

    public string getLocalizedResponse()
    {
        return DatingSimHelper.getSelectedCharacter().getLocalizedOptionDialogue(right, index, true);
    }
    
    public bool isRight()
    {
        return right;
    }

    void SetText(string text)
    {
        textComp.text = text;
    }

    public void HighlightText(bool highlight)
    {
        textComp.color = highlight ? defaultColor : greyColor;
    }

    public void ShowText(bool show)
    {
        gameObject.SetActive(show);
    }

    public void ShowCursor(bool show)
    {
        cursor.gameObject.SetActive(show);
        HighlightText(show);
    }
}
