using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{

	private bool paused;
	private float timeScale;

	private AudioSource[] audioSources;

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

		audioSources = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
		for (int i = 0; i < audioSources.Length; i++)
		{
			audioSources[i].Pause();
		}

		paused = true;
	}

	void unPause()
	{
		Time.timeScale = timeScale;

		for (int i = 0; i < audioSources.Length; i++)
		{
			audioSources[i].UnPause();
		}

		paused = false;
	}
}
