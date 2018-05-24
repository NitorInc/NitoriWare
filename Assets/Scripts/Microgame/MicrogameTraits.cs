using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(menuName = "Microgame Traits")]
public class MicrogameTraits : ScriptableObject
{
#pragma warning disable 0649
    [SerializeField]
	private ControlScheme _controlScheme;
	public virtual ControlScheme controlScheme { get { return _controlScheme; } set { } }

	[SerializeField]
	private bool _hideCursor;
	public virtual bool hideCursor { get { return _hideCursor; } set { } }

    [SerializeField]
    private CursorLockMode _cursorLockState = CursorLockMode.None;
    public virtual CursorLockMode cursorLockState { get { return _cursorLockState; } set { _cursorLockState = value; } }

    [SerializeField]
	private Duration _duration;
	public virtual Duration duration { get { return _duration; } set { } }

	[SerializeField]
	private bool _canEndEarly;
	public virtual bool canEndEarly { get { return _canEndEarly; } set { } }

	[SerializeField]
	private string _command;
	public virtual string command { get { return _command; } set { } }
	public virtual string localizedCommand { get { return TextHelper.getLocalizedText("microgame." + microgameId + ".command", command); } set { } }

	[SerializeField]
	private bool _defaultVictory;
	public virtual bool defaultVictory { get { return _defaultVictory; } set { } }

	[SerializeField]
	private float _victoryVoiceDelay, _failureVoiceDelay;
	public virtual float victoryVoiceDelay { get { return _victoryVoiceDelay; } set { } }
	public virtual float failureVoiceDelay { get { return _failureVoiceDelay; } set { } }

	[SerializeField]
	private AudioClip _musicClip;
	public virtual AudioClip musicClip{ get { return _musicClip; } set { } }

	[SerializeField]
	private bool _isStageReady;
	public virtual bool isStageReady { get { return _isStageReady; } set { } }

    [SerializeField]
    private string[] _credits;
    public virtual string[] credits { get { return _credits; } set { } }
#pragma warning restore 0649

    private string _microgameId;
	public string microgameId { get { return _microgameId; } set { } }

	public enum ControlScheme
	{
		Key,
		Mouse
	}

	public enum Duration
	{
		Short8Beats,
		Long16Beats
	}

	public virtual void onAccessInStage(string microgameId)
	{
		_microgameId = microgameId;
	}

	public virtual float getDurationInBeats()
	{
		return duration == Duration.Long16Beats ? 16f : 8f;
	}

	public static MicrogameTraits findMicrogameTraits(string microgameId, int difficulty, bool skipFinishedFolder = false)
    {
#if UNITY_EDITOR
        GameObject traits;


        if (!skipFinishedFolder)
		{
			traits = AssetDatabase.LoadAssetAtPath<GameObject>("Assets" + MicrogameCollection.MicrogameAssetPath + "_Finished/" + microgameId + "/Traits" + difficulty.ToString() + ".prefab");
			if (traits != null)
				return traits.GetComponent<MicrogameTraits>();
		}

		traits = AssetDatabase.LoadAssetAtPath<GameObject>("Assets" + MicrogameCollection.MicrogameAssetPath + microgameId + "/Traits" + difficulty.ToString() + ".prefab");
		if (traits != null)
			return traits.GetComponent<MicrogameTraits>();

		traits = AssetDatabase.LoadAssetAtPath<GameObject>("Assets" + MicrogameCollection.MicrogameAssetPath + "_Bosses/" + microgameId + "/Traits" + difficulty.ToString() + ".prefab");
		if (traits != null)
			return traits.GetComponent<MicrogameTraits>();

		Debug.LogError("Can't find Traits prefab for " + microgameId + difficulty.ToString());
		return null;
#else
        Debug.LogError("Microgame updates should NOT be called outside of the editor. You shouldn't even see this message.");
        return null;
#endif
    }
}