﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class CompilationStage : Stage
{
	[SerializeField]
	protected int microgamesPerRound = 20, microgamesPerSpeedChange = 4;
	[SerializeField]
	private MicrogameTraits.Milestone restriction = MicrogameTraits.Milestone.StageReady;
	[SerializeField]
	protected Interruption nextRound;
    [SerializeField]
    private int seed;
    [Multiline]
    [SerializeField]
    private string overrideCollection;

    //[SerializeField]	//Debug
    private List<Microgame> microgamePool;
    private System.Random shuffleRandom;
    protected int roundsCompleted = 0, roundStartIndex = 0;

	public override void onStageStart()
	{
		microgamePool = (from microgame in MicrogameHelper.getMicrogames(restriction)
                        select new Microgame(microgame.microgameId))
                        .ToList();
        if (!string.IsNullOrEmpty(overrideCollection))
        {
            var overriddenGames = overrideCollection.Split('\n').Select(a => a.ToUpper().Trim((char)13)).ToList();
            microgamePool = microgamePool.Where(a => overriddenGames.Contains(a.microgameId.ToUpper())).ToList();
        }
        roundsCompleted = roundStartIndex = 0;
        
        shuffleRandom = new System.Random(seed == 0 ? (int)System.DateTime.Now.Ticks : seed);
        shuffleGames();

        base.onStageStart();
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
        base.onMicrogameEnd(microgame, victoryStatus);
    }

	void shuffleGames()
	{

        int choice;
        Microgame hold;
        if (microgamesPerRound > microgamePool.Count)
        {
            Debug.LogError("Microgames per round set higher than microgame collection count");
            return;
        }
		for (int j = 0; j < microgamesPerRound; j++)
		{
            choice = (shuffleRandom.Next() % (microgamePool.Count - j)) + j;
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
