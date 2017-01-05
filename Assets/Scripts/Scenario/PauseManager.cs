using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{

	private bool paused;
	private float timeScale;

	private List<AudioSource> pausedAudioSources;
	private List<MonoBehaviour> disabledScripts;

	void Start ()
	{
		paused = false;
	}
	
	void Update ()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
			if (!paused)
				pause();
			else
				unPause();
	}

	void pause()
	{
		timeScale = Time.timeScale;
		Time.timeScale = 0f;

		AudioSource[] audioSources = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
		pausedAudioSources = new List<AudioSource>();
		foreach (AudioSource source in audioSources)
		{
			if (source.isPlaying)
			{
				source.Pause();
				pausedAudioSources.Add(source);
			}
		}

		MonoBehaviour[] scripts = FindObjectsOfType(typeof(MonoBehaviour)) as MonoBehaviour[];
		disabledScripts = new List<MonoBehaviour>();
		foreach( MonoBehaviour script in scripts)
		{
			if (script.enabled && script != this)
			{
				script.enabled = false;
				disabledScripts.Add(script);
			}
		}

		if (MicrogameController.instance != null)
			MicrogameController.instance.onPause.Invoke();

		paused = true;
	}

	void unPause()
	{
		Time.timeScale = timeScale;

		foreach (AudioSource source in pausedAudioSources)
		{
			source.UnPause();
		}

		foreach (MonoBehaviour script in disabledScripts)
		{
			script.enabled = true;
		}

		if (MicrogameController.instance != null)
			MicrogameController.instance.onUnPause.Invoke();

		paused = false;
	}
}
