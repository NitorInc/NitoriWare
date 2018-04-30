using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[ExecuteInEditMode]
public class MicrogameCollection : MonoBehaviour
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

    //public List<Microgame> getMicrogames(Restriction restriction)
    //{
    //    List<CollectionBase> returnList = convertToStageMicrogameList(finishedMicrogames);
    //    if (restriction != Restriction.Finished)
    //    {
    //        returnList.AddRange(convertToStageMicrogameList(stageReadyMicrogames));
    //        if (restriction == Restriction.All)
    //            returnList.AddRange(convertToStageMicrogameList(unfinishedMicrogames));
    //    }
    //    return returnList;
    //}

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
        Debug.Log("oops " + microgameId);
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
