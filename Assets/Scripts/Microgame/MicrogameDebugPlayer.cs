using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using System.Linq;

public class MicrogameDebugPlayer : MonoBehaviour
{
    public static MicrogameDebugPlayer instance;
    public static bool DebugModeActive => instance != null;

    [SerializeField]
    private DebugSettings settings;
    public DebugSettings Settings => settings;
    [SerializeField]
    private MicrogamePlayer player;
    [SerializeField]
    private VoicePlayer voicePlayer;
    [SerializeField]
    private CommandDisplay commandDisplay;
    [SerializeField]
    private MicrogameEventListener eventListener;
    [SerializeField]
    private SpeedController speedController;
    [SerializeField]
    private MicrogameTimer microgameTimer;

    public MicrogameEventListener EventListener => eventListener;

    [System.Serializable]
    public class DebugSettings
    {
        public bool playMusic, displayCommand, showTimer, timerTick, localizeText;
        public string forceLocalizationLanguage;
        public bool resetThroughAllLanguages;
        public VoicePlayer.VoiceSet voiceSet;
        [Range(1, SpeedController.MaxSpeed)]
        public int speed = 1;
        [Header("For microgames where difficulty isn't dependent on scene:")]
        public DebugDifficulty SimulateDifficulty;
        public DebugKeys debugKeys;
    }

    [System.Serializable]
    public class DebugKeys
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

    public Microgame.Session MicrogameSession { get; private set; }

    private float timerVolume;

    void Awake()
    {
        instance = this;
        timerVolume = microgameTimer.GetComponent<AudioSource>().volume;
    }

    public void Initialize(Microgame.Session microgameSession, DebugSettings settings)
    {
        MicrogameSession = microgameSession;
        microgameSession.EventListener = EventListener;
        this.settings = settings;
        player.AddLoadedMicrogame(microgameSession, gameObject.scene);
        UpdateSpeed();

        DontDestroyOnLoad(gameObject);
    }

    //public void VictoryDetermined(Microgame.Session session)
    //{
    //    voicePlayer.playClip(session);
    //}

    public void SceneStarted()
    {
        settings.speed = speedController.Speed;

        if (Settings.localizeText)
        {
            LocalizationManager manager = GameController.instance.transform.Find("Localization Manager").GetComponent<LocalizationManager>();

            if (Settings.resetThroughAllLanguages)
            {
                var isFirstLoad = LocalizationManager.instance == null;
                var languages = LanguagesData.instance.languages;
                var currentLanguageSetting = LanguagesData.instance.FindLanguage(settings.forceLocalizationLanguage, defaultToEnglish: false);
                var languageIndex = currentLanguageSetting != null
                    ? languages.ToList().IndexOf(currentLanguageSetting)
                    : -1;
                if (languageIndex < 0 || !isFirstLoad)
                {
                    languageIndex++;
                    if (languageIndex >= languages.Count())
                        languageIndex = 0;
                }
                var newLanguageID = languages[languageIndex].getLanguageID();
                settings.forceLocalizationLanguage = newLanguageID;
                print("Language cycling debugging in " + newLanguageID);
            }

            if (!string.IsNullOrEmpty(Settings.forceLocalizationLanguage))
            {
                if (LocalizationManager.instance == null)
                    manager.setForcedLanguage(settings.forceLocalizationLanguage);
                else
                    manager.setLanguage(settings.forceLocalizationLanguage);
            }

            manager.gameObject.SetActive(true);
        }

        microgameTimer.disableDisplay = !Settings.showTimer;
        microgameTimer.GetComponent<AudioSource>().volume = settings.timerTick ? timerVolume : 0f;

        if (Settings.displayCommand)
            commandDisplay.PlayCommand(MicrogameSession, MicrogameSession.GetLocalizedCommand(), MicrogameSession.GetCommandSettings());

        Cursor.visible = MicrogameSession.microgame.controlScheme == Microgame.ControlScheme.Mouse && !MicrogameSession.GetHideCursor();
        Cursor.lockState = MicrogameSession.GetCursorLockMode();

        voicePlayer.StopPlayback();
        voicePlayer.loadClips(Settings.voiceSet);
    }

    void UpdateSpeed()
    {
        speedController.ApplySpeed();
    }

    public void LoadNewMicrogame(Microgame.Session session)
    {
        UpdateSpeed();
        if (session != MicrogameSession)
            MicrogameSession.Dispose();
        MicrogameSession = session;
        microgameTimer.StopAllCoroutines();
        player.LoadMicrogameImmediately(session);
    }

    void Update()
    {
        if (!Input.GetKey(KeyCode.LeftControl) && !Input.GetKey(KeyCode.RightControl))
        {
            if (Input.GetKeyDown(Settings.debugKeys.Restart))
            {
                var newSession = MicrogameSession.microgame.CreateDebugSession(MicrogameSession.Difficulty);
                speedController.Speed = 1;
                LoadNewMicrogame(newSession);
                return;
            }
            else if (Input.GetKeyDown(Settings.debugKeys.Faster))
            {
                var newSession = MicrogameSession.microgame.CreateDebugSession(MicrogameSession.Difficulty);
                speedController.Speed = Mathf.Min(speedController.Speed + 1, SpeedController.MaxSpeed);
                Debug.Log("Debugging at speed " + speedController.Speed);
                LoadNewMicrogame(newSession);
                return;
            }
            else if (Input.GetKeyDown(Settings.debugKeys.NextDifficulty))
            {
                var newSession = MicrogameSession.microgame.CreateDebugSession(Mathf.Min(MicrogameSession.Difficulty + 1, 3));
                speedController.Speed = 1;
                Debug.Log("Debugging at difficulty " + newSession.Difficulty);
                LoadNewMicrogame(newSession);
                return;
            }
            else if (Input.GetKeyDown(Settings.debugKeys.PreviousDifficulty))
            {
                var newSession = MicrogameSession.microgame.CreateDebugSession(Mathf.Max(MicrogameSession.Difficulty - 1, 1));
                speedController.Speed = 1;
                Debug.Log("Debugging at difficulty " + newSession.Difficulty);
                LoadNewMicrogame(newSession);
                return;
            }
        }
    }
}
