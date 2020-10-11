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

    private DatingSimOptionData optionData;
    private int optionIndex;
    private bool isRight;
    public DatingSimOptionData.Option option => DatingSimLocalizationHelper.GetOption(optionData, optionIndex, isRight);

    public void initialize(DatingSimOptionData optionData, int optionIndex, bool isRight)
    {
        this.optionData = optionData;
        this.optionIndex = optionIndex;
        this.isRight = isRight;

        if (textComp == null)
            textComp = GetComponentInChildren<TMP_Text>();
        if (cursor == null)
        {
            cursor = GetComponentInChildren<DatingSimCursorAnimation>();
            cursor.gameObject.SetActive(false);
        }
        SetText(DatingSimLocalizationHelper.getLocalizedOptionDialogue(optionData, optionIndex, isRight, false));

        ShowCursor(false);
    }

    public string getLocalizedResponse()
    {
        return DatingSimLocalizationHelper.getLocalizedOptionDialogue(optionData, optionIndex, isRight, true);
    }

    public bool IsRight() => isRight;
    public void HighlightText(bool highlight) => textComp.color = highlight ? defaultColor : greyColor;
    public void ShowText(bool show) => gameObject.SetActive(show);

    public void ShowCursor(bool show)
    {
        cursor.gameObject.SetActive(show);
        HighlightText(show);
    }

    void SetText(string text) => textComp.text = text;

}

