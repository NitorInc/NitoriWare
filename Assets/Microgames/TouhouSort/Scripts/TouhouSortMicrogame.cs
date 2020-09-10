using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(menuName = "Microgame Assets/TouhouSort/Microgame")]
public class TouhouSortMicrogame : Microgame
{
    [SerializeField]
    private bool debugRandomScene = true;
    public bool DebugRandomScene => debugRandomScene;

    [SerializeField]
    private CategoryScene[] categories;

    public override MicrogameSession CreateSession(StageController player, int difficulty, bool debugMode = false)
    {
        return new TouhouSortSession(this, player, difficulty, debugMode, categories);
    }

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
    
    public override bool SceneDeterminesDifficulty => false;

    private string defaultCommand => _command;

    public class TouhouSortSession : MicrogameSession
    {
        public CategoryScene selectedCategory { get; private set; }

        public override string GetLocalizedCommand()
            => string.Format(TextHelper.getLocalizedText($"microgame.{microgame.microgameId}.command", (microgame as TouhouSortMicrogame).defaultCommand),
                TextHelper.getLocalizedText($"microgame.TouhouSort.{selectedCategory.IdName}", selectedCategory.IdName));

        public override string SceneName => selectedCategory.SceneName;

        public TouhouSortSession(Microgame microgame, StageController player, int difficulty, bool debugMode, CategoryScene[] categories)
            : base(microgame, player, difficulty, debugMode)
        {
            if (debugMode && !(microgame as TouhouSortMicrogame).DebugRandomScene)
            {
                var categoryPool = categories
                    .ToArray();
                selectedCategory = categoryPool
                    .FirstOrDefault(a => a.SceneName.Equals(MicrogameController.instance.gameObject.scene.name));
            }
            else
            {
                var categoryPool = categories
                    .Where(a => difficulty >= a.MinDifficulty)
                    .ToArray();
                selectedCategory = categoryPool[Random.Range(0, categoryPool.Length)];
            }
        }
    }
}
