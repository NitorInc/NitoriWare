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
}
