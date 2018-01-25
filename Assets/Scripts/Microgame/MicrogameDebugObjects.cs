using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MicrogameDebugObjects : MonoBehaviour
{
  public static MicrogameDebugObjects instance;

  [SerializeField]
  private CommandDisplay _commandDisplay;
  public CommandDisplay commandDisplay { get { return _commandDisplay; } }

  [SerializeField]
  private PauseManager _pauseManager;
  public PauseManager pauseManager { get { return _pauseManager; } }

  [SerializeField]
  private MicrogameTimer _microgameTimer;
  public MicrogameTimer microgameTimer { get { return _microgameTimer; } }

  [SerializeField]
  private AudioSource _musicSource;
  public AudioSource musicSource { get { return _musicSource; } }

  [SerializeField]
  private VoicePlayer _voicePlayer;
  public VoicePlayer voicePlayer { get { return _voicePlayer; } }

  void Awake()
  {
    if (instance == null)
    {
      instance = this;
      DontDestroyOnLoad(gameObject);
      DontDestroyOnLoad(pauseManager.gameObject);
    }
    else
      Destroy(gameObject);
  }

  void Start() => SceneManager.UnloadSceneAsync("Microgame Debug");

  public void Reset()
  {
    musicSource.Stop();
    voicePlayer.CancelInvoke();
    voicePlayer.GetComponent<AudioSource>().Stop();
    microgameTimer.CancelInvoke();
    microgameTimer.GetComponent<AudioSource>().Stop();
  }
}
