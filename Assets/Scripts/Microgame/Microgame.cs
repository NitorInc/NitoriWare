using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif


/// <summary>
/// Global data object for a microgame. Can be derived to inject custom logic
/// </summary>
[CreateAssetMenu(menuName = "Microgame/Normal")]
public class Microgame : ScriptableObject
{
    public const double BeatLength = 60d / 130d;

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
    protected float _commandRestY = -4f;

    [SerializeField]
    protected bool _defaultVictory;
    
    [SerializeField]
    protected float _victoryVoiceDelay;

    [SerializeField]
    protected float _failureVoiceDelay;

    [SerializeField]
    private Milestone _milestone = Milestone.Unfinished;
    public Milestone milestone => _milestone;
    public enum Milestone
    {
        Unfinished,
        StageReady,
        Finished
    }

    [SerializeField]
    private CharacterStage characterStage;
    public CharacterStage CharacterStage => characterStage;

    [Header("Credits order is Art, Code, Music:")]
    [SerializeField]
    private string[] _credits = { "", "", "" };
    public string[] credits => _credits;

    public virtual double getDurationInBeats() => (duration == Duration.Long16Beats) ? 17d : 9d;
    public virtual double getDurationInSeconds() => getDurationInBeats() * (float)BeatLength;

    public bool isBossMicrogame() => GetType() == typeof(MicrogameBoss) || GetType().IsSubclassOf(typeof(MicrogameBoss));

    // For debug mode purposes
    public virtual bool SceneDeterminesDifficulty() => true;
    public virtual int GetDifficultyFromScene(string sceneName) => int.Parse(sceneName.Last().ToString());

    /// <summary>
    /// Creates a new "instance" of this microgame to prepare for a play session
    /// </summary>
    /// <param name="eventListener"></param>
    /// <param name="difficulty"></param>
    /// <param name="debugMode"></param>
    /// <returns>The session</returns>
    public virtual Session CreateSession(int difficulty, bool debugMode = false)
        => new Session(this, difficulty, debugMode);

    public Session CreateDebugSession(int difficulty)
        => CreateSession(difficulty, true);


    /// <summary>
    /// This class essentially acts as an "instance" of the microgame, with data about what's occuring.
    /// It also contains functions that may change per-session that can be overridden in a subclass.
    ///  Note: You'll need to override CreateSession in microgame subclass as well to access your subclass
    /// </summary>
    public class Session : IDisposable
    {
        public virtual string GetSceneName() => microgame.microgameId + Difficulty.ToString();

        public virtual string GetNonLocalizedCommand() => microgame._command;

        public virtual string GetLocalizedCommand() =>
            TextHelper.getLocalizedText($"microgame.{microgame.microgameId}.command", GetNonLocalizedCommand());

        public virtual AnimatorOverrideController GetCommandAnimatorOverride() => microgame._commandAnimatorOveride;

        public virtual MicrogameCommandSettings GetCommandSettings() =>
            new MicrogameCommandSettings() { AnimatorOverride = microgame._commandAnimatorOveride, restYPosition = microgame._commandRestY};

        public virtual float GetCommandRestY() => microgame._commandRestY;

        public virtual bool GetHideCursor() => microgame._hideCursor;

        public virtual CursorLockMode GetCursorLockMode() => microgame._cursorLockState;

        public Microgame microgame { get; private set; }
        public MicrogameEventListener EventListener { get; set; }

        public int Difficulty { get; private set; }
        public bool VictoryStatus { get; set; }
        public bool WasVictoryDetermined { get; set; } = false;
        public float VictoryVoiceDelay { get; set; }
        public float FailureVoiceDelay { get; set; }
        public float GetVoiceDelay() => VictoryStatus ? VictoryVoiceDelay : FailureVoiceDelay;
        public float StartTime { get; set; }
        public double EndEarlyBeats { get; set; } // If >0, microgame is scheduled to end early by this many beats
        public double EndEarlyTime => EndEarlyBeats * BeatLength;
        public bool IsEndingEarly => EndEarlyBeats > 0d;
        public float EndTime => StartTime + (float)microgame.getDurationInSeconds();
        public float TimeRemaining => EndTime - Time.time;
        public float BeatsRemaining => TimeRemaining / (float)BeatLength;

        public SessionState AsyncState { get; set; }
        public enum SessionState
        {
            Loading,    // Microgame scene is loading but not set to activate
            Activating, // Microgame scene is set to activate but has not awaken yet
            Playing,    // Microgame scene is the active game scene and is performing gameplay
            Unloading   // Microgame scene is unloading async
        }

        public bool Cancelled { get; set; } // Microgame is set to destroy itself as soon as it loads in and won't be played

        /// <summary>
        /// If you inherit this class to randomize certain start attributes, randomize them in the constructor
        /// </summary>
        public Session(Microgame microgame, int difficulty, bool debugMode)
        {
            this.microgame = microgame;
            Difficulty = difficulty;
            VictoryStatus = microgame._defaultVictory;
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