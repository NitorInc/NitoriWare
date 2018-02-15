using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DatingSimHelper
{
    public static DatingSimCharacters.Character getSelectedCharacter()
    {
        return ((DatingSimTraits)MicrogameController.instance.getTraits()).getSelectedCharacter();
    }

    public static bool getOptionIsRight(DatingSimCharacters.CharacterOption option)
    {
        return getSelectedCharacter().rightOptions.Contains(option);
    }

    public static int getOptionIndex(DatingSimCharacters.CharacterOption option, bool right)
    {
        if (right)
            return getSelectedCharacter().rightOptions.IndexOf(option);
        else
            return getSelectedCharacter().wrongOptions.IndexOf(option);
    }

    public static int getOptionIndex(DatingSimCharacters.CharacterOption option)
    {
        return getOptionIndex(option, getOptionIsRight(option));
    }
}
