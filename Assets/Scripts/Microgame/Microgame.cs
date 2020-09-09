using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Events;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(menuName = "Microgame Traits/Traits")]
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

    public string GetSceneName(MicrogameSession session) => session.microgame.microgameId + session.Difficulty.ToString();
    

    [SerializeField]
    private float _victoryVoiceDelay;
    public float VictoryVoiceDelayDefault;

    [SerializeField]
    private float _failureVoiceDelay;
    public float FailureVoiceDelayDefault;

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

    public float getDurationInBeats() => (duration == Duration.Long16Beats) ? 16f : 8f;

    public bool isBossMicrogame() => GetType() == typeof(MicrogameBossTraits) || GetType().IsSubclassOf(typeof(MicrogameBossTraits));

    public virtual MicrogameSession CreateSession(StageController player, int difficulty)
        => new MicrogameSession(this, player, difficulty, false);

    public virtual MicrogameSession CreateDebugSession(int difficulty)
        => new MicrogameSession(this, null, difficulty, true);

    // For debug mode purposes
    public virtual bool SceneDeterminesDifficulty => true;
    public virtual int GetDifficultyFromScene(string sceneName) => int.Parse(sceneName.Last().ToString());

}