using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using System.Collections;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MicrogameController : MonoBehaviour
{
	public static MicrogameController instance;
	private static int preserveDebugSpeed = -1;
    private static int langaugeCycleIndex = 0;
    private static Microgame.MicrogameSession holdDebugSession;
    private static DebugSettings holdDebugSettings;

    [SerializeField]
	private DebugSettings debugSettings;
	[System.Serializable]
	struct DebugSettings
	{
		public bool playMusic, displayCommand, showTimer, timerTick, simulateStartDelay, localizeText;
        public string forceLocalizationLanguage;
        public bool resetThroughAllLanguages;
		public VoicePlayer.VoiceSet voiceSet;
		[Range(1, StageController.MAX_SPEED)]
		public int speed;
        [Header("For microgames where difficulty isn't dependent on scene:")]
        public DebugDifficulty SimulateDifficulty;
    }

    [SerializeField]
    private DebugKeys debugKeys;
    [System.Serializable]
    class DebugKeys
    {
        public KeyCode Restart = KeyCode.R;
        public KeyCode Faster = KeyCode.F;
        public KeyCode NextDifficulty = KeyCode.N;
        public KeyCode PreviousDifficulty = KeyCode.M;
    }

    public enum DebugDifficulty
    {
        Default,
        Stage1,
        Stage2,
        Stage3
    }


	public UnityEvent onPause, onUnPause;
    [Header("--NOTE--")]
    [Header("Please don't touch anything below here in this GameObject.")]
    [Header("--------")]

    [SerializeField]
    private AudioSource sfxSource;
    
    private bool debugMode;
    private CommandDisplay commandDisplay;

    public Microgame.MicrogameSession session { get; private set; }
    public Microgame microgame => session.microgame;
    public string microgameId => microgame.microgameId;
    public int difficulty => session.Difficulty;

    void Awake()
	{
		instance = this;

        var sceneName = gameObject.scene.name;
        if (sceneName.Contains("Template"))
        {
            Debug.Break();
            Debug.Log("You can't play the template scene.");
        }

        // Get collection microgame if available
        var microgame = MicrogameHelper.getMicrogames(includeBosses:true)
            .FirstOrDefault(a =>  sceneName.Contains(a.microgameId));

        // Otherwise create collection microgame
        if (microgame == null)
        {
#if UNITY_EDITOR
            microgame = MicrogameCollection.GetDebugModeMicrogame(gameObject.scene.name);
#else
            Debug.LogError("Failed to find microgame for " + gameObject.scene.name);
#endif
        }

        if (microgame == null)
        {
            Debug.LogError("Could not find microgame metadata. Make sure the scene name contains the microgame's ID and the folder is named correctly, and that your Microgame's metadata is where it should be.");
            Debug.Break();
        }

        debugMode = GameController.instance == null || GameController.instance.getStartScene() == "Microgame Debug";

        if (debugMode)
		{
            //Debug Mode Awake (scene open by itself)

            if (holdDebugSession != null)
            {
                session = holdDebugSession;
                debugSettings = holdDebugSettings;
            }
            else
            {
                holdDebugSettings = debugSettings;

                int difficulty;
                if (microgame.SceneDeterminesDifficulty)
                    difficulty = microgame.GetDifficultyFromScene(gameObject.scene.name);
                else
                    difficulty = debugSettings.SimulateDifficulty > 0 ? (int)debugSettings.SimulateDifficulty : 1;
                session = microgame.CreateDebugSession(difficulty);

                if (!session.SceneName.Equals(gameObject.scene.name))
                {
                    holdDebugSession = session;
                    SceneManager.LoadScene(session.SceneName);
                    return;
                }
            }


            if (MicrogameDebugObjects.instance == null)
                SceneManager.LoadScene("Microgame Debug", LoadSceneMode.Additive);
            else
                MicrogameDebugObjects.instance.Reset();

            if (preserveDebugSpeed > -1)
            {
                debugSettings.speed = preserveDebugSpeed;
                preserveDebugSpeed = -1;
            }

            StageController.beatLength = 60f / 130f;
            Time.timeScale = StageController.getSpeedMult(debugSettings.speed);
        }
		else
        {
            //Normal Awake

            session = MicrogameSessionManager.ActiveSessions
                .FirstOrDefault(a => a.SceneName.Equals(gameObject.scene.name) && a.State == Microgame.MicrogameSession.SessionState.Loading);

            if (session == null || isBeingDiscarded())
                return;


            session.State = Microgame.MicrogameSession.SessionState.Playing;
            
            session.microgamePlayer.onMicrogameAwake(this, session);

            Cursor.visible = microgame.controlScheme == Microgame.ControlScheme.Mouse && !session.HideCursor;
        }

	}

	void Start()
	{
        if (session == null || isBeingDiscarded())
        {
            shutDownMicrogame();
            return;
        }
        else
        {
            if (debugMode)
            {
                //Debug Start
                MicrogameDebugObjects debugObjects = MicrogameDebugObjects.instance;
                commandDisplay = debugObjects.commandDisplay;

                if (debugSettings.localizeText)
                {
                    LocalizationManager manager = GameController.instance.transform.Find("Localization").GetComponent<LocalizationManager>();
                    if (!string.IsNullOrEmpty(debugSettings.forceLocalizationLanguage))
                        manager.setForcedLanguage(debugSettings.forceLocalizationLanguage);
                    else if (debugSettings.resetThroughAllLanguages)
                    {
                        var languages = LanguagesData.instance.languages;
                        var currentLanguageName = languages[langaugeCycleIndex++].getLanguageID();
                        if (LocalizationManager.instance != null)
                            manager.setLanguage(currentLanguageName);
                        else
                            manager.setForcedLanguage(currentLanguageName);
                        if (langaugeCycleIndex >= languages.Count())
                            langaugeCycleIndex = 0;
                        print("Language cycling debugging in " + currentLanguageName);
                    }
                    manager.gameObject.SetActive(true);
                }

                MicrogameTimer.instance.beatsLeft = (float)microgame.getDurationInBeats() + (debugSettings.simulateStartDelay ? 1f : 0f);
                if (!debugSettings.showTimer)
                    MicrogameTimer.instance.disableDisplay = true;
                if (debugSettings.timerTick)
                    MicrogameTimer.instance.invokeTick();

                var musicClip = session.MusicClip;
                if (debugSettings.playMusic && musicClip != null)
                {
                    AudioSource source = debugObjects.musicSource;
                    source.clip = musicClip;
                    source.pitch = StageController.getSpeedMult(debugSettings.speed);
                    if (!debugSettings.simulateStartDelay)
                        source.Play();
                    else
                        AudioHelper.playScheduled(source, StageController.beatLength);
                }

                if (debugSettings.displayCommand)
                    debugObjects.commandDisplay.play(session.GetLocalizedCommand(), session.CommandAnimatorOverride);

                Cursor.visible = microgame.controlScheme == Microgame.ControlScheme.Mouse && !session.HideCursor;
                Cursor.lockState = session.cursorLockMode;

                debugObjects.voicePlayer.loadClips(debugSettings.voiceSet);

            }
            SceneManager.SetActiveScene(gameObject.scene);
        }
	}

    public void onPaused()
    {
        onPause.Invoke();
    }

    public void onUnPaused()
    {
        onUnPause.Invoke();
    }

    /// <summary>
    /// Disables all root objects in microgame
    /// </summary>
    public void shutDownMicrogame()
    {
        GameObject[] rootObjects = gameObject.scene.GetRootGameObjects();
        foreach (var rootObject in rootObjects)
        {
            rootObject.SetActive(false);

            //Is there a better way to do this?
            var monobehaviours = rootObject.GetComponentsInChildren<MonoBehaviour>();
            foreach (var behaviour in monobehaviours)
            {
                behaviour.CancelInvoke();
            }
        }
    }

    bool isBeingDiscarded()
	{
        if (debugMode)
            return false;
        return session.State == Microgame.MicrogameSession.SessionState.Unloading;
	}

	/// <summary>
	/// Returns MicrogameTraits for the microgame (from the prefab)
	/// </summary>
	/// <returns></returns>
	public Microgame getTraits()
	{
		return microgame;
	}

	/// <summary>
	/// Returns true if microgame is in debug mode (scene open by itself)
	/// </summary>
	/// <returns></returns>
	public bool isDebugMode()
	{
        return debugMode;
	}

    string getSceneWithoutNumber(string scene)
    {
        return scene.Substring(0, scene.Length - 1);
    }
    
    public AudioSource getSFXSource()
    {
        return sfxSource;
    }

    /// <summary>
    /// Call this to have the player win/lose a microgame, set 'final' to false if the victory status might be changed again before the microgame is up
    /// </summary>
    /// <param name="victory"></param>
    /// <param name="final"></param>
    public void setVictory(bool victory, bool final = true)
	{
        bool finalize = final && !session.WasVictoryDetermined;

        session.VictoryStatus = victory;
        session.WasVictoryDetermined = final;

        if (debugMode)
        {
            if (finalize)
            {
                MicrogameDebugObjects.instance.voicePlayer.playClip(victory, victory
                    ? session.VictoryVoiceDelay
                    : session.FailureVoiceDelay);

            }
        }
        else
        {
            session.microgamePlayer.setMicrogameVictory(session, victory, finalize);

        }
    }

    /// <summary>
    /// Returns whether the game would be won if it ends now
    /// </summary>
    /// <returns></returns>
    public bool getVictory() => session.VictoryStatus;

    /// <summary>
    /// Returns true if the game's victory outcome will not be changed for the rest of its duration
    /// </summary>
    /// <returns></returns>
    public bool getVictoryDetermined() => session.WasVictoryDetermined;

	/// <summary>
	/// Re-displays the command text with the specified message. Only use this if the text will not need to be localized
	/// </summary>
	/// <param name="command"></param>
	public void displayCommand(string command, AnimatorOverrideController commandAnimatorOverride = null)
	{
        if (debugMode)
        {
            if (!commandDisplay.gameObject.activeInHierarchy)
                commandDisplay.gameObject.SetActive(true);


            commandDisplay.play(command, commandAnimatorOverride);
        }
        else
            session.microgamePlayer.DisplayExtraCommand(session, command, commandAnimatorOverride);
    }

    /// <summary>
    /// Gets the currently active command display
    /// </summary>
    /// <returns></returns>
    public CommandDisplay getCommandDisplay()
    {
        return commandDisplay;
    }

	/// <summary>
	/// Re-displays the command text with a localized message. Key is automatically prefixed with "microgame.[ID]."
	/// </summary>
	/// <param name="command"></param>
	public void displayLocalizedCommand(string key, string defaultString, AnimatorOverrideController commandAnimatorOverride = null)
	{
		displayCommand(TextHelper.getLocalizedMicrogameText(key, defaultString), commandAnimatorOverride);
	}

    /// <summary>
    /// Plays sound effect unaffected by microgame speed
    /// </summary>
    /// <param name="clip"></param>
    /// <param name="panStereo"></param>
    /// <param name="pitch"></param>
    /// <param name="volume"></param>
    public void playSFXUnscaled(AudioClip clip, float panStereo = 0f, float pitch = 1f, float volume = 1f)
    {
        sfxSource.pitch = pitch;
        sfxSource.panStereo = panStereo;
        sfxSource.PlayOneShot(clip, volume * PrefsHelper.getVolume(PrefsHelper.VolumeType.SFX));
    }

    /// <summary>
    /// Plays sound effect and scales it with current speed. use this for most microgame sounds.
    /// </summary>
    /// <param name="clip"></param>
    /// <param name="panStereo"></param>
    /// <param name="pitchMult"></param>
    /// <param name="volume"></param>
    public void playSFX(AudioClip clip, float panStereo = 0f, float pitchMult = 1f, float volume = 1f)
    {
        playSFXUnscaled(clip, panStereo, pitchMult * Time.timeScale, volume);
    }

	void Update ()
	{
		if (debugMode)
		{
            if (!Input.GetKey(KeyCode.LeftControl) && !Input.GetKey(KeyCode.RightControl))
            {
                if (Input.GetKeyDown(debugKeys.Restart))
                {
                    holdDebugSession = microgame.CreateDebugSession(difficulty);
                    SceneManager.LoadScene(holdDebugSession.SceneName);
                    return;
                }
                else if (Input.GetKeyDown(debugKeys.Faster))
                {
                    holdDebugSession = microgame.CreateDebugSession(difficulty);
                    preserveDebugSpeed = Mathf.Min(debugSettings.speed + 1, StageController.MAX_SPEED);
                    Debug.Log("Debugging at speed " + preserveDebugSpeed);
                    SceneManager.LoadScene(holdDebugSession.SceneName);
                    return;
                }
                else if (Input.GetKeyDown(debugKeys.NextDifficulty))
                {
                    holdDebugSession = microgame.CreateDebugSession(Mathf.Min(session.Difficulty + 1, 3));
                    Debug.Log("Debugging at difficulty " + holdDebugSession.Difficulty);
                    SceneManager.LoadScene(holdDebugSession.SceneName);
                    return;
                }
                else if (Input.GetKeyDown(debugKeys.PreviousDifficulty))
                {
                    holdDebugSession = microgame.CreateDebugSession(Mathf.Max(session.Difficulty - 1, 1));
                    Debug.Log("Debugging at difficulty " + holdDebugSession.Difficulty);
                    SceneManager.LoadScene(holdDebugSession.SceneName);
                    return;
                }
            }
        }
	}
}
