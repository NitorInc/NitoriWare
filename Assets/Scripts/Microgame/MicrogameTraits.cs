using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(menuName = "Microgame Traits")]
public class MicrogameTraits : ScriptableObject
{
    [SerializeField]
	private ControlScheme _controlScheme;
	public virtual ControlScheme controlScheme => _controlScheme;
    public enum ControlScheme
    {
        Key,
        Mouse
    }

    [SerializeField]
	private bool _hideCursor;
	public virtual bool hideCursor => _hideCursor;

    [SerializeField]
    private CursorLockMode _cursorLockState = CursorLockMode.None;
    public virtual CursorLockMode cursorLockState => _cursorLockState;

    [SerializeField]
	private Duration _duration;
	public virtual Duration duration => _duration;
    public enum Duration
    {
        Short8Beats,
        Long16Beats
    }
	public virtual bool canEndEarly => _duration == Duration.Long16Beats;

    [SerializeField]
	private string _command;
	public virtual string command => _command;
    public virtual string commandKey => "command";
    public virtual string localizedCommand => TextHelper.getLocalizedText($"microgame.{microgameId}.{commandKey}", command);

    [SerializeField]
    private AnimatorOverrideController _commandAnimatorOveride;
    public virtual AnimatorOverrideController commandAnimatorOverride => _commandAnimatorOveride;

    [SerializeField]
	private bool _defaultVictory;
	public virtual bool defaultVictory => _defaultVictory;

    [SerializeField]
    private float _victoryVoiceDelay;
    private float instanceVictoryVoiceDelay;
	public virtual float victoryVoiceDelay {get {return instanceVictoryVoiceDelay; } 
        set { instanceVictoryVoiceDelay = value; }}

    [SerializeField]
    private float _failureVoiceDelay;
    private float instanceFailureVoiceDelay;
	public virtual float failureVoiceDelay {get {return instanceFailureVoiceDelay; } 
        set { instanceFailureVoiceDelay = value; }}

    [SerializeField]
	private AudioClip _musicClip;
    public virtual AudioClip musicClip => _musicClip;

    [SerializeField]
	private Milestone _milestone = Milestone.Unfinished;
	public virtual Milestone milestone => _milestone;
    public enum Milestone
    {
        Unfinished,
        StageReady,
        Finished
    }

    [SerializeField]
    private string[] _credits = { "", "", "" };
    public virtual string[] credits => _credits;

    private string _microgameId;
    public string microgameId => _microgameId;
    private int _difficulty;
    public int difficulty => _difficulty;

	public virtual void onAccessInStage(string microgameId, int difficulty)
	{
        instanceVictoryVoiceDelay = _victoryVoiceDelay;
        instanceFailureVoiceDelay = _failureVoiceDelay;
		_microgameId = microgameId;
        _difficulty = difficulty;
	}

	public virtual float getDurationInBeats()
	{
		return duration == Duration.Long16Beats ? 16f : 8f;
	}

    public bool isBossMicrogame()
    {
        return GetType() == typeof(MicrogameBossTraits);
    }

    public static MicrogameTraits findMicrogameTraits(string microgameId, int difficulty)
    {
#if UNITY_EDITOR
        MicrogameTraits traits;

        //Search normal games
        traits = findMicrogameTraitsInFolder($"Assets{MicrogameCollection.MicrogameAssetPath}{microgameId}", difficulty);
        if (traits != null)
            return traits;

        //Search bosses
        traits = findMicrogameTraitsInFolder($"Assets{MicrogameCollection.MicrogameAssetPath}_Bosses/{microgameId}", difficulty);
        if (traits != null)
            return traits;

        Debug.LogError("Can't find Traits prefab for " + microgameId + difficulty.ToString());
        return null;
    }
    static MicrogameTraits findMicrogameTraitsInFolder(string microgameFolder, int difficulty)
    {
        string fileName = "Traits";
        string extension = ".asset";

        //Look in Traits.asset
        MicrogameTraits traits = AssetDatabase.LoadAssetAtPath<MicrogameTraits>($"{microgameFolder}/{fileName}{extension}");
        if (traits != null)
            return traits;
        //Look in Traits[diff].asset
        traits = AssetDatabase.LoadAssetAtPath<MicrogameTraits>($"{microgameFolder}/{fileName}{difficulty.ToString()}{extension}");
        if (traits != null)
            return traits;

        return null;
        }
#else
        Debug.LogError("Microgame updates should NOT be called outside of the editor. You shouldn't even see this message.");
        return null;
    }
#endif
}