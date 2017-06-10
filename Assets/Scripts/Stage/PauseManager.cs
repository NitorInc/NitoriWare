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

	PauseData pauseData;
	private struct PauseData
	{
		public List<AudioSource> pausedAudioSources;
		public List<MonoBehaviour> disabledScripts;
		public int camCullingMask;
		public Color camColor;
	}

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
		pauseData.pausedAudioSources = new List<AudioSource>();
		List<AudioSource> whitelistedAudioSources = new List<AudioSource>(audioSourceWhitelist);
		foreach (AudioSource source in audioSources)
		{
			if (!whitelistedAudioSources.Remove(source) && source.isPlaying)
			{
				source.Pause();
				pauseData.pausedAudioSources.Add(source);
			}
		}

		MonoBehaviour[] scripts = FindObjectsOfType(typeof(MonoBehaviour)) as MonoBehaviour[];
		pauseData.disabledScripts = new List<MonoBehaviour>();
		List<MonoBehaviour> whitelistedScripts = new List<MonoBehaviour>(scriptWhitelist);
		foreach( MonoBehaviour script in scripts)
		{
			if (!whitelistedScripts.Remove(script) && script.enabled && script != this)
			{
				script.enabled = false;
				pauseData.disabledScripts.Add(script);
			}
		}

		onPause.Invoke();
		if (MicrogameController.instance != null)
		{
			MicrogameController.instance.onPause.Invoke();
			pauseData.camCullingMask = Camera.main.cullingMask;
			pauseData.camColor = Camera.main.backgroundColor;
			Camera.main.cullingMask = 0;
			Camera.main.backgroundColor = Color.black;
			MicrogameController.instance.getCommandTransform().FindChild("Text").gameObject.SetActive(false);
		}
		MicrogameTimer.instance.gameObject.SetActive(false);

		paused = true;
	}

	void unPause()
	{
		Time.timeScale = timeScale;

		foreach (AudioSource source in pauseData.pausedAudioSources)
		{
			if (source != null)
				source.UnPause();
		}

		foreach (MonoBehaviour script in pauseData.disabledScripts)
		{
			if (script != null)
				script.enabled = true;
		}

		onUnPause.Invoke();
		if (MicrogameController.instance != null)
		{
			Camera.main.cullingMask = pauseData.camCullingMask;
			Camera.main.backgroundColor = pauseData.camColor;
			MicrogameController.instance.onUnPause.Invoke();
			MicrogameController.instance.getCommandTransform().FindChild("Text").gameObject.SetActive(true);
		}
		MicrogameTimer.instance.gameObject.SetActive(true);

		paused = false;
	}
}
