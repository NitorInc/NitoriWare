using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMicrogamePool : MonoBehaviour
{
	public bool shuffleMicrogames = true;
	public MicrogameBatch[] microgameBatches;
	public int[] speedUpTimes;
	public Stage.Microgame bossMicrogame;
	public bool skipBossMicrogame;

	[System.Serializable]
	public struct MicrogameBatch
	{
		public int pick;
		public Stage.Microgame[] pool;

	}
}
