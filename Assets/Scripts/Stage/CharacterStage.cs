using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Rendering;
using UnityEngine.Serialization;
using StageFSM;

[CreateAssetMenu(menuName = "Stage/Character Stage")]
public class CharacterStage : Stage
{
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
    private bool skipBossMicrogame;
    [SerializeField]
    private ForceRevisit forceRevisit = ForceRevisit.Default;

    private enum ForceRevisit
    {
        Default,
        ForceTrue,
        ForceFalse
    }

#if UNITY_EDITOR
    public void SetInternalBatches(List<MicrogameBatch> batches) => microgameBatchesInternal = batches;
#endif

    private MicrogamePool microgamePool;
    private bool revisiting;


    [System.Serializable]
    public class MicrogamePool
    {
        public List<MicrogameBatch> batches;
        public Microgame[] bossGames;
    }


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

	public MicrogamePool GetFullMicrogamePool()
    {
        // Some modifications to preserve the integrity of the 1:1 character-stage relationship and to help editor functions

        var batches = new List<MicrogameBatch>();
        var pool = new MicrogamePool();

        // Load microgames
        var useAllBosses = useAllBossesWhenRevisiting && revisiting;
        var loadedMicrogames = MicrogameCollection.LoadAllMicrogames()
            .Where(a => MicrogameQualifiesForStage(a) || (useAllBosses && a.isBossMicrogame()))
            .ToArray();
        pool.bossGames = loadedMicrogames
            .Where(a => a.isBossMicrogame())
            .ToArray();

        // Filter batches
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
            microgame.difficulty = Mathf.Clamp(microgame.difficulty, 1, 3);
        }

        // Add games that aren't stored internally but belong to this character stage
        var normalMicrogames = loadedMicrogames
            .Where(a => MicrogameQualifiesForStage(a) && !a.isBossMicrogame());
        var flattenedPool = batches
            .SelectMany(a => a.pool)
            .Select(a => a.microgame);
        var missingGames = normalMicrogames.Where(a => !flattenedPool.Contains(a));
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

        pool.batches = batches;
        return pool;
    }

    public override void InitScene()
    {
        base.InitScene();
        if (forceRevisit != ForceRevisit.Default)
            revisiting = forceRevisit == ForceRevisit.ForceTrue;
        else
            revisiting = PrefsHelper.getProgress() > 0;   // TODO temporary setup until new story stages are added
        microgamePool = GetFullMicrogamePool();
    }

	public override StageMicrogame getMicrogame(int num)
	{
        var index = GetIndexInRound(num);
        var difficulty = Mathf.Min((GetRound(num) + 1), 3);
        var rand = GetRandomForRound(GetRound(num));

		for (int i = 0; i < microgamePool.batches.Count; i++)
		{
			var batch = microgamePool.batches[i];
            if (index >= batch.pick)
            {
                index -= batch.pick;
                rand.Next();
                continue;
            }

            StageMicrogame stageMicrogame;
            if (shuffleMicrogames)
                stageMicrogame = GetShuffledMicrogame(batch.pool.ToArray(), index, rand);
            else
                stageMicrogame = batch.pool[index];

            difficulty = Mathf.Min(difficulty + stageMicrogame.difficulty - 1, 3);
            return new StageMicrogame(stageMicrogame.microgame, difficulty);
        }
        var bossMicrogame = GetShuffledMicrogame(microgamePool.bossGames, 0, rand);
        return new StageMicrogame(bossMicrogame, difficulty);
    }

    public override Dictionary<string, bool> GetStateMachineFlags(int microgame, bool lastVictoryResult, int currentLife)
    {
        var dict = base.GetStateMachineFlags(microgame, lastVictoryResult, currentLife);
        var round = GetRound(microgame);
        var roundIndex = GetIndexInRound(microgame);
        dict["SpeedUp"] = speedUpTimes.Contains(roundIndex);
        dict["SpeedUpWarning"] = dict["SpeedUp"];
        dict["BossWarning"] = IsBossIndex(microgame) && !IsBossIndex(microgame - 1);
        dict["LowerScore"] = !revisiting && IsBossIndex(microgame - 1) && !lastVictoryResult;
        dict["LevelUp"] = roundIndex == 0 && round > 0 && revisiting;
        dict["LevelUpWarning"] = dict["LevelUp"];
        dict["OneUp"] = dict["LevelUp"] && revisiting && lastVictoryResult && currentLife < getMaxLife();
        dict["StageVictory"] = !revisiting && lastVictoryResult && IsBossIndex(microgame - 1);
        return dict;
    }

    public override string getDiscordState(int microgameIndex)
    {
        if (!skipBossMicrogame
            && IsBossIndex(microgameIndex))
            return TextHelper.getLocalizedText("discord.boss", "Boss Stage");
        else
            return base.getDiscordState(microgameIndex);
    }

    int GetRoundMicrogameCount()
    {
        return microgamePool.batches.Sum(a => a.pick) + (skipBossMicrogame ? 0 : 1);
    }

    public override int GetRound(int microgame)
    {
        if (!revisiting)
            return 0;
        var roundSize = GetRoundMicrogameCount();
        return (microgame - (microgame % roundSize)) / roundSize;
    }

	int GetIndexInRound(int index)
    {
        if (!revisiting)
            return index;
        return index - (GetRound(index) * GetRoundMicrogameCount());
	}

    bool IsBossIndex(int index)
    {
        return !skipBossMicrogame && GetIndexInRound(index) >= GetRoundMicrogameCount() - 1;
    }
}
