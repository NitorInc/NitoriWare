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

    public enum MicrogameType
    {
        OneScenePerDifficulty,
        SingleScene,
        RandomScene
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

        // Copy scene and create new traits
        var newScenePath = $"Assets/Microgames/{microgameId}/Scenes/{microgameId}1.unity";
        var traitsPath = $"Assets/Resources/Microgames/{microgameId}.asset";
        switch (type)
        {
            case (MicrogameType.SingleScene):
                newScenePath = $"Assets/Microgames/{microgameId}/Scenes/{microgameId}.unity";
                AssetDatabase.CreateAsset(CreateInstance<MicrogameSingleScene>(), traitsPath);
                break;
            case (MicrogameType.RandomScene):
                AssetDatabase.CreateAsset(CreateInstance<MicrogameRandomScene>(), traitsPath);
                break;
            default:
                AssetDatabase.CreateAsset(CreateInstance<Microgame>(), traitsPath);
                break;
        }
        AssetDatabase.CopyAsset(templateSceneFilePath, newScenePath);

        AssetDatabase.Refresh();
        EditorSceneManager.OpenScene(newScenePath);
        Selection.activeObject = AssetDatabase.LoadAssetAtPath<Microgame>(traitsPath);

        Debug.Log($"Microgame {microgameId} created!");

        return true;
    }
}