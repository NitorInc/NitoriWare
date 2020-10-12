using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DatingSimLocalizationHelper
{

    public static string getLocalizedOptionDialogue(DatingSimOptionData optionData, int optionIndex, bool isRight, bool getResponse)
    {
        var option = GetOption(optionData, optionIndex, isRight);
        var charId = optionData.charId;
        string optionKey = (isRight ? "right" : "wrong") + optionIndex.ToString();
        return TextHelper.getLocalizedMicrogameText(charId + "." + optionKey + "." + (getResponse ? "response" : "option"),
            (getResponse ? option.responseDialogue : option.optionDialogue));
    }

    public static DatingSimOptionData.Option GetOption(DatingSimOptionData optionData, int optionIndex, bool isRight)
        => (isRight? optionData.rightOptions : optionData.wrongOptions)[optionIndex];
}
