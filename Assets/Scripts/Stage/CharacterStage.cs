using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStage : Stage
{
	[SerializeField]
	private CharacterMicrogamePool microgamePool;
	[SerializeField]
	private bool revisiting;
	[SerializeField]
	private bool shuffleMicrogames = true;

	private int round, roundStartIndex;

	void Awake()
	{
		round = roundStartIndex = 0;
		if (shuffleMicrogames)
			shuffleBatches();
	}

	public override Microgame getMicrogame(int num)
	{
		return getMicrogameAt(getIndex(num));
	}

	public override int getMicrogameDifficulty(Stage.Microgame microgame)
	{
		return Mathf.Min(microgame.baseDifficulty + round, 3);
	}

	public override Interruption[] getInterruptions(int num)
	{
		Microgame game = getMicrogame(num);
		num = getIndex(num);
		return new Interruption[0];
	}

	public override bool isMicrogameDetermined(int num)
	{
		return !(getMicrogame(num).microgameId.Equals(microgamePool.bossMicrogame.microgameId) &&
			getMicrogame(num - 1).microgameId.Equals(microgamePool.bossMicrogame.microgameId));
	}

	public override void onMicrogameEnd(int microgame, bool victory)
	{
		if (getMicrogame(microgame).microgameId.Equals(microgamePool.bossMicrogame.microgameId) && victory)
		{
			if (revisiting)
			{
				roundStartIndex = microgame + 1;
				nextRound();
			}
			else
				winStage();
		}
	}

	void winStage()
	{
		//TODO
	}

	void nextRound()
	{
		//TODO
		shuffleBatches();
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

	//TODO remove if not needed
	Microgame getMicrogameAt(int index)
	{
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

	int getIndex(int num)
	{
		return num - roundStartIndex;
	}
}
