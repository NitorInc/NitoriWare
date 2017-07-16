using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public static bool disablePause;
    public static bool exitedWhilePaused;

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
        if (transform.root != transform)
            Debug.LogWarning("Pause Controller should be put in hierachy root!");
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
        else if (Input.GetKey(KeyCode.Q) && (paused  || StageController.instance.animationPart == StageController.AnimationPart.GameOver))
        {
            //TODO make this a button function
            if (paused)
                quit();
            SceneManager.LoadScene("Menu");
        }
	}

	public void pause()
	{
        if (disablePause)
            return;

		pauseData.timeScale = Time.timeScale;
		Time.timeScale = 0f;
		AudioListener.pause = true;

		MonoBehaviour[] scripts = FindObjectsOfType(typeof(MonoBehaviour)) as MonoBehaviour[];
		pauseData.disabledScripts = new List<MonoBehaviour>();
		List<MonoBehaviour> whitelistedScripts = new List<MonoBehaviour>(scriptWhitelist);
		foreach(MonoBehaviour script in scripts)
		{
			if (script.enabled && script.transform.root != transform &&
                !(script.gameObject.layer == gameObject.layer && script.name.ToLower().Contains("text")))
				pauseData.disabledScripts.Add(script);
		}
        foreach (MonoBehaviour script in whitelistedScripts)
        {
            pauseData.disabledScripts.Remove(script);
        }
        foreach (MonoBehaviour script in pauseData.disabledScripts)
        {
            script.enabled = false;
        }

		onPause.Invoke();
		if (MicrogameController.instance != null)
		{
			MicrogameController.instance.onPause.Invoke();
			pauseData.camCullingMask = Camera.main.cullingMask;
			pauseData.camColor = Camera.main.backgroundColor;
			Camera.main.cullingMask = 0;
			Camera.main.backgroundColor = Color.black;
			//MicrogameController.instance.getCommandDisplay().transform.FindChild("Text").gameObject.SetActive(false);
		}
        if (MicrogameTimer.instance != null)
		    MicrogameTimer.instance.gameObject.SetActive(false);

		pauseData.cursorVisible = Cursor.visible;
		Cursor.visible = true;

		menu.gameObject.SetActive(true);
		paused = true;
	}

    /// <summary>
    /// Called when quitting from pause menu, GameController.onSceneLoaded takes over when the scene is loaded and sets components back to normal
    /// </summary>
    public void quit()
    {
        foreach (MonoBehaviour script in pauseData.disabledScripts)
        {
            if (script != null)
                script.CancelInvoke();
        }

        AudioSource[] allAudioSources = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
        foreach (AudioSource source in allAudioSources)
        {
            source.Stop();
        }

        exitedWhilePaused = true;
    }

	public void unPause()
	{
		foreach (MonoBehaviour script in pauseData.disabledScripts)
		{
			if (script != null)
            {
                script.enabled = true;
            }
		}

		if (MicrogameController.instance != null)
		{
			Camera.main.cullingMask = pauseData.camCullingMask;
			Camera.main.backgroundColor = pauseData.camColor;
			MicrogameController.instance.onUnPause.Invoke();
			//MicrogameController.instance.getCommandTransform().FindChild("Text").gameObject.SetActive(true);
        }
        if (MicrogameTimer.instance != null)
            MicrogameTimer.instance.gameObject.SetActive(true);
        
        Time.timeScale = pauseData.timeScale;
        AudioListener.pause = false;
        Cursor.visible = pauseData.cursorVisible;
        menu.gameObject.SetActive(false);

        onUnPause.Invoke();
        paused = false;
    }
}
