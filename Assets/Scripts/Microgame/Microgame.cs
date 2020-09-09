using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Events;
using System.Linq;

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
    private bool _hideCursor;
    public bool hideCursorDefault => _hideCursor;

    [SerializeField]
    private CursorLockMode _cursorLockState = CursorLockMode.None;
    public CursorLockMode cursorLockStateDefault => _cursorLockState;

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
    private string _command;
    public string commandDefault => _command;

    [SerializeField]
    private AnimatorOverrideController _commandAnimatorOveride;
    public AnimatorOverrideController CommandAnimatorOverrideDefault => _commandAnimatorOveride;

    [SerializeField]
    private bool _defaultVictory;
    public bool defaultVictory => _defaultVictory;
    
    [SerializeField]
    private float _victoryVoiceDelay;
    public float VictoryVoiceDelayDefault => _victoryVoiceDelay;

    [SerializeField]
    private float _failureVoiceDelay;
    public float FailureVoiceDelayDefault => _failureVoiceDelay;

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

}