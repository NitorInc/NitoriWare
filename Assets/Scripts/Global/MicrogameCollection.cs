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
    public List<Microgame> BossMicrogames => microgames.Where(a => a.isBoss).ToList();

    [System.Serializable]
    public class Microgame
    {
        [SerializeField]
        private string _microgameId;
        public string microgameId => _microgameId;

        [SerializeField]
        private MicrogameTraits _traits;
        public MicrogameTraits traits => _traits;

        [SerializeField]
        private Sprite _menuIcon;
        public Sprite menuIcon => _menuIcon;

        public Microgame(string microgameId, MicrogameTraits traits, Sprite menuIcon)
        {
            _microgameId = microgameId;
            _traits = traits;
            _menuIcon = menuIcon;
        }

        public bool isBoss => traits.isBossMicrogame();
    }

	public void updateMicrogames()
	{
        _microgames = new List<Microgame>();

		var microgameDirectories = Directory.GetDirectories(Application.dataPath + MicrogameAssetPath)
            .Concat(Directory.GetDirectories(Application.dataPath + MicrogameAssetPath + "_Bosses/"));
        foreach (var directory in microgameDirectories)
		{
			string microgameId = Path.GetFileName(directory);
            if (!microgameId.StartsWith("_"))
            {
                _microgames.Add(new Microgame(microgameId, MicrogameTraits.findMicrogameTraits(microgameId), getSprite(microgameId)));
            }
		}
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
        foreach (var microgame in microgames.Where(a => a.traits.milestone >= MicrogameTraits.Milestone.StageReady))
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
        Debug.LogError("Microgame updates should NOT be called outside of the editor. You shouldn't even see this message.");
#endif
    }

#if UNITY_EDITOR

    public void SetMicrogameMusicToStreaming()
    {
        var musicClips = MicrogameCollection.instance.microgames
            .SelectMany(a => a.traits.GetAllMusicClips())
            .Distinct();

        foreach (var musicClip in musicClips)
        {
            if (musicClip == null || musicClip.loadType == AudioClipLoadType.Streaming)
                continue;

            var assetPath = AssetDatabase.GetAssetPath(musicClip.GetInstanceID());
            var audio = AssetImporter.GetAtPath(assetPath) as AudioImporter;
            var sampleSettings = audio.defaultSampleSettings;
            sampleSettings.loadType = AudioClipLoadType.Streaming;
            audio.defaultSampleSettings = sampleSettings;
            audio.SaveAndReimport();
            Debug.Log("Changed music settings for " + musicClip.name);
        }
        AssetDatabase.Refresh();
        Debug.Log("Microgame music import settings updated");
    }
#endif

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
