using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CharacterMicrogamePool : MonoBehaviour
{
	public bool shuffleMicrogames = true;
	public MicrogameBatch[] microgameBatches;
	public int[] speedUpTimes;
    public Stage.StageMicrogame bossMicrogame;
    public bool skipBossMicrogame;

	[System.Serializable]
	public struct MicrogameBatch
	{
		public int pick;
		public Stage.StageMicrogame[] pool;

	}

    void Update()
    {
        if (Application.isPlaying)
            return;

        for (int i = 0; i < microgameBatches.Length; i++)
        {
            microgameBatches[i].pick = Mathf.Min(microgameBatches[i].pool.Length, microgameBatches[i].pick);
        }
    }
}
