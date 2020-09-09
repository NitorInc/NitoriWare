using System.Linq;
using UnityEngine;

public class DatingSimSession : MicrogameSession
{
    public DatingSimMicrogame.CharacterScene selectedCharacterScene { get; private set; }

    public override string SceneName => selectedCharacterScene.SceneName;

    public DatingSimSession(Microgame microgame, StageController player, int difficulty, bool debugMode, DatingSimMicrogame.CharacterScene[] possibleScenes)
        : base(microgame, player, difficulty, debugMode)
    {
        if (debugMode && !(microgame as DatingSimMicrogame).DebugRandomCharacter)
        {
            selectedCharacterScene = possibleScenes
                .FirstOrDefault(a => a.SceneName.Equals(MicrogameController.instance.gameObject.scene.name));
        }
        else
        {
            selectedCharacterScene = possibleScenes[Random.Range(0, possibleScenes.Length)];
        }
    }
}
