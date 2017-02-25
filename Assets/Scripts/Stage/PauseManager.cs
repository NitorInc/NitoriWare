using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PauseManager : MonoBehaviour
{
	public UnityEvent onPause, onUnPause;

	[SerializeField]
	private bool enableVigorousTesting;

	//Whitelisted items won't be affected by pause
	public AudioSource[] audioSourceWhitelist;
	public MonoBehaviour[] scriptWhitelist;

	private bool paused;
	private float timeScale;

	private List<AudioSource> pausedAudioSources;
	private List<MonoBehaviour> disabledScripts;

	private float pauseTimer = 0f;

	void Start ()
	{
		paused = false;
	}
	
	void Update ()
	{
		if (enableVigorousTesting && Input.GetKey(KeyCode.P))
			pauseTimer -= Time.fixedDeltaTime;
		if (Input.GetKeyDown(KeyCode.Escape) || pauseTimer < 0f)
		{
			if (!paused)
				pause();
			else
				unPause();
			pauseTimer = Random.Range(.1f, .2f);
		}
	}

	void pause()
	{
		timeScale = Time.timeScale;
		Time.timeScale = 0f;

		AudioSource[] audioSources = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
		pausedAudioSources = new List<AudioSource>();
		List<AudioSource> whitelistedAudioSources = new List<AudioSource>(audioSourceWhitelist);
		foreach (AudioSource source in audioSources)
		{
			if (!whitelistedAudioSources.Remove(source) && source.isPlaying)
			{
				source.Pause();
				pausedAudioSources.Add(source);
			}
		}

		MonoBehaviour[] scripts = FindObjectsOfType(typeof(MonoBehaviour)) as MonoBehaviour[];
		disabledScripts = new List<MonoBehaviour>();
		List<MonoBehaviour> whitelistedScripts = new List<MonoBehaviour>(scriptWhitelist);
		foreach( MonoBehaviour script in scripts)
		{
			if (!whitelistedScripts.Remove(script) && script.enabled && script != this)
			{
				script.enabled = false;
				disabledScripts.Add(script);
			}
		}

		onPause.Invoke();
		if (MicrogameController.instance != null)
			MicrogameController.instance.onPause.Invoke();

		paused = true;
	}

	void unPause()
	{
		Time.timeScale = timeScale;

		foreach (AudioSource source in pausedAudioSources)
		{
			if (source != null)
				source.UnPause();
		}

		foreach (MonoBehaviour script in disabledScripts)
		{
			if (script != null)
				script.enabled = true;
		}

		onUnPause.Invoke();
		if (MicrogameController.instance != null)
			MicrogameController.instance.onUnPause.Invoke();

		paused = false;
	}
}
