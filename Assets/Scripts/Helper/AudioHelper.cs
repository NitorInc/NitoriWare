using UnityEngine;
using System.Collections;

public class AudioHelper
{

	/// <summary>
	/// Plays the source after a scheduled period of time, factoring in dsptime and timescale
	/// </summary>
	/// <param name="source"></param>
	/// <param name="time"></param>
	public static void playScheduled(AudioSource source, float time)
	{
		source.PlayScheduled(AudioSettings.dspTime + (time / Time.timeScale));
	}

	/// <summary>
	/// Finds the appropriate audio pan for a sound effect based on the object's x position
	/// </summary>
	/// <param name="camera"></param>
	/// <param name="position"></param>
	/// <returns></returns>
	public static float getAudioPan(Camera camera, Vector3 position)
	{
		float x = position.x;
		x -= camera.transform.position.x;
		x /= camera.orthographicSize * (4f / 3f);

		return MathHelper.clamp(x, -1f, 1f);
	}

}
