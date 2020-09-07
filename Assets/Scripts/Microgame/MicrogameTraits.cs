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
	public virtual bool GetHideCursor(MicrogameSession session) => _hideCursor;

    [SerializeField]
    private CursorLockMode _cursorLockState = CursorLockMode.None;
    public virtual CursorLockMode GetCursorLockState(MicrogameSession session) => _cursorLockState;

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
    public virtual string GetCommandKey(MicrogameSession session) => "command";
    public virtual string GetLocalizedCommand(MicrogameSession session) =>
        TextHelper.getLocalizedText($"microgame.{session.MicrogameId}.{GetCommandKey(session)}", command);

    [SerializeField]
    private AnimatorOverrideController _commandAnimatorOveride;
    public virtual AnimatorOverrideController GetCommandAnimatorOverride(MicrogameSession session) => _commandAnimatorOveride;

    [SerializeField]
	private bool _defaultVictory;
	public virtual bool defaultVictory => _defaultVictory;
    
    public virtual string GetSceneName(MicrogameSession session) => session.MicrogameId + session.Difficulty.ToString();

    // For debug mode purposes
    public virtual bool SceneDeterminesDifficulty => true;
    public virtual int GetDifficultyFromScene(string sceneName) => int.Parse(sceneName.Last().ToString());

    [SerializeField]
    private float _victoryVoiceDelay;
    public virtual float GetVictoryVoiceDelay(MicrogameSession session) => _victoryVoiceDelay;

    [SerializeField]
    private float _failureVoiceDelay;
    public virtual float GetFailureVoiceDelay(MicrogameSession session) => _failureVoiceDelay;

    [SerializeField]
	private AudioClip _musicClip;
    public virtual AudioClip GetMusicClip(MicrogameSession session) => _musicClip;
    public virtual AudioClip[] GetAllMusicClips() => new AudioClip []{ _musicClip};

    [SerializeField]
	private Milestone _milestone = Milestone.Unfinished;
	public virtual Milestone milestone => _milestone;
    public enum Milestone
    {
        Unfinished,
        StageReady,
        Finished
    }

    [Header("Credits order is Art, Code, Music:")]
    [SerializeField]
    private string[] _credits = { "", "", "" };
    public virtual string[] credits => _credits;

	public virtual MicrogameSession onAccessInStage(string microgameId, int difficulty)
	{
        return new MicrogameSession(microgameId, difficulty);
	}

    public virtual void onDebugModeAccess(MicrogameController microgameController, MicrogameSession session)
    {

    }

	public virtual float getDurationInBeats()
	{
		return duration == Duration.Long16Beats ? 16f : 8f;
	}

    public bool isBossMicrogame()
    {
        return GetType() == typeof(MicrogameBossTraits);
    }

    public static MicrogameTraits findMicrogameTraits(string microgameId)
    {
#if UNITY_EDITOR
        MicrogameTraits traits;

        //Search normal games
        traits = findMicrogameTraitsInFolder($"Assets{MicrogameCollection.MicrogameAssetPath}{microgameId}");
        if (traits != null)
            return traits;

        //Search bosses
        traits = findMicrogameTraitsInFolder($"Assets{MicrogameCollection.MicrogameAssetPath}_Bosses/{microgameId}");
        if (traits != null)
            return traits;

        Debug.LogError("Can't find Traits prefab for " + microgameId);
        return null;
    }
    static MicrogameTraits findMicrogameTraitsInFolder(string microgameFolder)
    {
        string fileName = "Traits";
        string extension = ".asset";
        return AssetDatabase.LoadAssetAtPath<MicrogameTraits>(Path.Combine(microgameFolder, fileName + extension));
        }
#else
        Debug.LogError("Microgame updates should NOT be called outside of the editor. You shouldn't even see this message.");
        return null;
    }
#endif
}