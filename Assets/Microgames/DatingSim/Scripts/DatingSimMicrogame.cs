using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(menuName = "Microgame Assets/DatingSim/Microgame")]
public class DatingSimMicrogame : Microgame
{
    [SerializeField]
    int overrideCharacter = -1;
    [SerializeField]
    private bool debugRandomCharacter;
    public bool DebugRandomCharacter => debugRandomCharacter;
    
    [SerializeField]
    private CharacterScene[] possibleScenes;
    
    
    public override AudioClip[] GetAllPossibleMusicClips() => possibleScenes.Select(a => a.MusicClip).ToArray();

    public override bool SceneDeterminesDifficulty => false;

    public override MicrogameSession CreateSession(StageController player, int difficulty, bool debugMode = false)
    {
        return new DatingSimSession(this, player, difficulty, debugMode, possibleScenes);
    }

    [System.Serializable]
    public class CharacterScene
    {
        [SerializeField]
        private string characterId;
        public string CharacterId { get => characterId; set => characterId = value; }

        [SerializeField]
        private string sceneName;
        public string SceneName { get => sceneName; set => sceneName = value; }

        [SerializeField]
        private AudioClip music;
        public AudioClip MusicClip { get => music; set => music = value; }
    }

    public class DatingSimSession : MicrogameSession
    {
        public CharacterScene selectedCharacterScene { get; private set; }

        public override string SceneName => selectedCharacterScene.SceneName;

        public override AudioClip MusicClip => selectedCharacterScene.MusicClip;

        public DatingSimSession(Microgame microgame, StageController player, int difficulty, bool debugMode, CharacterScene[] possibleScenes)
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
}
