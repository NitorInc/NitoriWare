using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CharacterStage : Stage
{
	public static bool revisiting;

#pragma warning disable 0649
    [SerializeField]
	private CharacterMicrogamePool microgamePool;
	[SerializeField]
	private Interruption speedUp, bossIntro, nextRound, oneUp, wonStage;
    [SerializeField]
    private float victorySceneShiftTime = 5f;
    [SerializeField]
    private bool useAllBossesWhenRevisiting = true;
#pragma warning restore 0649

    private int roundsCompleted, roundStartIndex;
	private bool bossWon;
    private Microgame selectedBossMicrogame;

	public override void onStageStart()
    {
        roundsCompleted = roundStartIndex = 0;
		if (microgamePool.shuffleMicrogames)
			shuffleBatches();

        revisiting = PrefsHelper.getProgress() > 0; //TODO replace when we have multiple stage progression

        base.onStageStart();
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
		return selectedBossMicrogame;
	}

    public override string getDiscordState(int microgameIndex)
    {
        if (!microgamePool.skipBossMicrogame
            && isSelectedBossIndex(microgameIndex))
            return TextHelper.getLocalizedText("discord.boss", "Boss Stage");
        else
            return base.getDiscordState(microgameIndex);
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
        if (!revisiting && bossWon)
        {
            return new Interruption[0].add(wonStage);
        }
        else if (roundsCompleted > 0 && num == roundStartIndex)
		{
            //TODO more after-boss stuff
            if (bossWon)
            {
                if (StageController.instance.getLife() < getMaxLife())
                {
                    StageController.instance.setLife(StageController.instance.getLife() + 1);
                    return new Interruption[0].add(oneUp);
                }
                //TODO separate next round after oneUp when we have music
            }
			return new Interruption[0].add(nextRound);
        }

        //Boss Intro
        if (isSelectedBoss(microgame))
        {
            if (isSelectedBossIndex(num - 1))  //Not first boss attempt
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
		if (interruption.animation == StageController.AnimationPart.BossStage
            || interruption.animation == StageController.AnimationPart.NextRound
            || interruption.animation == StageController.AnimationPart.OneUp)
			return 1 + getRoundSpeedOffset();

        Debug.Log("no not here");
		return 1;
	}

	public override bool isMicrogameDetermined(int num)
    {
        int index = getIndex(num);
        var totalMicrogameCount = microgamePool.microgameBatches.Sum(a => a.pick);

        print(index);
        print(totalMicrogameCount);
        if (microgamePool.skipBossMicrogame)
            return index < totalMicrogameCount;
        else
            return index <= totalMicrogameCount;
    }

	public override void onMicrogameEnd(int microgame, bool victoryStatus)
    {
        if (microgamePool.skipBossMicrogame)
		{
			if (isSelectedBossIndex(microgame + 1))
			{
				startNextRound(microgame + 1);
			}
			return;
		}

		if (isSelectedBossIndex(microgame))
        {
            bossWon = victoryStatus;
            if (revisiting)
            {
                startNextRound(microgame + 1);
            }
            else if (victoryStatus)
                winStage();
            else
                StageController.instance.lowerScore();
        }
        base.onMicrogameEnd(microgame, victoryStatus);
    }

	void winStage()
    {
        PrefsHelper.setProgress(PrefsHelper.GameProgress.StoryComplete);
        GameController.instance.sceneShifter.startShift("NitoriSplash", victorySceneShiftTime); //TODO replace when we're past the demo
        PrefsHelper.setHighScore(gameObject.scene.name, getRoundMicrogameCount());
	}

    int getRoundMicrogameCount()
    {
        int batch = 0;
        foreach (var item in microgamePool.microgameBatches)
        {
            batch += item.pick;
        }
        return batch + 1;
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
        if (revisiting && useAllBossesWhenRevisiting)
        {
            var bossMicrogames = MicrogameCollection.instance.BossMicrogames;
            var randomBossData = bossMicrogames[Random.Range(0, bossMicrogames.Count)];
            selectedBossMicrogame = new Microgame(randomBossData.microgameId, microgamePool.bossMicrogame.baseDifficulty);
        }
        else
            selectedBossMicrogame = microgamePool.bossMicrogame;
	}

	int getIndex(int num)
	{
		return num - roundStartIndex;
	}

    bool isSelectedBoss(Microgame microgame) => microgame.microgameId.Equals(selectedBossMicrogame.microgameId);
    bool isSelectedBossIndex(int index) => isSelectedBoss(getMicrogame(index));
}
