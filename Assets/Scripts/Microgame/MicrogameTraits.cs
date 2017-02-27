using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MicrogameTraits : MonoBehaviour
{
	public ControlScheme controlScheme;
	public bool hideCursor;
	public Duration duration;
	public bool canEndEarly;
	public string command;
	public bool defaultVictory;
	public float victoryVoiceDelay, failureVoiceDelay;
	public AudioClip musicClip;

	public enum ControlScheme
	{
		Touhou,
		Mouse
	}

	public enum Duration
	{
		Short8Beats,
		Long16Beats
	}

	public float getDurationInBeats()
	{
		return duration == Duration.Short8Beats ? 8f : 16f;
	}

	public static MicrogameTraits findMicrogameTraits(string microgameId, int difficulty)
	{
		GameObject traits = Resources.Load<GameObject>("Microgames/_Finished/" + microgameId + "/Traits" + difficulty.ToString());
		if (traits != null)
			return traits.GetComponent<MicrogameTraits>();

		traits = Resources.Load<GameObject>("Microgames/" + microgameId + "/Traits" + difficulty.ToString());
		if (traits != null)
			return traits.GetComponent<MicrogameTraits>();

		traits = Resources.Load<GameObject>("Microgames/_Bosses/" + microgameId + "/Traits" + difficulty.ToString());
		if (traits != null)
			return traits.GetComponent<MicrogameTraits>();

		Debug.LogError("Can't find Traits prefab for " + microgameId + difficulty.ToString());
		return null;
	}
}