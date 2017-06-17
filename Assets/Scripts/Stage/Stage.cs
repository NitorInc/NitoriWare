using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Stage : MonoBehaviour
{
	public VoicePlayer.VoiceSet voiceSet;

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
	}

	[System.Serializable]
	public class  Microgame
	{
		public string microgameId;
		public int baseDifficulty;
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
	public abstract int getMicrogameDifficulty(Microgame microgame);

	/// <summary>
	/// Fetch all animation interruptions between outro and intro segments
	/// </summary>
	/// <param name="cycleIndex"></param>
	/// <returns></returns>
	public abstract Interruption[] getInterruptions(int num);


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
	/// Calculates custom speed change when Custom is selected for Interruption speed change
	/// </summary>
	/// <param name="interruption"></param>
	/// <returns></returns>
	public virtual int getCustomSpeed(int microgame, Interruption interruption)
	{
		return 1;
	}

}
