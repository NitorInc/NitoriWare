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

#if UNITY_EDITOR
    public void SetInternalBatches(List<MicrogameBatch> batches) => microgameBatchesInternal = batches;
#endif
    private MicrogamePool microgamePool;


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

    public bool Revisiting => PrefsHelper.getProgress() > 0;   // TODO temporary setup until new story stages are added

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
        var useAllBosses = useAllBossesWhenRevisiting && Application.isPlaying && Revisiting;
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

    public override string getDiscordState(int microgameIndex)
    {
        if (!skipBossMicrogame
            && IsBossGameIndex(microgameIndex))
            return TextHelper.getLocalizedText("discord.boss", "Boss Stage");
        else
            return base.getDiscordState(microgameIndex);
    }

    int GetRoundMicrogameCount()
    {
        return microgamePool.batches.Sum(a => a.pool.Count) + (skipBossMicrogame ? 0 : 1);
    }

    int GetRound(int index)
    {
        var roundSize = GetRoundMicrogameCount();
        return (index - (index % roundSize)) / roundSize;
    }

	int GetIndexInRound(int index)
	{
		return index - (GetRound(index) * GetRoundMicrogameCount());
	}

    bool IsBossGameIndex(int index)
    {
        return !skipBossMicrogame && GetIndexInRound(index) == GetRoundMicrogameCount() - 1;
    }
}
