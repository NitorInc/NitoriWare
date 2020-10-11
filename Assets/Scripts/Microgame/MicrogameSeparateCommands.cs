using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Microgame/Separate Commands")]
public class MicrogameSeparateCommands : Microgame
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

    new public class Session : Microgame.Session
    { 
        private DifficultyCommand command;

        public override string GetLocalizedCommand()
            => TextHelper.getLocalizedText("microgame." + microgame.microgameId + ".command" + command.commandKeySuffix,
                command.defaultValue);

        public Session(Microgame microgame, int difficulty, bool debugMode, DifficultyCommand command)
            : base(microgame, difficulty, debugMode)
        {
            this.command = command;
        }
    }
    
    public override Microgame.Session CreateSession(int difficulty, bool debugMode = false)
    {
        return new Session(this, difficulty, debugMode, GetCommand(difficulty));
    }

    private DifficultyCommand GetCommand(int difficulty)
    {
        switch (difficulty)
        {
            case (1):
                return difficulty1;
            case (2):
                return difficulty2;
            case (3):
                return difficulty3;
        }
        return null;
    }
}