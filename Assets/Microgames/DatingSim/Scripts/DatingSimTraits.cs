using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(menuName = "Microgame Assets/DatingSim/Traits")]
public class DatingSimTraits : MicrogameTraits
{
    [SerializeField]
    private CharacterScene[] possibleScenes;
    
    public int overrideCharacter = -1;

    public override AudioClip GetMusicClip(MicrogameSession session) => ((DatingSimSession)session).scene.MusicClip;
    public override AudioClip[] GetAllMusicClips() => possibleScenes.Select(a => a.MusicClip).ToArray();

    public override string GetSceneName(MicrogameSession session) => ((DatingSimSession)session).scene.SceneName;
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

    public override MicrogameSession onAccessInStage(string microgameId, int difficulty)
    {
        CharacterScene selectedCharacter;

        if (overrideCharacter > -1)
            selectedCharacter = possibleScenes[overrideCharacter];
        else
            selectedCharacter = possibleScenes[Random.Range(0, possibleScenes.Length)];
        
        return new DatingSimSession(microgameId, difficulty, selectedCharacter);
    }

    public override void onDebugModeAccess(MicrogameController microgameController, MicrogameSession session)
    {
        var loadedScene = possibleScenes
            .FirstOrDefault(a => a.SceneName.Equals(microgameController.gameObject.scene.name));
        
        if (loadedScene != null)
            (session as DatingSimSession).scene = loadedScene;
    }
}
