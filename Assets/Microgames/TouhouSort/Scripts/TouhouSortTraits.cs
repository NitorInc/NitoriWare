using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(menuName = "Microgame Assets/TouhouSort/Traits")]
public class TouhouSortTraits : MicrogameTraits
{
    [SerializeField]
    private CategoryScene[] categories;


    public override string GetLocalizedCommand(MicrogameSession session)
    {
        var touhouSortSession = (TouhouSortSession)session;
        return string.Format(TextHelper.getLocalizedText("microgame." + session.MicrogameId + ".command", command),
            TextHelper.getLocalizedText("microgame.TouhouSort." + touhouSortSession.category.IdName, touhouSortSession.category.IdName));
    }

    public override string GetSceneName(MicrogameSession session) => ((TouhouSortSession)session).category.SceneName;
    public override bool SceneDeterminesDifficulty => false;

    [System.Serializable]
    public class CategoryScene
    {
        [SerializeField]
        private string idName;
        public string IdName => idName;

        [SerializeField]
        private string sceneName;
        public string SceneName => sceneName;

        [SerializeField]
        private int minDifficulty;
        public int MinDifficulty => minDifficulty;
    }

    public override MicrogameSession onAccessInStage(string microgameId, int difficulty, bool isDebugMode = false)
    {
        var categoryPool = categories
            .Where(a => difficulty >= a.MinDifficulty)
            .ToArray();
        if (isDebugMode)
        {
            var loadedCategory = categoryPool
                .FirstOrDefault(a => a.SceneName.Equals(MicrogameController.instance.gameObject.scene.name));
            return new TouhouSortSession(microgameId, difficulty, loadedCategory);
        }
        else
        {
            var selectedCategory = categoryPool[Random.Range(0, categoryPool.Length)];
            return new TouhouSortSession(microgameId, difficulty, selectedCategory);
        }
    }
}
