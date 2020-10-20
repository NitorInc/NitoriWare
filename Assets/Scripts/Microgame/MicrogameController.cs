using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using System.Collections;
using System.Linq;
using TMPro;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MicrogameController : MonoBehaviour
{
	public static MicrogameController instance;

    [SerializeField]
	private MicrogameDebugPlayer.DebugSettings debugSettings;
    public MicrogameDebugPlayer.DebugSettings DebugSettings => debugSettings;

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

    [SerializeField]
    private GameObject debugPlayerPrefab;
    
    private bool debugMode;
    private CommandDisplay commandDisplay;

    public Microgame.Session session { get; private set; }
    public Microgame microgame => session.microgame;
    public string microgameId => microgame.microgameId;
    public int difficulty => session.Difficulty;

    void Awake()
	{
		instance = this;

        debugMode = GameController.instance == null || MicrogameDebugPlayer.DebugModeActive;

        if (debugMode && MicrogameDebugPlayer.instance == null)
        {
            var sceneName = gameObject.scene.name;
            if (sceneName.Contains("Template"))
            {
                Debug.Break();
                Debug.Log("You can't play the template scene.");
            }

            // Get collection microgame if available
            var microgame = MicrogameHelper.getMicrogames(includeBosses: true)
                .FirstOrDefault(a => sceneName.Contains(a.microgameId));

            if (microgame == null)
            {
                Debug.LogError("Could not find microgame metadata. Make sure the scene name contains the microgame's ID and the folder is named correctly, and that your Microgame's metadata is where it should be.");
                Debug.Break();
            }

            Instantiate(debugPlayerPrefab, Vector3.zero, Quaternion.identity);

            int difficulty;
            if (microgame.SceneDeterminesDifficulty())
                difficulty = microgame.GetDifficultyFromScene(gameObject.scene.name);
            else
                difficulty = Mathf.Max((int)debugSettings.SimulateDifficulty, 1);
            session = microgame.CreateDebugSession(difficulty);

            MicrogameDebugPlayer.instance.Initialize(session, debugSettings);

            if (!sceneName.Equals(session.GetSceneName()))
            {
                MicrogameDebugPlayer.instance.LoadNewMicrogame(session);
                return;
            }
        }
        else
        {
            session = MicrogameSessionManager.ActiveSessions
                .FirstOrDefault(a => a.GetSceneName().Equals(gameObject.scene.name) && a.AsyncState == Microgame.Session.SessionState.Activating);

            if (session == null)    // If null the session was likely cancelled
                return;
        }

        if (debugMode)
        {
            session = MicrogameDebugPlayer.instance.MicrogameSession;
            debugSettings = MicrogameDebugPlayer.instance.Settings;
        }

        session.AsyncState = Microgame.Session.SessionState.Playing;
        session.EventListener.SceneAwake.Invoke(session, gameObject.scene);

	}

	void Start()
    {
        if (session == null
            && MicrogameSessionManager.ActiveSessions
                .Any(a => a.GetSceneName().Equals(gameObject.scene.name) && a.AsyncState == Microgame.Session.SessionState.Loading)
            && !MicrogameDebugPlayer.DebugModeActive
            && Application.isPlaying)
        {

            Debug.LogError("Microgame scene(s) activated prematurely. This is a Unity bug. Try restarting the scene.");
            Debug.Break();
            return;
        }

        SceneManager.SetActiveScene(gameObject.scene);
        Cursor.visible = microgame.controlScheme == Microgame.ControlScheme.Mouse && !session.GetHideCursor();
        Cursor.lockState = microgame.controlScheme == Microgame.ControlScheme.Mouse ? session.GetCursorLockMode() : GameController.DefaultCursorMode;
        session.StartTime = Time.time;
        session.EventListener.MicrogameStart.Invoke(session);
    }

    public void onPaused()
    {
        onPause.Invoke();
    }

    public void onUnPaused()
    {
        onUnPause.Invoke();
    }

    bool isBeingDiscarded()
	{
        if (debugMode)
            return false;
        return session.AsyncState == Microgame.Session.SessionState.Unloading;
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
        if (session.WasVictoryDetermined)
            return;

        bool finalize = final && !session.WasVictoryDetermined;

        session.VictoryStatus = victory;
        session.WasVictoryDetermined = final;

        session.EventListener.VictoryStatusUpdated.Invoke(session);
        if (final)
            session.EventListener.VictoryStatusFinalized.Invoke(session);

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
	public void displayCommand(string command, MicrogameCommandSettings commandSettingsOverride = null)
    {
        if (commandSettingsOverride == null)
            commandSettingsOverride = session.GetCommandSettings();
        session.EventListener.DisplayCommand.Invoke(session, command, commandSettingsOverride);
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
	public void displayLocalizedCommand(string key, string defaultString, MicrogameCommandSettings commandSettingsOverride = null)
	{
		displayCommand(TextHelper.getLocalizedMicrogameText(key, defaultString), commandSettingsOverride);
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
        sfxSource.PlayOneShot(clip, volume);
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
        playSFXUnscaled(clip, panStereo, pitchMult, volume);
    }
}
