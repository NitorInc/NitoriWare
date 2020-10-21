using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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
        [SerializeField]
        [Range(0, 1)]
        private float probabilityWeight = 1f;
        public float ProbabilityWeight => probabilityWeight;
    }

    public override bool SceneDeterminesDifficulty() => false;

    public override Microgame.Session CreateSession(int difficulty, bool debugMode = false)
    {

        return new Session(this, difficulty, debugMode);
    }

    new public class Session : Microgame.Session
    {
        protected MicrogameScene chosenScene;

        public override string GetSceneName() => chosenScene.SceneName;

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
            {
                // Choose random scene
                chosenScene = choices.FirstOrDefault();
                var pSum = choices.Sum(a => a.ProbabilityWeight);
                if (pSum <= 0f)
                    chosenScene = choices[Random.Range(0, choices.Length)];
                else
                {
                    var selectedP = Random.Range(0f, pSum);
                    foreach (var choice in choices)
                    {
                        selectedP -= choice.ProbabilityWeight;
                        if (selectedP <= 0f)
                        {
                            chosenScene = choice;
                            break;
                        }
                    }
                }
            }
        }
    }
}

