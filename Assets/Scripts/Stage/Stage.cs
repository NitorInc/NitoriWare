using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Stage : MonoBehaviour
{

	[System.Serializable]
	public struct Interruption
	{
		[SerializeField]
		public int animatorPart;	//Must be >3 to not interfere with default stage parts
		public float beatDuration;
		public SpeedChange speedChange;
		public bool applySpeedChangeOnStart;

		public enum SpeedChange
		{
			None,
			SpeedUp,
			ResetSpeed,
			Custom
		}
	}

	[System.Serializable]
	public struct Microgame
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
	/// Calculates custom speed change when Custom is selected for Interruption speed change
	/// </summary>
	/// <param name="interruption"></param>
	/// <returns></returns>
	public virtual int getCustomSpeed(Interruption interruption)
	{
		return 1;
	}

}
