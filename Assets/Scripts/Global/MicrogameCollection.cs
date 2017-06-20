using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[ExecuteInEditMode]
public class MicrogameCollection : MonoBehaviour
{
	[SerializeField]
	private List<Stage.Microgame> finishedMicrogames, stageReadyMicrogames, unfinishedMicrogames, bossMicrogames;

	public enum Restriction
	{
		All,
		StageReady,
		Finished
	}

	void Start()
	{
		updateMicrogames();
	}
	
	public void updateMicrogames()
	{
		finishedMicrogames = new List<Stage.Microgame>();
		stageReadyMicrogames = new List<Stage.Microgame>();
		unfinishedMicrogames = new List<Stage.Microgame>();
		bossMicrogames = new List<Stage.Microgame>();

		string[] microgameDirectories = Directory.GetDirectories(Application.dataPath + "/Resources/Microgames/_Finished/");
		for (int i = 0; i < microgameDirectories.Length; i++)
		{
			string[] dirs = microgameDirectories[i].Split('/');
			string microgameId = dirs[dirs.Length - 1];
			finishedMicrogames.Add(new Stage.Microgame(microgameId));
		}

		microgameDirectories = Directory.GetDirectories(Application.dataPath + "/Resources/Microgames/");
		for (int i = 0; i < microgameDirectories.Length; i++)
		{
			string[] dirs = microgameDirectories[i].Split('/');
			string microgameId = dirs[dirs.Length - 1];
			if (!microgameId.StartsWith("_"))
			{
				if (MicrogameTraits.findMicrogameTraits(microgameId, 1, true).isStageReady)
					stageReadyMicrogames.Add(new Stage.Microgame(microgameId));
				else
					unfinishedMicrogames.Add(new Stage.Microgame(microgameId));
			}
		}

		microgameDirectories = Directory.GetDirectories(Application.dataPath + "/Resources/Microgames/_Bosses/");
		for (int i = 0; i < microgameDirectories.Length; i++)
		{
			string[] dirs = microgameDirectories[i].Split('/');
			string microgameId = dirs[dirs.Length - 1];
			bossMicrogames.Add(new Stage.Microgame(microgameId));
		}
	}

	/// <summary>
	/// Returns a copied list of all microgames available in the game, with the given restriction on completeness, does not include bosses
	/// </summary>
	/// <param name="restriction"></param>
	/// <returns></returns>
	public List<Stage.Microgame> getMicrogames(Restriction restriction)
	{
		List<Stage.Microgame> returnList = new List<Stage.Microgame>(finishedMicrogames);
		if (restriction != Restriction.Finished)
		{
			returnList.AddRange(stageReadyMicrogames);
			if (restriction == Restriction.All)
				returnList.AddRange(unfinishedMicrogames);
		}
		return returnList;
	}

	/// <summary>
	/// Returns a copied list of all boss microgmaes, regardless of completion
	/// </summary>
	/// <returns></returns>
	public List<Stage.Microgame> getBossMicrogames()
	{
		List<Stage.Microgame> returnList = new List<Stage.Microgame>(bossMicrogames);
		return returnList;
	}
}
