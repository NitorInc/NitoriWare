using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMicrogamePool : MonoBehaviour
{
	public MicrogameBatch[] microgameBatches;
	public int[] speedUpInstances;
	public Stage.Microgame bossMicrogame;

	[System.Serializable]
	public struct MicrogameBatch
	{
		public int pick;
		public Stage.Microgame[] pool;

	}
}
