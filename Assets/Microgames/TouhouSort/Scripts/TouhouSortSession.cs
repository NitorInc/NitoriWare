using System.Linq;
using UnityEngine;

public class TouhouSortSession : MicrogameSession
{
    public TouhouSortMicrogame.CategoryScene selectedCategory { get; private set; }

    public override string GetLocalizedCommand()
        => string.Format(TextHelper.getLocalizedText($"microgame.{microgame.microgameId}.command", microgame.commandDefault),
            TextHelper.getLocalizedText($"microgame.TouhouSort.{selectedCategory.IdName}", selectedCategory.IdName));

    public override string SceneName => selectedCategory.SceneName;

    public TouhouSortSession(Microgame microgame, StageController player, int difficulty, bool debugMode, TouhouSortMicrogame.CategoryScene[] categories)
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
