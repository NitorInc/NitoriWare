using System.Collections;
using System.Collections.Generic;
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

    protected StageController stageController;

	[System.Serializable]
	public class StageMicrogame
	{
        public Microgame microgame;
        public int baseDifficulty;


        public StageMicrogame(Microgame microgame, int baseDifficulty = 1)
		{
            this.microgame = microgame;
			this.baseDifficulty = baseDifficulty;
		}
    }
    public enum SpeedChange
    {
        None,
        SpeedUp,
        ResetSpeed,
        Custom
    }


    /// <summary>
    /// Called when the stage is first started or the player attempts it again after game over, called before any other method
    /// </summary>
    public virtual void onStageStart(StageController stageController)
    {
        this.stageController = stageController;
        updateDiscordStatus(0);
        PrefsHelper.setVisitedStage(name, true);
    }

    /// <summary>
    /// Get the nth microgame (based on total microgames encountered so far, starts at 0)
    /// </summary>
    /// <param name="cycleIndex"></param>
    /// <returns></returns>
    public abstract StageMicrogame getMicrogame(int num);

	/// <summary>
	/// Gets microgame difficulty for this specific instance
	/// </summary>
	/// <param name="microgame"></param>
	/// <param name="num"></param>
	/// <returns></returns>
	public abstract int getMicrogameDifficulty(StageMicrogame microgame, int num);

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
	/// Called when a microgame has finished and passes results
	/// </summary>
	/// <param name="microgame"></param>
	/// <param name="victory"></param>
	public virtual void onMicrogameEnd(int microgame, bool victory)
	{

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
	/// Calculates custom speed when Custom is selected for Interruption speed change
	/// </summary>
	/// <param name="interruption"></param>
	/// <returns></returns>
	public virtual int getCustomSpeed(int microgame)
	{
		return 1;
	}

	/// <summary>
	/// Returns the speed setting to start the stage at
	/// </summary>
	/// <returns></returns>
	public virtual int getStartSpeed()
	{
		return 1;
	}

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

}