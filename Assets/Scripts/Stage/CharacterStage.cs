using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Rendering;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "Stage/Character Stage")]
public class CharacterStage : Stage
{
	public static bool revisiting;
    private const bool useAllBossesWhenRevisiting = true;

    [SerializeField]
    private bool shuffleMicrogames = true;
    [FormerlySerializedAs("microgameBatches")]
    [SerializeField]
    [HideInInspector]
    private List<MicrogameBatch> microgameBatchesInternal;
    [SerializeField]
    private int[] speedUpTimes;
    [SerializeField]
    private StageMicrogame bossMicrogame;
    [SerializeField]
    private bool skipBossMicrogame;

#if UNITY_EDITOR
    public void SetInternalBatches(List<MicrogameBatch> batches) => microgameBatchesInternal = batches;
#endif
    private int roundsCompleted, roundStartIndex;
	private bool bossWon;
    private StageMicrogame selectedBossMicrogame;
    private Microgame[] loadedMicrogames;
    private List<MicrogameBatch> microgameBatches;


    [System.Serializable]
    public class MicrogameBatch
    {
        public int pick;
        public List<StageMicrogame> pool;

        public void Sort() => pool = pool.OrderBy(a => a.microgame.microgameId).ToList();
    }

    private bool MicrogameQualifiesForStage(Microgame microgame)
    {
        return microgame.CharacterStage == this && microgame.milestone >= Microgame.Milestone.StageReady;
    }

	public List<MicrogameBatch> GetFullMicrogamePool()
    {
        // Some modifications to preserve the integrity of the 1:1 character-stage relationship and to help editor functions

        var batches = new List<MicrogameBatch>();

        if (microgameBatchesInternal == null || !microgameBatchesInternal.Any())
        {
            // There needs to be at least one batch
            var newBatch = new MicrogameBatch();
            newBatch.pool = new List<StageMicrogame>();
            batches.Add(newBatch);
        }
        else
        {
            // Filter out games in internal collection that are not from this character
            foreach (var internalBatch in microgameBatchesInternal)
            {
                var newBatch = new MicrogameBatch();
                newBatch.pick = internalBatch.pick;
                newBatch.pool = internalBatch.pool
                    .Where(a => MicrogameQualifiesForStage(a.microgame) && !a.microgame.isBossMicrogame())
                    .ToList();
                batches.Add(newBatch);
            }
        }

        // Ensure each difficulty is in correct range
        foreach (var microgame in batches.SelectMany(a => a.pool))
        {
            microgame.baseDifficulty = Mathf.Clamp(microgame.baseDifficulty, 1, 3);
        }

        // Add games that aren't stored internally but belong to this character stage
        var allMicrogames = loadedMicrogames
            .Where(a => MicrogameQualifiesForStage(a) && !a.isBossMicrogame());
        var flattenedPool = batches
            .SelectMany(a => a.pool)
            .Select(a => a.microgame);
        var missingGames = allMicrogames.Where(a => !flattenedPool.Contains(a));
        foreach (var missingGame in missingGames)
        {
            batches[0].pool.Add(new StageMicrogame(missingGame));
        }

        // Also make sure we aren't picking an invalid number from any batch
        foreach (var batch in batches)
        {
            batch.pick = Mathf.Clamp(batch.pick, 0, batch.pool.Count);
        }

        // Sort games
        batches.ForEach(a => a.Sort());

        return batches;
    }

    public override void onStageStart(StageController stageController)
    {
        loadedMicrogames = MicrogameCollection.LoadAllMicrogames()
            .Where(a => MicrogameQualifiesForStage(a) || a.isBossMicrogame())
            .ToArray();

        roundsCompleted = roundStartIndex = 0;
        microgameBatches = GetFullMicrogamePool();
        if (shuffleMicrogames)
            shuffleBatches();
        else
            selectedBossMicrogame = bossMicrogame;

        revisiting = PrefsHelper.getProgress() > 0; //TODO replace when we have multiple stage progression


        base.onStageStart(stageController);
    }

	public override StageMicrogame getMicrogame(int num)
	{
		int index = getIndex(num);
		for (int i = 0; i < microgameBatches.Count; i++)
		{
			var batch = microgameBatches[i];
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
        if (!skipBossMicrogame
            && isSelectedBossIndex(microgameIndex))
            return TextHelper.getLocalizedText("discord.boss", "Boss Stage");
        else
            return base.getDiscordState(microgameIndex);
    }

    public override int getMicrogameDifficulty(StageMicrogame microgame, int num)
	{
		return Mathf.Min(microgame.baseDifficulty + roundsCompleted, 3);
	}

	//public override Interruption[] getInterruptions(int num)
	//{
	//	StageMicrogame microgame = getMicrogame(num);
	//	int index = getIndex(num);

 //       //Boss over
 //       if (!revisiting && bossWon)
 //       {
 //           return new Interruption[0].add(wonStage);
 //       }
 //       else if (roundsCompleted > 0 && num == roundStartIndex)
	//	{
 //           //TODO more after-boss stuff
 //           if (bossWon)
 //           {
 //               if (stageController.getLife() < getMaxLife())
 //               {
 //                   stageController.setLife(stageController.getLife() + 1);
 //                   return new Interruption[0].add(oneUp);
 //               }
 //               //TODO separate next round after oneUp when we have music
 //           }
	//		return new Interruption[0].add(nextRound);
 //       }

 //       //Boss Intro
 //       if (isSelectedBoss(microgame))
 //       {
 //           if (isSelectedBossIndex(num - 1))  //Not first boss attempt
 //               return new Interruption[0];
 //           else
 //               return new Interruption[0].add(bossIntro);
 //       }

 //       //Speed up check
 //       for (int i = 0; i < speedUpTimes.Length; i++)
 //       {
 //           if (speedUpTimes[i] == index)
 //               return new Interruption[0].add(speedUp);
 //       }

 //       return new Interruption[0];
	//}

	//public override int getCustomSpeed(int microgame, Interruption interruption)
	//{
	//	if (interruption.animation == StageController.AnimationPart.BossStage
 //           || interruption.animation == StageController.AnimationPart.NextRound
 //           || interruption.animation == StageController.AnimationPart.OneUp)
	//		return 1 + getRoundSpeedOffset();

 //       Debug.Log("no not here");
	//	return 1;
	//}

	public override bool isMicrogameDetermined(int num)
    {
        int index = getIndex(num);
        var totalMicrogameCount = microgameBatches.Sum(a => a.pick);
        
        if (skipBossMicrogame)
            return index < totalMicrogameCount;
        else
            return index <= totalMicrogameCount;
    }

	public override void onMicrogameEnd(int microgame, bool victoryStatus)
    {
        if (skipBossMicrogame)
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
            //else if (victoryStatus)
            //    winStage();
            else
                stageController.lowerScore();
        }
        base.onMicrogameEnd(microgame, victoryStatus);
    }

	//void winStage()
 //   {
 //       PrefsHelper.setProgress(PrefsHelper.GameProgress.StoryComplete);
 //       GameController.instance.sceneShifter.startShift("NitoriSplash", victorySceneShiftTime); //TODO replace when we're past the demo
 //       PrefsHelper.setHighScore(gameObject.scene.name, getRoundMicrogameCount());
	//}

    int getRoundMicrogameCount()
    {
        int batch = 0;
        foreach (var item in microgameBatches)
        {
            batch += item.pick;
        }
        return batch + 1;
    }

	void startNextRound(int startIndex)
	{
		roundsCompleted++;
		roundStartIndex = startIndex;
		if (shuffleMicrogames)
			shuffleBatches();
	}
	
	int getRoundSpeedOffset()
	{
		return (roundsCompleted < 3) ? 0 : roundsCompleted - 2;
	}

	void shuffleBatches()
	{
		for (int i = 0; i < microgameBatches.Count; i++)
		{
			var pool = microgameBatches[i].pool;
			int choice;
			StageMicrogame hold;
			for (int j = 0; j < pool.Count; j++)
			{
				choice = Random.Range(j, pool.Count);
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
            var bossMicrogames = loadedMicrogames
                .Where(a => a.isBossMicrogame())
                .ToList();
            var randomBossData = bossMicrogames[Random.Range(0, bossMicrogames.Count)];
            selectedBossMicrogame = new StageMicrogame(randomBossData, bossMicrogame.baseDifficulty);
        }
        else
            selectedBossMicrogame = bossMicrogame;
	}

	int getIndex(int num)
	{
		return num - roundStartIndex;
	}

    bool isSelectedBoss(StageMicrogame microgame) => microgame.microgame.microgameId.Equals(selectedBossMicrogame.microgame.microgameId);
    bool isSelectedBossIndex(int index) => isSelectedBoss(getMicrogame(index));
}
