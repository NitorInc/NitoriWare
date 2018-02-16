using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Stage : MonoBehaviour
{
    private const string DiscordStateFormat = "{0} Games";

#pragma warning disable 0649    //Serialized Fields
    [SerializeField]
	private VoicePlayer.VoiceSet voiceSet;
#pragma warning restore 0649

    [System.Serializable]
	public class Interruption
	{
		public StageController.AnimationPart animation;
		public float beatDuration;
		public AudioSource audioSource;
		public AudioClip audioClip;
		public SpeedChange speedChange;
		public bool applySpeedChangeAtEnd;

		public enum SpeedChange
		{
			None,
			SpeedUp,
			ResetSpeed,
			Custom
		}

		[HideInInspector]
		public float scheduledPlayTime;

		public Interruption(SpeedChange speedChange = SpeedChange.None)
		{
			this.speedChange = speedChange;
		}
	}

	[System.Serializable]
	public class  Microgame
	{
		public string microgameId;
		public int baseDifficulty = 1;

		public Microgame(string microgameId = "", int baseDifficulty = 1)
		{
			this.microgameId = microgameId;
			this.baseDifficulty = baseDifficulty;
		}
	}

	/// <summary>
	/// Called when the stage is first started or the player attempts it again, called before any other method
	/// </summary>
	public virtual void onStageStart()
	{
        PrefsHelper.setVisitedStage(gameObject.scene.name, true);
	}

	/// <summary>
	/// Get the nth microgame (based on total microgmaes encountered so far, starts at 0)
	/// </summary>
	/// <param name="cycleIndex"></param>
	/// <returns></returns>
	public abstract Microgame getMicrogame(int num);

	/// <summary>
	/// Gets microgame difficulty for this specific instance
	/// </summary>
	/// <param name="microgame"></param>
	/// <param name="num"></param>
	/// <returns></returns>
	public abstract int getMicrogameDifficulty(Microgame microgame, int num);

	/// <summary>
	/// Fetch all animation interruptions between outro and intro segments
	/// </summary>
	/// <returns></returns>
	public abstract Interruption[] getInterruptions(int num);
	/// <param name="cycleIndex"></param>


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
	/// Called when a microgame has finished and passes results
	/// </summary>
	/// <param name="microgame"></param>
	/// <param name="victory"></param>
	public virtual void onMicrogameEnd(int microgame, bool victory)
	{
        GameController.instance.discord.updatePresenceState(getDiscordState(microgame + 2), false);
    }

    
    /// <summary>
    /// For display in Discord's rich presence, called from onMicrogameEnd under normal circumstances
    /// </summary>
    /// <param name="score"></param>
    public virtual string getDiscordState(int score)
    {
        return string.Format(DiscordStateFormat, score.ToString());
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
	public virtual int getCustomSpeed(int microgame, Interruption interruption)
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

}


public static class StageHelper
{
	public static Stage.Interruption[] add(this Stage.Interruption[] interruptions, Stage.Interruption interruption)
	{
		Stage.Interruption[] newInterruptions = new Stage.Interruption[interruptions.Length + 1];
		for (int i = 0; i < interruptions.Length; i++)
		{
			newInterruptions[i] = interruptions[i];
		}

		newInterruptions[interruptions.Length] = interruption;
		return newInterruptions;
	}
}
