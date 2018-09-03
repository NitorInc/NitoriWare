using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(menuName = "Control/Microgame Collection")]
public class MicrogameCollection : ScriptableObjectSingleton<MicrogameCollection>
{
    public const string MicrogameAssetPath = "/Microgames/";

    [SerializeField]
    private List<Microgame> _microgames;
    public List<Microgame> microgames => _microgames;
    
    [System.Serializable]
    public class Microgame
    {
        [SerializeField]
        private string _microgameId;
        public string microgameId => _microgameId;

        [SerializeField]
        private MicrogameTraits[] _difficultyTraits;
        public MicrogameTraits[] difficultyTraits => _difficultyTraits;

        [SerializeField]
        private Sprite _menuIcon;
        public Sprite menuIcon => _menuIcon;

        public Microgame(string microgameId, MicrogameTraits[] difficultyTraits, Sprite menuIcon)
        {
            _microgameId = microgameId;
            _difficultyTraits = difficultyTraits;
            _menuIcon = menuIcon;
        }
    }

	public void updateMicrogames()
	{
        _microgames = new List<Microgame>();

		var microgameDirectories = Directory.GetDirectories(Application.dataPath + MicrogameAssetPath)
            .Concat(Directory.GetDirectories(Application.dataPath + MicrogameAssetPath + "_Bosses/"));
        foreach (var directory in microgameDirectories)
		{
			string microgameId = directory.Substring(directory.LastIndexOf('/') + 1);
            if (!microgameId.StartsWith("_"))
            {
                MicrogameTraits[] difficultyTraits = getDifficultyTraits(microgameId);
                _microgames.Add(new Microgame(microgameId, difficultyTraits, getSprite(microgameId)));
            }
		}
	}

    MicrogameTraits[] getDifficultyTraits(string microgameId)
    {
        MicrogameTraits[] traits = new MicrogameTraits[3];
        for (int i = 0; i < 3; i++)
        {
            traits[i] = MicrogameTraits.findMicrogameTraits(microgameId, i + 1);
        }
        return traits;
    }
    
    Sprite getSprite(string microgameId)
    {
        return Resources.Load<Sprite>("MicrogameIcons/" + microgameId + "Icon");
    }

    public void updateBuildPath()
    {
#if UNITY_EDITOR

        string microgameFolderLocation = Path.Combine(Application.dataPath, MicrogameAssetPath.Replace("/", ""));
        var microgameFolders = Directory.GetDirectories(microgameFolderLocation)
                .Concat(Directory.GetDirectories(Path.Combine(microgameFolderLocation, "_Bosses")));
        var buildScenes = EditorBuildSettings.scenes.ToList();

        //Remove all microgames from path
        buildScenes = buildScenes.Where(a => !a.path.Replace('\\', '/').Contains(MicrogameAssetPath.Replace('\\', '/'))).ToList();

        //Re-add stage ready games
        foreach (var microgame in microgames.Where(a => a.difficultyTraits[0].milestone > MicrogameTraits.Milestone.Unfinished))
        {
            var microgameFolder = microgameFolders.FirstOrDefault(a => a.Contains(microgame.microgameId));
            if (microgameFolder != null)
            {
                if (buildScenes.FirstOrDefault(a => a.path.Contains(microgame.microgameId)) == null)
                {
                    var scenePaths = getMicrogameSceneFilesRecursive(microgameFolder, microgame.microgameId);
                    if (scenePaths != null)
                    {
                        foreach (var scenePath in scenePaths)
                        {
                            var newBuildScene = new EditorBuildSettingsScene("Assets" + scenePath.Substring(Application.dataPath.Length), true);
                            if (!newBuildScene.guid.Empty())
                                buildScenes.Add(newBuildScene);
                        }
                    }
                    else
                        Debug.Log($"Microgame {microgame.microgameId} scenes not found in folder!");
                }

            }
            else
                Debug.Log($"Microgame {microgame.microgameId} folder not found!");
        }
        EditorBuildSettings.scenes = buildScenes.ToArray();
        Debug.Log("Build path updated");
#else
        Debug.LogError("Microgame build updates should NOT be called outside of the editor. You shouldn't even see this message.");
#endif
    }

    string[] getMicrogameSceneFilesRecursive(string folder, string microgameId, bool checkForScenesFolderInBase= true)
    {
        var basePath = Path.Combine(folder, microgameId);
        if (File.Exists(basePath + "1.unity"))
        {
            return new string[]
            {
                basePath + "1.unity",
                basePath + "2.unity",
                basePath + "3.unity"
            };
        }

        //Try scenes folder first
        if (checkForScenesFolderInBase)
        {
            var scenesFolder = Directory.GetDirectories(folder).FirstOrDefault(a => a.ToLower().Contains("scene"));
            if (scenesFolder != null)
            {
                var foundScene = getMicrogameSceneFilesRecursive(scenesFolder, microgameId, false);
                if (foundScene != null)
                    return foundScene;
            }
        }

        //Check other folders
        foreach (var subfolder in Directory.GetDirectories(folder))
        {
            var foundScene = getMicrogameSceneFilesRecursive(subfolder, microgameId, false);
            if (foundScene != null)
                return foundScene;
        }
        return null;
    }

    public Microgame getMicrogame(string microgameId)
    {
        var microgame = microgames.FirstOrDefault(a => a.microgameId.Equals(microgameId));
        if (microgame == null)
            Debug.Log($"Can't find Microgame {microgameId}. Make sure the Microgame Collection has been updated");
        return microgame;
    }
}
