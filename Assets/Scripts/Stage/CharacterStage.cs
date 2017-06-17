using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStage : Stage
{
	public static bool revisiting;

	[SerializeField]
	private CharacterMicrogamePool microgamePool;
	[SerializeField]
	private bool shuffleMicrogames = true;
	[SerializeField]
	private Interruption speedUp, bossIntro, nextRound;

	private int roundsComplete, roundStartIndex;
	private bool bossVictoryStatus;

	void Awake()
	{
		roundsComplete = roundStartIndex = 0;
		if (shuffleMicrogames)
			shuffleBatches();
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

	public override int getMicrogameDifficulty(Stage.Microgame microgame)
	{
		return Mathf.Min(microgame.baseDifficulty + roundsComplete, 3);
	}

	public override Interruption[] getInterruptions(int num)
	{
		Microgame microgame = getMicrogame(num);
		int index = getIndex(num);

		//Boss over
		if (roundsComplete > 0 && num == roundStartIndex)
		{
			//TODO more after-boss stuff
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
		for (int i = 0; i < microgamePool.speedUpInstances.Length; i++)
		{
			if (microgamePool.speedUpInstances[i] == index)
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
		return !(getMicrogame(num).microgameId.Equals(microgamePool.bossMicrogame.microgameId) &&
			getMicrogame(num - 1).microgameId.Equals(microgamePool.bossMicrogame.microgameId));
	}

	public override void onMicrogameEnd(int microgame, bool victoryStatus)
	{
		if (getMicrogame(microgame).microgameId.Equals(microgamePool.bossMicrogame.microgameId))
		{
			bossVictoryStatus = victoryStatus;
			if (revisiting)
			{
				roundStartIndex = microgame + 1;
				startNextRound(microgame);
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
		roundsComplete++;
		roundStartIndex = startIndex;
		shuffleBatches();
	}
	
	int getRoundSpeedOffset()
	{
		return (roundsComplete < 3) ? 0 : roundsComplete - 3;
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
