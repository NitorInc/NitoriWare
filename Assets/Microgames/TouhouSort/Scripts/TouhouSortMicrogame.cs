using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(menuName = "Microgame Assets/TouhouSort/Microgame")]
public class TouhouSortMicrogame : MicrogameRandomScene
{
    [SerializeField]
    private CategoryScene[] categories;

    public override Microgame.Session CreateSession(int difficulty, bool debugMode = false)
    {
        return new Session(this, difficulty, debugMode);
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
    }

    private string defaultCommand => _command;

    new public class Session : MicrogameRandomScene.Session
    {
        public CategoryScene selectedCategory { get; private set; }

        public override string GetLocalizedCommand()
        {
            return string.Format(TextHelper.getLocalizedText($"microgame.{microgame.microgameId}.command", (microgame as TouhouSortMicrogame).defaultCommand),
                TextHelper.getLocalizedText($"microgame.TouhouSort.{selectedCategory.IdName}", selectedCategory.IdName));
        }

        public Session(Microgame microgame, int difficulty, bool debugMode)
            : base(microgame, difficulty, debugMode)
        {
            selectedCategory = (microgame as TouhouSortMicrogame).categories
                .FirstOrDefault(a => a.SceneName.Equals(chosenScene.SceneName));
        }
    }
}
