using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class Stage : ScriptableObject
{
    private const string DiscordStateFormat = "{0} Games";
    private const string DiscordStateFormatSingular = "{0} Game";

    [SerializeField]
	private VoicePlayer.VoiceSet voiceSet;
    [SerializeField]
    private string displayName;

    protected int seed;

	[System.Serializable]
	public class StageMicrogame
	{
        public Microgame microgame;
        public int difficulty;


        public StageMicrogame(Microgame microgame, int difficulty = 1)
		{
            this.microgame = microgame;
			this.difficulty = difficulty;
		}

        public Microgame.Session CreateSession()
        {
            return microgame.CreateSession(difficulty);
        }
    }


    /// <summary>
    /// Called when the stage is first started, called before any other method
    /// </summary>
    public virtual void InitScene()
    {
        PrefsHelper.setVisitedStage(name, true);
    }

    /// <summary>
    /// Called when the stage is first started or the player attempts it again after game over, called after InitScene
    /// </summary>
    public virtual void InitStage(int seed)
    {
        if (seed == 0)
            seed = new System.Random().Next();
        this.seed = seed;
        updateDiscordStatus(0);
    }

    /// <summary>
    /// Get the nth microgame (based on total microgames encountered so far, starts at 0)
    /// </summary>
    /// <param name="cycleIndex"></param>
    /// <returns></returns>
    public abstract StageMicrogame getMicrogame(int num);

	/// <summary>
	/// Returns true if we know for sure what microgame will play at the specific index
	/// </summary>
	/// <param name="num"></param>
	/// <returns></returns>
	public virtual bool isMicrogameDetermined(int num)
	{
		return true;
	}

    /// <summary>
    /// Called when a microgame has started gameplay and scene has activated
    /// </summary>
    /// <param name="microgame"></param>
    public virtual void onMicrogameStart(int microgame)
    {
        updateDiscordState(microgame);
    }

    /// <summary>
    /// For display in Discord's rich presence (first line), called from onMicrogameEnd under normal circumstances
    /// </summary>
    /// <param name="microgameIndex"></param>
    public virtual string getDiscordDetails()
    {
        return TextHelper.getLocalizedText("menu.gamemode." + name, displayName);
    }
    
    void updateDiscordDetails()
    {
        GameController.instance.discord.updatePresence(
            details: getDiscordDetails(),
            startTimeStamp: DiscordController.TimeStampType.CurrentTime);
    }

    /// <summary>
    /// For display in Discord's rich presence (second line), called from onMicrogameEnd under normal circumstances
    /// </summary>
    /// <param name="microgameIndex"></param>
    public virtual string getDiscordState(int microgameIndex)
    {
        int score = microgameIndex + 1;
        string localizedFormat;
        if (score < 2)
            localizedFormat = TextHelper.getLocalizedText("discord.scoreformatsingular", DiscordStateFormatSingular);
        else
            localizedFormat = TextHelper.getLocalizedText("discord.scoreformat", DiscordStateFormat);
        return string.Format(localizedFormat, score.ToString());
    }

    void updateDiscordState(int microgame)
    {
        GameController.instance.discord.updatePresence(state: getDiscordState(microgame));
    }

    void updateDiscordStatus(int microgame)
    {
        GameController.instance.discord.updatePresence(
            getDiscordDetails(),
            getDiscordState(microgame),
            DiscordController.TimeStampType.CurrentTime);
    }

    /// <summary>
    /// Returns max lives in stage, 4 in most cases
    /// </summary>
    /// <returns></returns>
    public virtual int getMaxLife()
	{
		return 4;
    }

    /// <summary>
    /// Get microgame speed at beginning of this round
    /// </summary>
    /// <param name="interruption"></param>
    /// <returns></returns>
    public virtual int GetRoundSpeed(int round) => Mathf.Clamp(round - 1, 1, SpeedController.MaxSpeed);

    /// <summary>
    /// Get microgame speed at beginning of this stage
    /// </summary>
    /// <returns></returns>
    public virtual int getStartSpeed() => GetRoundSpeed(0);

    /// <summary>
    /// Returns voice set used for this stage
    /// </summary>
    /// <returns></returns>
    public VoicePlayer.VoiceSet getVoiceSet()
    {
        return voiceSet;
    }

    /// <summary>
    /// Returns which scene the stage will exit to, called upon exit
    /// </summary>
    /// <returns></returns>
    public virtual string getExitScene()
    {
        return "Title";
    }

    public string getDefaultDisplayName()
    {
        return displayName;
    }

    public virtual Dictionary<string, bool> GetStateMachineFlags(int microgame, bool victoryResult, int currentLife)
    {
        var dict = new Dictionary<string, bool>();
        dict["GameOver"] = currentLife <= 0;
        return dict;
    }

    public abstract int GetRound(int microgameIndex);
    
    protected System.Random GetRandomForRound(int round) => new System.Random(GetSeedForRound(round));

    protected int GetSeedForRound(int round)
    {
        var returnSeed = seed;
        var rand = new System.Random(seed);
        for (int i = 0; i < round; i++)
        {
            returnSeed = rand.Next();
        }
        return returnSeed;
    }

    protected T GetShuffledMicrogame<T>(T[] microgames, int index, System.Random random)
    {
        if (microgames.Length < 2)
            return microgames.FirstOrDefault();

        var indexArray = Enumerable.Range(0, microgames.Length).ToArray();
        for (int i = 0; i <= index; i++)
        {
            var range = indexArray.Length - i;
            var pick = i + (random.Next() % range);
            if (i == index)
                return microgames[indexArray[pick]];
            else if (pick != i)
            {
                var hold = indexArray[pick];
                indexArray[pick] = indexArray[i];
                indexArray[i] = hold;
            }
        }

        UnityEngine.Debug.LogError("Shuffle out of range");
        return microgames.FirstOrDefault();
    }

}