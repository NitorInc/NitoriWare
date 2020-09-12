using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;

[CreateAssetMenu(menuName = "Microgame Creator")]
public class MicrogameCreator : ScriptableObject
{
    [SerializeField]
    private string templateSceneFilePath;
    [SerializeField]
    private string templateTraitsPath;

    public enum MicrogameType
    {
        OneScenePerDifficulty,
        SingleScene
    }

    public bool CreateMicrogame(string microgameId, MicrogameType type)
    {
        // Validate microgame ID
        if (string.IsNullOrEmpty(microgameId))
        {
            Debug.LogError("Microgame ID cannot be blank");
            return false;
        }
        if (Directory.Exists(Path.Combine(MicrogameCollection.FullMicrogameAssetPath, microgameId)))
        {
            Debug.LogError("Microgame folder for " + microgameId + " already exists");
            return false;
        }

        AssetDatabase.CreateFolder("Assets/Microgames", microgameId);
        AssetDatabase.CreateFolder($"Assets/Microgames/{microgameId}", "Scenes");

        var traitsPath = $"Assets/Microgames/{microgameId}/Traits.asset";
        string newScenePath;
        switch (type)
        {
            case (MicrogameType.SingleScene):
                // Copy single scene and create new traits
                newScenePath = $"Assets/Microgames/{microgameId}/Scenes/{microgameId}.unity";
                AssetDatabase.CopyAsset(templateSceneFilePath, newScenePath);
                AssetDatabase.CreateAsset(CreateInstance<MicrogameSingleScene>(), traitsPath);
                break;
            default:
                // Copy first difficulty scene and traits
                newScenePath = $"Assets/Microgames/{microgameId}/Scenes/{microgameId}1.unity";
                AssetDatabase.CopyAsset(templateSceneFilePath, newScenePath);
                AssetDatabase.CopyAsset(templateTraitsPath, traitsPath);
                break;
        }

        AssetDatabase.Refresh();
        EditorSceneManager.OpenScene(newScenePath);
        Selection.activeObject = AssetDatabase.LoadAssetAtPath<Microgame>(traitsPath);

        Debug.Log($"Microgame {microgameId} created");

        return true;
    }
}