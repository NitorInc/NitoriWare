using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(menuName = "Microgame Assets/DatingSim/Traits")]
public class DatingSimTraits : Microgame
{
    [SerializeField]
    int overrideCharacter = -1;
    [SerializeField]
    private bool debugRandomCharacter;
    
    [SerializeField]
    private CharacterScene[] possibleScenes;
    

    public override AudioClip GetMusicClip(MicrogameSession session) => ((DatingSimSession)session).scene.MusicClip;
    public override AudioClip[] GetAllPossibleMusicClips() => possibleScenes.Select(a => a.MusicClip).ToArray();

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

    public override MicrogameSession onAccessInStage(string microgameId, int difficulty, bool isDebugMode = false)
    {
        if (isDebugMode && !debugRandomCharacter)
        {
            var loadedScene = possibleScenes
                .FirstOrDefault(a => a.SceneName.Equals(MicrogameController.instance.gameObject.scene.name));
            return new DatingSimSession(microgameId, difficulty, loadedScene);
        }
        else
        {
            var selectedCategory = possibleScenes[Random.Range(0, possibleScenes.Length)];
            return new DatingSimSession(microgameId, difficulty, selectedCategory);
        }
    }
}
