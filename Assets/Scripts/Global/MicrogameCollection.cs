using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

public static class MicrogameCollection
{
    public const string MicrogameAssetPath = "/Microgames/";
    public const string MicrogameResourcesPath = "Microgames/";
    public static string FullMicrogameAssetPath => Path.Combine(Application.dataPath, MicrogameAssetPath.Replace("/", ""));


    static  Sprite getSprite(string microgameId)
    {
        return Resources.Load<Sprite>("MicrogameIcons/" + microgameId + "Icon");
    }

    public static Microgame[] LoadAllMicrogames() => Resources.LoadAll<Microgame>(MicrogameResourcesPath);

    public static Microgame LoadMicrogame(string microgameId)
    {
        return Resources.Load<Microgame>(MicrogameResourcesPath + microgameId);
    }


    public static void updateBuildPath()
    {
#if UNITY_EDITOR

        var microgames = LoadAllMicrogames();

        string microgameFolderLocation = Path.Combine(Application.dataPath, MicrogameAssetPath.Replace("/", ""));
        var microgameFolders = Directory.GetDirectories(microgameFolderLocation)
                .Concat(Directory.GetDirectories(Path.Combine(microgameFolderLocation, "_Bosses")));
        var buildScenes = EditorBuildSettings.scenes.ToList();

        //Remove all microgames from path
        buildScenes = buildScenes.Where(a => !a.path.Replace('\\', '/').Contains(MicrogameAssetPath.Replace('\\', '/'))).ToList();

        //Re-add stage ready games
        foreach (var microgame in microgames.Where(a => a.milestone >= Microgame.Milestone.StageReady))
        {
            // Get all stages with Microgame ID in name that are part of microgames path
            var scenePaths = AssetDatabase.FindAssets($"{microgame.microgameId} t:Scene")
                .Select(a => AssetDatabase.GUIDToAssetPath(a))
                .Where(a => a.Replace('\\', '/').Contains(MicrogameAssetPath.Replace('\\', '/')))
                .Select(a => new EditorBuildSettingsScene(a, true));

            buildScenes.AddRange(scenePaths);
        }

        EditorBuildSettings.scenes = buildScenes.ToArray();

        Debug.Log("Build path updated");
#else
        Debug.LogError("Microgame updates should not be called outside of the editor. You shouldn't even see this message.");
#endif
    }

    public static Microgame GetDebugModeMicrogame(string sceneName)
    {
        var microgameDirectories = Directory.GetDirectories(Application.dataPath + MicrogameAssetPath)
            .Concat(Directory.GetDirectories(Application.dataPath + MicrogameAssetPath + "_Bosses/"));
        foreach (var directory in microgameDirectories)
        {
            string microgameId = Path.GetFileName(directory);
            if (!microgameId.StartsWith("_") && sceneName.Contains(microgameId))
                return LoadMicrogame(microgameId);
        }

        return null;
    }

}
