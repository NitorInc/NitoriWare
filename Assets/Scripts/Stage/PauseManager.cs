using System.Collections;
using System.Collections.Generic;
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
	//Enable and hold P to pause and unpause frantically
	private bool enableVigorousTesting;

    [SerializeField]
    private float quitShiftDuration;

	//Whitelisted items won't be affected by pause
	public MonoBehaviour[] scriptWhitelist;

	[SerializeField]
	private Transform menu;
    [SerializeField]
    private Canvas menuCanvas;
    [SerializeField]
    private AudioSource sfxSource;
    [SerializeField]
    private AudioClip pauseClip;

	private bool paused;
    public bool Paused => paused;

	PauseData pauseData;
	//Varopis data stored on pause that gets reapplied on unpause
	private struct PauseData
	{
		public float timeScale;
		public List<MonoBehaviour> disabledScripts;
        public List<CamPauseData> camPauseDatas;
		public bool cursorVisible;
        public CursorLockMode cursorLockState;
	}

    private struct CamPauseData
    {
        public Camera camera;
        public Color backgroundColor;
        public int cullingMask;
        public CameraClearFlags clearFlags;
        public CamPauseData(Camera camera)
        {
            this.camera = camera;
            backgroundColor = camera.backgroundColor;
            cullingMask = camera.cullingMask;
            clearFlags = camera.clearFlags;
        }
    }


	private float pauseTimer = 0f;

    void Awake()
    {
        instance = this;
    }

    void Start ()
	{
        transform.position = Vector3.zero;
		paused = false;
        menuCanvas.worldCamera = Camera.main;
        sfxSource.ignoreListenerPause = true;
        if (transform.root != transform)
            Debug.LogWarning("Pause Controller should be put in hierarchy root!");
    }
	
	void Update ()
	{
		if (enableVigorousTesting && Input.GetKey(KeyCode.P))
			pauseTimer -= Time.fixedDeltaTime;
		if (Input.GetKeyDown(KeyCode.Escape) || pauseTimer < 0f)
		{
			if (!paused)
				pause();
      }
	}

	public void pause()
	{
        if (disablePause)
            return;

        paused = true;

        sfxSource.PlayOneShot(pauseClip);
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
            MicrogameController.instance.onPaused();

            pauseData.camPauseDatas = new List<CamPauseData>();
            var rootObjects = MicrogameController.instance.gameObject.scene.GetRootGameObjects();
            foreach (var rootObject in rootObjects)
            {
                var cams = rootObject.GetComponentsInChildren<Camera>();
                foreach (var cam in cams)
                {
                    pauseData.camPauseDatas.Add(new CamPauseData(cam));
                    cam.cullingMask = 0;
                    cam.clearFlags = CameraClearFlags.SolidColor;
                    cam.backgroundColor = Color.black;
                }
            }
		}
        if (MicrogameTimer.instance != null)
		    MicrogameTimer.instance.gameObject.SetActive(false);

		pauseData.cursorVisible = Cursor.visible;
		Cursor.visible = true;
        pauseData.cursorLockState = Cursor.lockState;
        Cursor.lockState = GameController.DefaultCursorMode;

		menu.gameObject.SetActive(true);
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

        GameController.instance.sceneShifter.startShift(StageController.instance.getStage().getExitScene(), quitShiftDuration);
    }

	public void unPause()
    {
        if (disablePause)
            return;

        paused = false;

        foreach (MonoBehaviour script in pauseData.disabledScripts)
		{
			if (script != null)
            {
                script.enabled = true;
            }
		}

		if (MicrogameController.instance != null)
		{
            foreach (var camPauseData in pauseData.camPauseDatas)
            {
                camPauseData.camera.backgroundColor = camPauseData.backgroundColor;
                camPauseData.camera.cullingMask = camPauseData.cullingMask;
                camPauseData.camera.clearFlags = camPauseData.clearFlags;
            }
            MicrogameController.instance.onUnPaused();
        }
        if (MicrogameTimer.instance != null)
            MicrogameTimer.instance.gameObject.SetActive(true);
        
        Time.timeScale = pauseData.timeScale;
        AudioListener.pause = false;
        Cursor.visible = pauseData.cursorVisible;
        Cursor.lockState = pauseData.cursorLockState;
        menu.gameObject.SetActive(false);

        onUnPause.Invoke();
    }
}
