using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Microgame Traits/Traits (Separate Commands)")]
public class MicrogameTraitsSeparateCommands : Microgame
{
    public string commandKeySuffix;

    [SerializeField]
    private DifficultyCommand difficulty1;
    [SerializeField]
    private DifficultyCommand difficulty2;
    [SerializeField]
    private DifficultyCommand difficulty3;

    [System.Serializable]
    public class DifficultyCommand
    {
        public string commandKeySuffix;
        public string defaultValue;
    }


    public override string GetLocalizedCommand(MicrogameSession session)
    {
        var difficulty = new DifficultyCommand[] { null, difficulty1, difficulty2, difficulty3 }[session.Difficulty];
        return TextHelper.getLocalizedText("microgame." + session.MicrogameId + ".command" + difficulty.commandKeySuffix, difficulty.defaultValue);
    }
}

