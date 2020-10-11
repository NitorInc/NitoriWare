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
    /// Finds the appropriate audio pan for a sound effect based on the object's x position, custom camera
    /// </summary>
    /// <param name="camera"></param>
    /// <param name="xPosition"></param>
    /// <returns></returns>
    public static float getAudioPan(float xPosition, Camera camera, float extreme = 1f)
    {
        xPosition -= camera.transform.position.x;
        xPosition /= camera.orthographicSize * (4f / 3f);

        return Mathf.Clamp(xPosition, -extreme, extreme);
    }

    /// <summary>
    /// Finds the appropriate audio pan for a sound effect based on the object's x position
    /// </summary>
    /// <param name="xPosition"></param>
    /// <returns></returns>
    public static float getAudioPan(float xPosition, float extreme = 1f)
	{
        return getAudioPan(xPosition, MainCameraSingleton.instance, extreme);
    }

    /// <summary>
    /// Converts a decibal shift value to a volume level between 1 and 0
    /// </summary>
    /// <param name="db"></param>
    /// <returns></returns>
    public static float DecibalsToVolumeLevel(float db)
    {
        if (db <= -80f)
            return 0;
        return Mathf.Pow(10f, db / 20f);
    }

    /// <summary>
    /// Converts a volume level between 1 and 0 to a decibal shift value
    /// </summary>
    /// <param name="db"></param>
    /// <returns></returns>
    public static float VolumeLevelToDecibals(float volume)
    {
        volume = Mathf.Max(volume, 0.0001f);
        return 20f * Mathf.Log10(volume);
    }

}
