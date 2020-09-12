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
    private AudioSource musicSource;
    [SerializeField]
    private VoicePlayer voicePlayer;
    [SerializeField]
    private CommandDisplay commandDisplay;
    [SerializeField]
    private MicrogameEventListener eventListener;
    public MicrogameEventListener EventListener => eventListener;

    [System.Serializable]
    public class DebugSettings
    {
        public bool playMusic, displayCommand, showTimer, timerTick, simulateStartDelay, localizeText;
        public string forceLocalizationLanguage;
        public bool resetThroughAllLanguages;
        public VoicePlayer.VoiceSet voiceSet;
        [Range(1, StageController.MAX_SPEED)]
        public int speed;
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

    private int playSpeed = 1;

    void Awake()
    {
        instance = this;
        StageController.beatLength = 60f / 130f;
    }

    public void Initialize(Microgame.Session microgameSession, DebugSettings settings)
    {
        MicrogameSession = microgameSession;
        this.settings = settings;
        player.AddLoadedMicrogame(microgameSession, gameObject.scene);
        UpdateSpeed();

        DontDestroyOnLoad(gameObject);
    }

    public void VictoryDetermined(Microgame.Session session)
    {
        voicePlayer.playClip(session.VictoryStatus,
            session.VictoryStatus
                ? session.VictoryVoiceDelay
                : session.FailureVoiceDelay);
    }

    public void SceneStarted()
    {
        settings.speed = playSpeed;

        if (Settings.localizeText)
        {
            LocalizationManager manager = GameController.instance.transform.Find("Localization").GetComponent<LocalizationManager>();
            if (!string.IsNullOrEmpty(Settings.forceLocalizationLanguage))
                manager.setForcedLanguage(Settings.forceLocalizationLanguage);
            else if (Settings.resetThroughAllLanguages)
            {
                var languages = LanguagesData.instance.languages;
                var languageIndex = languages.ToList().IndexOf(LocalizationManager.instance.getLoadedLanguage());
                languageIndex++;
                if (languageIndex >= languages.Count())
                    languageIndex = 0;
                var newLanguageID = languages[languageIndex].getLanguageID();
                manager.setLanguage(newLanguageID);
                print("Language cycling debugging in " + newLanguageID);
            }
        }

        MicrogameTimer.instance.CancelInvoke();
        MicrogameTimer.instance.beatsLeft = (float)MicrogameSession.microgame.getDurationInBeats()
            + (Settings.simulateStartDelay ? 1f : 0f);
        if (!Settings.showTimer)
            MicrogameTimer.instance.disableDisplay = true;
        if (Settings.timerTick)
            MicrogameTimer.instance.invokeTick();

        var musicClip = MicrogameSession.MusicClip;
        if (Settings.playMusic && musicClip != null)
        {
            musicSource.clip = musicClip;
            musicSource.pitch = StageController.getSpeedMult(playSpeed);
            if (!Settings.simulateStartDelay)
                musicSource.Play();
            else
                AudioHelper.playScheduled(musicSource, StageController.beatLength);
        }

        if (Settings.displayCommand)
            commandDisplay.play(MicrogameSession.GetLocalizedCommand(), MicrogameSession.CommandAnimatorOverride);

        Cursor.visible = MicrogameSession.microgame.controlScheme == Microgame.ControlScheme.Mouse && !MicrogameSession.HideCursor;
        Cursor.lockState = MicrogameSession.cursorLockMode;

        voicePlayer.loadClips(Settings.voiceSet);
    }

    void UpdateSpeed()
    {
        Time.timeScale = StageController.getSpeedMult(playSpeed);
    }

    public void LoadNewMicrogame(Microgame.Session session)
    {
        UpdateSpeed();
        if (session != MicrogameSession)
            MicrogameSession.Dispose();
        MicrogameSession = session;
        player.LoadMicrogameImmediately(session);
    }

    void Update()
    {
        if (!Input.GetKey(KeyCode.LeftControl) && !Input.GetKey(KeyCode.RightControl))
        {
            if (Input.GetKeyDown(Settings.debugKeys.Restart))
            {
                var newSession = MicrogameSession.microgame.CreateSession(eventListener, MicrogameSession.Difficulty);
                playSpeed = 1;
                LoadNewMicrogame(newSession);
                return;
            }
            else if (Input.GetKeyDown(Settings.debugKeys.Faster))
            {
                var newSession = MicrogameSession.microgame.CreateSession(eventListener, MicrogameSession.Difficulty);
                playSpeed = Mathf.Min(playSpeed + 1, StageController.MAX_SPEED);
                Debug.Log("Debugging at speed " + playSpeed);
                LoadNewMicrogame(newSession);
                return;
            }
            else if (Input.GetKeyDown(Settings.debugKeys.NextDifficulty))
            {
                var newSession = MicrogameSession.microgame.CreateSession(eventListener,
                    Mathf.Min(MicrogameSession.Difficulty + 1, 3));
                Debug.Log("Debugging at difficulty " + newSession.Difficulty);
                LoadNewMicrogame(newSession);
                return;
            }
            else if (Input.GetKeyDown(Settings.debugKeys.PreviousDifficulty))
            {
                var newSession = MicrogameSession.microgame.CreateSession(eventListener,
                    Mathf.Max(MicrogameSession.Difficulty - 1, 1));
                playSpeed = 1;
                Debug.Log("Debugging at difficulty " + newSession.Difficulty);
                LoadNewMicrogame(newSession);
                return;
            }
        }
    }
}
