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
}
