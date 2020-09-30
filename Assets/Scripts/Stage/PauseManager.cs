using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public static PauseManager instance;

    public static bool disablePause;
    public static bool exitedWhilePaused;

	public UnityEvent onPause, onUnPause;

    [SerializeField]
    private string quitScene = "Title";

    [SerializeField]
	//Enable and hold P to pause and unpause frantically
	private bool enableVigorousTesting;

    [SerializeField]
    private float quitShiftDuration;

	//Whitelisted items won't be affected by pause
	public MonoBehaviour[] scriptWhitelist;

    [SerializeField]
    private AudioSource sfxSource;
    [SerializeField]
    private AudioClip pauseClip;

    public bool Paused { get; private set; }

    private Animator pauseAnimator;

    PauseData pauseData;
	//Varopis data stored on pause that gets reapplied on unpause
	private struct PauseData
	{
		public float timeScale;
		public List<MonoBehaviour> disabledScripts;
		public bool cursorVisible;
        public CursorLockMode cursorLockState;
	}


	private float pauseTimer = 0f;

    void Awake()
    {
        instance = this;
        pauseAnimator = GetComponent<Animator>();
    }

    void Start ()
	{
        transform.position = Vector3.zero;
		Paused = false;
        sfxSource.ignoreListenerPause = true;
        transform.parent = null;
    }
	
	void Update ()
	{
		if (enableVigorousTesting && Input.GetKey(KeyCode.P))
			pauseTimer -= Time.fixedDeltaTime;
		if (Input.GetKeyDown(KeyCode.Escape) || pauseTimer < 0f)
		{
			if (!Paused)
				pause();
      }
	}

	public void pause()
	{
        if (disablePause)
            return;

        Paused = true;

        sfxSource.PlayOneShot(pauseClip);
		pauseData.timeScale = Time.timeScale;
		Time.timeScale = 0f;
		AudioListener.pause = true;

		MonoBehaviour[] scripts = FindObjectsOfType(typeof(MonoBehaviour)) as MonoBehaviour[];
		pauseData.disabledScripts = new List<MonoBehaviour>();
		foreach(MonoBehaviour script in scripts)
		{
			if (script.enabled
                && script.transform.root != transform
                && !scriptWhitelist.Any(a => a.GetType() == script.GetType())
                && !(script.gameObject.layer == gameObject.layer && script.name.ToLower().Contains("text")))
				pauseData.disabledScripts.Add(script);
		}
        foreach (MonoBehaviour script in pauseData.disabledScripts)
        {
            script.enabled = false;
        }

		onPause.Invoke();
		if (MicrogameController.instance != null)
		{
            MicrogameController.instance.onPaused();

            var rootObjects = MicrogameController.instance.gameObject.scene.GetRootGameObjects();
		}
      //  if (MicrogameTimer.instance != null)
		    //MicrogameTimer.instance.gameObject.SetActive(false);

		pauseData.cursorVisible = Cursor.visible;
		Cursor.visible = true;
        pauseData.cursorLockState = Cursor.lockState;
        Cursor.lockState = GameController.DefaultCursorMode;

        pauseAnimator.SetBool("Paused", true);
	}

    /// <summary>
    /// Called when quitting from pause menu, GameController.onSceneLoaded takes over when the scene is loaded and sets components back to normal
    /// </summary>
    public void quit()
    {
        exitedWhilePaused = true;
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

        pauseAnimator.SetTrigger("Quit");
        
        GameController.instance.sceneShifter.startShift(quitScene, quitShiftDuration);
    }

	public void unPause()
    {
        if (disablePause)
            return;



        foreach (MonoBehaviour script in pauseData.disabledScripts)
		{
			if (script != null)
            {
                script.enabled = true;
            }
		}

		if (MicrogameController.instance != null)
		{
            MicrogameController.instance.onUnPaused();
        }
        //if (MicrogameTimer.instance != null)
        //    MicrogameTimer.instance.gameObject.SetActive(true);
        
        Time.timeScale = pauseData.timeScale;
        AudioListener.pause = false;
        Cursor.visible = pauseData.cursorVisible;
        Cursor.lockState = pauseData.cursorLockState;
        pauseAnimator.SetBool("Paused", false);

        onUnPause.Invoke();

        Paused = false;
    }
}
