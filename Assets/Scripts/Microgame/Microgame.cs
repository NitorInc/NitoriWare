using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(menuName = "Microgame/Normal")]
public class Microgame : ScriptableObject
{

    public string microgameId => name;

    [SerializeField]
    private ControlScheme _controlScheme;
    public ControlScheme controlScheme => _controlScheme;
    public enum ControlScheme
    {
        Key,
        Mouse
    }

    [SerializeField]
    protected bool _hideCursor;

    [SerializeField]
    protected CursorLockMode _cursorLockState = CursorLockMode.None;

    [SerializeField]
    private Duration _duration;
    public Duration duration => _duration;
    public enum Duration
    {
        Short8Beats,
        Long16Beats
    }
    public bool canEndEarly => _duration == Duration.Long16Beats;

    [SerializeField]
    protected string _command;

    [SerializeField]
    protected AnimatorOverrideController _commandAnimatorOveride;

    [SerializeField]
    protected bool _defaultVictory;
    
    [SerializeField]
    protected float _victoryVoiceDelay;

    [SerializeField]
    protected float _failureVoiceDelay;

    [SerializeField]
    private AudioClip _musicClip;
    public AudioClip MusicClipDefault => _musicClip;
    public virtual AudioClip[] GetAllPossibleMusicClips() => new AudioClip[] { _musicClip };

    [SerializeField]
    private Milestone _milestone = Milestone.Unfinished;
    public Milestone milestone => _milestone;
    public enum Milestone
    {
        Unfinished,
        StageReady,
        Finished
    }

    [Header("Credits order is Art, Code, Music:")]
    [SerializeField]
    private string[] _credits = { "", "", "" };
    public string[] credits => _credits;

    public virtual float getDurationInBeats() => (duration == Duration.Long16Beats) ? 16f : 8f;

    public bool isBossMicrogame() => GetType() == typeof(MicrogameBoss) || GetType().IsSubclassOf(typeof(MicrogameBoss));

    public virtual MicrogameSession CreateSession(StageController player, int difficulty, bool debugMode = false)
        => new MicrogameSession(this, player, difficulty, debugMode);

    public MicrogameSession CreateDebugSession(int difficulty)
        => CreateSession(null, difficulty, true);

    // For debug mode purposes
    public virtual bool SceneDeterminesDifficulty => true;
    public virtual int GetDifficultyFromScene(string sceneName) => int.Parse(sceneName.Last().ToString());


    public class MicrogameSession : IDisposable
    {
        static List<MicrogameSession> activeSessions;
        //public static ReadOnlyCollection<MicrogameSession> ActiveSessions =>
        //    (activeSessions != null  ? activeSessions : new List<MicrogameSession>()).AsReadOnly();

        public Microgame microgame { get; private set; }

        public StageController microgamePlayer { get; private set; }

        public int Difficulty { get; private set; }

        public bool Victory { get; set; }

        public bool VictoryWasDetermined { get; set; } = false;

        public float VictoryVoiceDelay { get; set; }

        public float FailureVoiceDelay { get; set; }

        public SessionState State { get; set; }
        public enum SessionState
        {
            Loading,
            Playing,
            Unloading
        }


        public virtual bool HideCursor => microgame._hideCursor;

        public virtual CursorLockMode cursorLockMode => microgame._cursorLockState;

        public virtual string GetNonLocalizedCommand() => microgame._command;

        public virtual AnimatorOverrideController CommandAnimatorOverride => microgame._commandAnimatorOveride;

        public virtual string SceneName => microgame.microgameId + Difficulty.ToString();

        public virtual string GetLocalizedCommand() =>
            TextHelper.getLocalizedText($"microgame.{microgame.microgameId}.command", GetNonLocalizedCommand());

        public virtual AudioClip MusicClip => microgame.MusicClipDefault;

        /// <summary>
        /// If you inherit this class to randomize certain start attributes, randomize them in the constructor
        /// </summary>
        public MicrogameSession(Microgame microgame, StageController player, int difficulty, bool debugMode)
        {
            this.microgame = microgame;
            Difficulty = difficulty;
            Victory = microgame._defaultVictory;
            VictoryVoiceDelay = microgame._victoryVoiceDelay;
            FailureVoiceDelay = microgame._failureVoiceDelay;

            MicrogameSessionManager.AddSession(this);
        }

        public void Dispose()
        {
            MicrogameSessionManager.RemoveSession(this);
        }
    }


}