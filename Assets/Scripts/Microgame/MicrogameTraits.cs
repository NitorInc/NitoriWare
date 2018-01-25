using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class MicrogameTraits : MonoBehaviour
{
#pragma warning disable 0649
  [SerializeField]
  private ControlScheme _controlScheme;
  public virtual ControlScheme controlScheme => _controlScheme;

  [SerializeField]
  private bool _hideCursor;
  public virtual bool hideCursor => _hideCursor;

  [SerializeField]
  private Duration _duration;
  public virtual Duration duration => _duration;

  [SerializeField]
  private bool _canEndEarly;
  public virtual bool canEndEarly => _canEndEarly;

  [SerializeField]
  private string _command;
  public virtual string command => _command;
  public virtual string localizedCommand => TextHelper.getLocalizedText($"microgame.{microgameId}.command", command);

  [SerializeField]
  private bool _defaultVictory;
  public virtual bool defaultVictory => _defaultVictory;

  [SerializeField]
  private float _victoryVoiceDelay, _failureVoiceDelay;
  public virtual float victoryVoiceDelay => _victoryVoiceDelay;
  public virtual float failureVoiceDelay
  {
    get { return _failureVoiceDelay; }
    set { _failureVoiceDelay = value; }
  }

  [SerializeField]
  private AudioClip _musicClip;
  public virtual AudioClip musicClip => _musicClip;

  [SerializeField]
  private bool _isStageReady;
  public virtual bool isStageReady => _isStageReady;

  [SerializeField]
  private string[] _credits;
  public virtual string[] credits => _credits;
#pragma warning restore 0649

  private string _microgameId;
  public string microgameId => _microgameId;

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

  public virtual void onAccessInStage(string microgameId) => _microgameId = microgameId;

  public virtual float getDurationInBeats() => duration == Duration.Long16Beats ? 16f : 8f;

  public static MicrogameTraits findMicrogameTraits(string microgameId, int difficulty, bool skipFinishedFolder = false)
  {
#if UNITY_EDITOR
    GameObject traits;


    if (!skipFinishedFolder)
    {
      traits = AssetDatabase.LoadAssetAtPath<GameObject>("Assets" + MicrogameCollection.MicrogameAssetPath + "_Finished/" + microgameId + "/Traits" + difficulty.ToString() + ".prefab");
      print("Assets" + MicrogameCollection.MicrogameAssetPath + "/" + microgameId + "/Traits" + difficulty.ToString() + ".prefab");
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