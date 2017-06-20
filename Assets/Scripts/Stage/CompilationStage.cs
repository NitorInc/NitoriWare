using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class CompilationStage : Stage
{
	[SerializeField]
	private int microgamesPerRound = 20;
	[SerializeField]
	private bool onlyFinishedMicrogames;

	[SerializeField]	//TODO remove
	private List<Microgame> microgamePool;
	private int roundsCompleted = 0, roundStartIndex = 0;

	void Awake()
	{
		microgamePool = new List<Microgame>();
		string[] finishedMicrogameDirectories = Directory.GetDirectories(Application.dataPath + "/Resources/Microgames/_Finished/");

		for (int i = 0; i < finishedMicrogameDirectories.Length; i++)
		{
			string[] dirs = finishedMicrogameDirectories[i].Split('/');
			string microgameId = dirs[dirs.Length - 1];
			microgamePool.Add(new Microgame(microgameId));
		}

		if (!onlyFinishedMicrogames)
		{
			string[] microgameDirectories = Directory.GetDirectories(Application.dataPath + "/Resources/Microgames/");
			for (int i = 0; i < microgameDirectories.Length; i++)
			{
				string[] dirs = microgameDirectories[i].Split('/');
				string microgameId = dirs[dirs.Length - 1];
				if (!microgameId.StartsWith("_"))	//TODO is ready for compilation on microgame traits
					microgamePool.Add(new Microgame(microgameId));
			}
		}

		shuffleGames();
	}

	public override bool isMicrogameDetermined(int num)
	{
		return (num - roundStartIndex < microgamesPerRound);
	}

	public override Microgame getMicrogame(int num)
	{
		Microgame microgame = microgamePool[num - roundStartIndex];
		return microgame;
	}

	public override int getMicrogameDifficulty(Microgame microgame, int num)
	{
		return Mathf.Min(roundsCompleted + 1, 3);
	}

	public override void onMicrogameEnd(int microgame, bool victoryStatus)
	{
		if (microgame - roundStartIndex >= microgamesPerRound)
		{
			roundsCompleted += microgamesPerRound;
			shuffleGames();
		}
	}

	void shuffleGames()
	{
		int choice;
		Microgame hold;
		for (int j = 0; j < microgamesPerRound; j++)
		{
			choice = Random.Range(j, microgamePool.Count);
			if (choice != j)
			{
				hold = microgamePool[j];
				microgamePool[j] = microgamePool[choice];
				microgamePool[choice] = hold;
			}
		}
	}

	public override Interruption[] getInterruptions(int num)
	{
		return new Interruption[0];
	}
}
