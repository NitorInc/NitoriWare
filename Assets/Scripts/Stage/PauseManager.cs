using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PauseManager : MonoBehaviour
{
	public UnityEvent onPause, onUnPause;

	[SerializeField]
	//Enable and hold P to pause and unpause frantically
	private bool enableVigorousTesting;

	//Whitelisted items won't be affected by pause
	public MonoBehaviour[] scriptWhitelist;

	[SerializeField]
	private Transform menu;

	private bool paused;

	PauseData pauseData;
	//Varopis data stored on pause that gets reapplied on unpause
	private struct PauseData
	{
		public float timeScale;
		public List<MonoBehaviour> disabledScripts;
		public int camCullingMask;
		public Color camColor;
		public bool cursorVisible;
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
		pauseData.timeScale = Time.timeScale;
		Time.timeScale = 0f;
		AudioListener.pause = true;

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
		pauseData.cursorVisible = Cursor.visible;
		Cursor.visible = true;

		menu.gameObject.SetActive(true);
		paused = true;
	}

	void unPause()
	{
		Time.timeScale = pauseData.timeScale;
		AudioListener.pause = false;

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
		Cursor.visible = pauseData.cursorVisible;

		menu.gameObject.SetActive(false);
		paused = false;
	}
}
