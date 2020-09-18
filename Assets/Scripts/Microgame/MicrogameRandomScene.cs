using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(menuName = "Microgame/Random Scene")]
public class MicrogameRandomScene : Microgame
{
    [Header("Random Scene traits")]
    [SerializeField]
    private bool dontRandomizeInDebugMode;

    [SerializeField]
    protected MicrogameScene[] scenePool;

    [System.Serializable]
    public class MicrogameScene
    {
        [SerializeField]
        private string sceneName;
        public string SceneName => sceneName;
        [SerializeField]
        private int minDifficulty = 1;
        public int MinDifficulty => minDifficulty;
        [SerializeField]
        private int maxDifficulty = 3;
        public int MaxDifficulty => maxDifficulty;
        [Header("Music clip is optional and will default to base audio clip if not set")]
        [SerializeField]
        private AudioClip musicClip;
        public AudioClip MusicClip => musicClip;
    }

    public override bool SceneDeterminesDifficulty() => false;

    public override AudioClip[] GetAllPossibleMusicClips()
    {
        var list = scenePool
            .Select(a => a.MusicClip)
            .ToList();
        list.Add(_musicClip);
        return list
            .Where(a => a != null)
            .ToArray();
    }

    public override Microgame.Session CreateSession(int difficulty, bool debugMode = false)
    {

        return new Session(this, difficulty, debugMode);
    }

    new public class Session : Microgame.Session
    {
        protected MicrogameScene chosenScene;

        public override string GetSceneName() => chosenScene.SceneName;

        public override AudioClip GetMusicClip()
        {
            return chosenScene.MusicClip != null ? chosenScene.MusicClip : base.GetMusicClip();
        }

        public Session(Microgame microgame, int difficulty, bool debugMode)
            : base(microgame, difficulty, debugMode)
        {
            var scenePool = (microgame as MicrogameRandomScene).scenePool;
            if (!scenePool.Any())
            {
                Debug.LogError($"You must enter values for the scene pool in {microgame.microgameId}.asset before playing.");
                Debug.Break();
                return;
            }

            var choices = scenePool
                .Where(a => difficulty >= a.MinDifficulty && difficulty <= a.MaxDifficulty)
                .ToArray();
            if (!choices.Any())
            {
                Debug.LogError($"No appropriate scenes found for {microgame.microgameId} difficulty {difficulty}. Check the scene pool configuration.");
                Debug.Break();
                return;
            }
            if (debugMode && (microgame as MicrogameRandomScene).dontRandomizeInDebugMode)
                chosenScene = choices.FirstOrDefault(a => a.SceneName.Equals(MicrogameController.instance.gameObject.scene.name));
            else
                chosenScene = choices[Random.Range(0, choices.Length)];
        }
    }
}

