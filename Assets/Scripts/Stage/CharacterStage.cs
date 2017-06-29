using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStage : Stage
{
	public static bool revisiting;

#pragma warning disable 0649
    [SerializeField]
	private CharacterMicrogamePool microgamePool;
	[SerializeField]
	private Interruption speedUp, bossIntro, nextRound;
#pragma warning restore 0649

    private int roundsCompleted, roundStartIndex;
	private bool bossWon;

	public override void onStageStart()
	{
		roundsCompleted = roundStartIndex = 0;
		if (microgamePool.shuffleMicrogames)
			shuffleBatches();

		revisiting = true;	//TODO remove
	}

	public override Microgame getMicrogame(int num)
	{
		int index = getIndex(num);
		for (int i = 0; i < microgamePool.microgameBatches.Length; i++)
		{
			CharacterMicrogamePool.MicrogameBatch batch = microgamePool.microgameBatches[i];
			for (int j = 0; j < batch.pick; j++)
			{
				if (index == 0)
					return batch.pool[j];
				index--;
			}
		}
		return microgamePool.bossMicrogame;
	}

	public override int getMicrogameDifficulty(Stage.Microgame microgame, int num)
	{
		return Mathf.Min(microgame.baseDifficulty + roundsCompleted, 3);
	}

	public override Interruption[] getInterruptions(int num)
	{
		Microgame microgame = getMicrogame(num);
		int index = getIndex(num);

		//Boss over
		if (roundsCompleted > 0 && num == roundStartIndex)
		{
            //TODO more after-boss stuff
            if (bossWon)
            {
                //TODO 1-up
            }
			return new Interruption[0].add(nextRound);
		}

		//Boss Intro
		if (microgame.microgameId.Equals(microgamePool.bossMicrogame.microgameId))
		{
			if (getMicrogame(num - 1).microgameId.Equals(microgamePool.bossMicrogame.microgameId))	//Not first boss attempt
				return new Interruption[0];
			else
				return new Interruption[0].add(bossIntro);
		}

		//Speed up check
		for (int i = 0; i < microgamePool.speedUpTimes.Length; i++)
		{
			if (microgamePool.speedUpTimes[i] == index)
				return new Interruption[0].add(speedUp);
		}
		
		return new Interruption[0];
	}

	public override int getCustomSpeed(int microgame, Interruption interruption)
	{
		if (interruption.animation == StageController.AnimationPart.BossStage || interruption.animation == StageController.AnimationPart.NextRound)
			return 1 + getRoundSpeedOffset();
		return 1;
	}

	public override bool isMicrogameDetermined(int num)
	{
		if (microgamePool.skipBossMicrogame)
			return !getMicrogame(num).microgameId.Equals(microgamePool.bossMicrogame.microgameId);

		return !(getMicrogame(num).microgameId.Equals(microgamePool.bossMicrogame.microgameId) &&
			getMicrogame(num - 1).microgameId.Equals(microgamePool.bossMicrogame.microgameId));
	}

	public override void onMicrogameEnd(int microgame, bool victoryStatus)
	{
		if (microgamePool.skipBossMicrogame)
		{
			if (getMicrogame(microgame + 1).microgameId.Equals(microgamePool.bossMicrogame.microgameId))
			{
				startNextRound(microgame + 1);
			}
			return;
		}

		if (getMicrogame(microgame).microgameId.Equals(microgamePool.bossMicrogame.microgameId))
		{
			bossWon = victoryStatus;
			if (revisiting)
			{
				if (victoryStatus)	//TODO remove when proper boss win is added
				{
					startNextRound(microgame + 1);
				}
			}
			else if (victoryStatus)
				winStage();
		}
	}

	void winStage()
	{
		//TODO
	}

	void startNextRound(int startIndex)
	{
		roundsCompleted++;
		roundStartIndex = startIndex;
		if (microgamePool.shuffleMicrogames)
			shuffleBatches();
	}
	
	int getRoundSpeedOffset()
	{
		return (roundsCompleted < 3) ? 0 : roundsCompleted - 2;
	}

	void shuffleBatches()
	{
		for (int i = 0; i < microgamePool.microgameBatches.Length; i++)
		{
			Microgame[] pool = microgamePool.microgameBatches[i].pool;
			int choice;
			Microgame hold;
			for (int j = 0; j < pool.Length; j++)
			{
				choice = Random.Range(j, pool.Length);
				if (choice != j)
				{
					hold = pool[j];
					pool[j] = pool[choice];
					pool[choice] = hold;
				}
			}
		}
	}

	int getIndex(int num)
	{
		return num - roundStartIndex;
	}
}
