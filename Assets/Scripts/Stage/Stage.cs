using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Stage : MonoBehaviour
{

	[System.Serializable]
	public struct Interruption
	{
		[SerializeField]
		private string name;	//Not actually used, but makes inspector editing easier
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

		[HideInInspector]
		public AsyncOperation asyncOperation;
	}

	public abstract Microgame getMicrogame(int cycleIndex);

	public virtual int getCustomSpeed(Interruption interruption)
	{
		return 1;
	}

	public abstract Interruption[] getInterruptions(int cycleIndex);

}
