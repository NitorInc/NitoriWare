using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class CompilationStage : Stage
{
	[SerializeField]
	protected int microgamesPerRound = 20, microgamesPerSpeedChange = 4;
	[SerializeField]
	private bool onlyFinishedMicrogames;
	[SerializeField]
	protected Interruption nextRound;

	//[SerializeField]	//Debug
	private List<Microgame> microgamePool;
	protected int roundsCompleted = 0, roundStartIndex = 0;

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
				if (!microgameId.StartsWith("_") && MicrogameTraits.findMicrogameTraits(microgameId, 1, true).isStageReady)
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
		if (microgame - roundStartIndex >= microgamesPerRound - 1)
		{
			roundsCompleted++;
			roundStartIndex += microgamesPerRound;
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
		num -= roundStartIndex;
		if (num % microgamesPerRound == 0)
			return new Interruption[0].add(nextRound);
		if (microgamesPerSpeedChange > 0 && num % microgamesPerSpeedChange == 0)
			return new Interruption[0].add(new Interruption(Interruption.SpeedChange.SpeedUp));
		return new Interruption[0];
	}

	public override int getCustomSpeed(int microgame, Interruption interruption)
	{
		return 1 + getRoundSpeedOffset();
	}

	int getRoundSpeedOffset()
	{
		return (roundsCompleted < 3) ? 0 : roundsCompleted - 2;
	}
}
