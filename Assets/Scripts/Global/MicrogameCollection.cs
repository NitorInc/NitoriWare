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
    private List<Microgame> finishedMicrogames, stageReadyMicrogames, unfinishedMicrogames, bossMicrogames;

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

	public enum Restriction
	{
		All,
		StageReady,
		Finished
	}

	public void updateMicrogames()
	{
		finishedMicrogames = new List<Microgame>();
		stageReadyMicrogames = new List<Microgame>();
		unfinishedMicrogames = new List<Microgame>();
		bossMicrogames = new List<Microgame>();

		string[] microgameDirectories = Directory.GetDirectories(Application.dataPath + MicrogameAssetPath + "_Finished/");
		for (int i = 0; i < microgameDirectories.Length; i++)
		{
			string[] dirs = microgameDirectories[i].Split('/');
			string microgameId = dirs[dirs.Length - 1];
            MicrogameTraits[] difficultyTraits = getDifficultyTraits(microgameId, false);
            finishedMicrogames.Add(new Microgame(microgameId, difficultyTraits, getSprite(microgameId)));
		}

		microgameDirectories = Directory.GetDirectories(Application.dataPath + MicrogameAssetPath);
		for (int i = 0; i < microgameDirectories.Length; i++)
		{
			string[] dirs = microgameDirectories[i].Split('/');
			string microgameId = dirs[dirs.Length - 1];
			if (!microgameId.StartsWith("_"))
			{
                MicrogameTraits[] difficultyTraits = getDifficultyTraits(microgameId, true);
				if (difficultyTraits[0].isStageReady)
					stageReadyMicrogames.Add(new Microgame(microgameId, difficultyTraits, getSprite(microgameId)));
				else
					unfinishedMicrogames.Add(new Microgame(microgameId, difficultyTraits, getSprite(microgameId)));
			}
		}

		microgameDirectories = Directory.GetDirectories(Application.dataPath + MicrogameAssetPath + "_Bosses/");
        for (int i = 0; i < microgameDirectories.Length; i++)
		{
			string[] dirs = microgameDirectories[i].Split('/');
			string microgameId = dirs[dirs.Length - 1];
            MicrogameTraits[] difficultyTraits = getDifficultyTraits(microgameId, true);
            bossMicrogames.Add(new Microgame(microgameId, difficultyTraits, getSprite(microgameId)));
		}
	}

    MicrogameTraits[] getDifficultyTraits(string microgameId, bool skipFInishedFolder)
    {
        MicrogameTraits[] traits = new MicrogameTraits[3];
        for (int i = 0; i < 3; i++)
        {
            traits[i] = MicrogameTraits.findMicrogameTraits(microgameId, i + 1, skipFInishedFolder);
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

        string microgameFolderLocation = Path.Combine(Application.dataPath + MicrogameAssetPath);
        var microgameFolders = Directory.GetDirectories(microgameFolderLocation)
                .Concat(Directory.GetDirectories(Path.Combine(microgameFolderLocation, "_Finished")))
                .Concat(Directory.GetDirectories(Path.Combine(microgameFolderLocation, "_Bosses")));
        var buildScenes = EditorBuildSettings.scenes.ToList();
        

        //Add stage ready games
        foreach (var microgame in stageReadyMicrogames.Concat(finishedMicrogames))
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
                            buildScenes.Add(new EditorBuildSettingsScene("Assets" + scenePath.Substring(Application.dataPath.Length), true));
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

    /// <summary>
    /// Returns all microgames in the game (with given restriction) in Stage.Microgame type (used for determining what will play in the stage)
    /// </summary>
    /// <param name="restriction"></param>
    /// <returns></returns>
    public List<Stage.Microgame> getStageMicrogames(Restriction restriction)
	{
        List<Stage.Microgame> returnList = convertToStageMicrogameList(finishedMicrogames);
		if (restriction != Restriction.Finished)
		{
			returnList.AddRange(convertToStageMicrogameList(stageReadyMicrogames));
			if (restriction == Restriction.All)
				returnList.AddRange(convertToStageMicrogameList(unfinishedMicrogames));
		}
		return returnList;
	}

	/// <summary>
	/// Returns a copied list of all boss microgmaes, regardless of completion, in Stage.Microgame type
	/// </summary>
	/// <returns></returns>
	public List<Stage.Microgame> getStageBossMicrogames()
	{
        List<Stage.Microgame> returnList = convertToStageMicrogameList(bossMicrogames);
		return returnList;
    }

    /// <summary>/// Returns all microgames in the game (with given restriction) in MicrogmaeCollection.Microgame type (used for batch getting traits)
    /// </summary>
    /// <returns></returns>
    public List<Microgame> getCollectionMicrogames(Restriction restriction)
    {
        List<Microgame> returnList = new List<Microgame>(finishedMicrogames);
        if (restriction != Restriction.Finished)
        {
            returnList.AddRange(new List<Microgame>(stageReadyMicrogames));
            if (restriction == Restriction.All)
                returnList.AddRange(new List<Microgame>(unfinishedMicrogames));
        }
        return returnList;
    }

    /// <summary>
    /// Returns a copied list of all boss microgmaes, regardless of completion, in MicrogameCollection.Microgame type
    /// </summary>
    /// <returns></returns>
    public List<Microgame> getCollectionBossMicrogames()
    {
        List<Microgame> returnList = new List<Microgame>(bossMicrogames);
        return returnList;
    }

    public Microgame findMicrogame(string microgameId)
    {
        //TODO optimize this whole process
        foreach (Microgame microgame in finishedMicrogames)
        {
            if (microgame.microgameId.Equals(microgameId))
                return microgame;
        }
        foreach (Microgame microgame in stageReadyMicrogames)
        {
            if (microgame.microgameId.Equals(microgameId))
                return microgame;
        }
        foreach (Microgame microgame in unfinishedMicrogames)
        {
            if (microgame.microgameId.Equals(microgameId))
                return microgame;
        }
        foreach (Microgame microgame in bossMicrogames)
        {
            if (microgame.microgameId.Equals(microgameId))
                return microgame;
        }
        Debug.Log("Can't find Microgame " + microgameId);
        return null;
    }

    private List<Stage.Microgame> convertToStageMicrogameList(List<Microgame> list)
    {
        List<Stage.Microgame>  returnList = new List<Stage.Microgame>();
        for (int i = 0; i < list.Count; i++)
        {
            returnList.Add(new Stage.Microgame(list[i].microgameId));
        }
        return returnList;
    }
}
