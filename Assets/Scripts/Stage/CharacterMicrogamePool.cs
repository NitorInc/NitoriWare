using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMicrogamePool : MonoBehaviour
{
	[SerializeField]
	private MicrogameBatch[] microgameBatches;
	[SerializeField]
	private Stage.Microgame bossMicrogame;

	[System.Serializable]
	public struct MicrogameBatch
	{
		public int pick;
		public Stage.Microgame[] pool;

	}
}
