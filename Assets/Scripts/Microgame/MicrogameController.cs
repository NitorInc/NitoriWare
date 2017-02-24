using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using System.Collections;

public class MicrogameController : MonoBehaviour
{
	public static MicrogameController instance;
	private static int preserveDebugSpeed = -1;

	public ControlScheme controlScheme;
	public int beatDuration;
	public string command;
	public bool defaultVictory, canEndEarly, hideCursor;
	public float victoryVoiceDelay, failureVoiceDelay;
	public AudioClip musicClip;

	public bool debugMusic, debugCommand, debugTimer, debugTimerTick, debugSimulateDelay;
	public VoicePlayer.VoiceSet debugVoiceSet;
	[Range(1, StageController.MAX_SPEED)]
	public int debugSpeed;
	public UnityEvent onPause, onUnPause;
	public GameObject debugObjects;

	private bool victory, victoryDetermined;
	private Transform commandTransform;
	private VoicePlayer debugVoicePlayer;

	public enum ControlScheme
	{
		Touhou,
		Mouse
	}

	void Awake()
	{
		instance = this;

		if (StageController.instance == null)
		{
			//Debug Mode Start (scene open by itself)

			if (preserveDebugSpeed > -1)
			{
				Debug.Log("Debugging at speed " + preserveDebugSpeed);
				debugSpeed = preserveDebugSpeed;
				preserveDebugSpeed = -1;
			}

			StageController.beatLength = 60f / 130f;
			Time.timeScale = StageController.getSpeedMult(debugSpeed);

			debugObjects = Instantiate(debugObjects, Vector3.zero, Quaternion.identity) as GameObject;

			MicrogameTimer.instance = debugObjects.transform.FindChild("UI Camera").FindChild("Timer").GetComponent<MicrogameTimer>();
			MicrogameTimer.instance.beatsLeft = (float)beatDuration + (debugSimulateDelay ? 1f : 0f);
			if (!debugTimer)
				MicrogameTimer.instance.disableDisplay = true;
			if (debugTimerTick)
				MicrogameTimer.instance.invokeTick();

			victory = defaultVictory;
			victoryDetermined = false;

			if (debugMusic && musicClip != null)
			{
				AudioSource source = debugObjects.transform.FindChild("Music").GetComponent<AudioSource>();
				source.clip = musicClip;
				source.pitch = StageController.getSpeedMult(debugSpeed);
				if (!debugSimulateDelay)
					source.Play();
				else
					AudioHelper.playScheduled(source, StageController.beatLength);
			}

			Transform UICam = debugObjects.transform.FindChild("UI Camera");
			commandTransform = UICam.FindChild("Command");
			UICam.gameObject.SetActive(true);
			if (debugCommand)
			{
				commandTransform.gameObject.SetActive(true);
				commandTransform.FindChild("Text").GetComponent<TextMesh>().text = command;
			}

			Cursor.visible = controlScheme == ControlScheme.Mouse && !hideCursor;

			debugVoicePlayer = debugObjects.transform.FindChild("Voice Player").GetComponent<VoicePlayer>();
			debugVoicePlayer.loadClips(debugVoiceSet);
		}
		else
		{
			//Normal Start
			StageController.instance.stageCamera.tag = "Camera";
			Camera.main.GetComponent<AudioListener>().enabled = false;

			MicrogameTimer.instance.beatsLeft = StageController.instance.getBeatsRemaining();
			MicrogameTimer.instance.gameObject.SetActive(true);

			StageController.instance.microgameMusicSource.clip = musicClip;

			if (hideCursor)
				Cursor.visible = false;

			commandTransform = StageController.instance.transform.root.FindChild("UI").FindChild("Command");

			StageController.instance.resetVictory();
			StageController.instance.invokeNextCycle();
		}

	}

	void Start()
	{
		SceneManager.SetActiveScene(gameObject.scene);
	}

	/// <summary>
	/// Call this to have the player win/lose a microgame, set final to true if the victory status will NOT be changed again
	/// </summary>
	/// <param name="victory"></param>
	/// <param name="final"></param>
	public void setVictory(bool victory, bool final)
	{
		if (StageController.instance == null)
		{
			if (victoryDetermined)
			{
				return;
			}
			this.victory = victory;
			victoryDetermined = final;
			if (final)
				debugVoicePlayer.playClip(victory, victory ? victoryVoiceDelay : failureVoiceDelay);
		}
		else
		{
			StageController.instance.setMicrogameVictory(victory, final);
		}
	}
	
	/// <summary>
	/// Returns whether the game would be won if it ends now
	/// </summary>
	/// <returns></returns>
	public bool getVictory()
	{
		if (StageController.instance == null)
		{
			return victory;
		}
		else
			return StageController.instance.getMicrogameVictory();
	}

	/// <summary>
	/// Returns true if the game's victory outcome will not be changed for the rest of its duration
	/// </summary>
	/// <returns></returns>
	public bool getVictoryDetermined()
	{
		if (StageController.instance == null)
		{
			return victoryDetermined;
		}
		else
			return StageController.instance.getVictoryDetermined();
	}

	/// <summary>
	/// Redisplays the command text with the specified message
	/// </summary>
	/// <param name="command"></param>
	public void displayCommand(string command)
	{
		if (!commandTransform.gameObject.activeInHierarchy)
			commandTransform.gameObject.SetActive(true);

		Animator _animator = commandTransform.GetComponent<Animator>();
		_animator.Rebind();
		_animator.Play("Command");
		commandTransform.FindChild("Text").GetComponent<TextMesh>().text = command;
	}

	void Update ()
	{
		if (StageController.instance == null)
		{
			if (Input.GetKeyDown(KeyCode.R))
				SceneManager.LoadScene(gameObject.scene.buildIndex);
			else if (Input.GetKey(KeyCode.F))
			{
				preserveDebugSpeed = Mathf.Min(debugSpeed + 1, StageController.MAX_SPEED);
				SceneManager.LoadScene(gameObject.scene.buildIndex);
			}
			else if (Input.GetKey(KeyCode.N))
			{
				string sceneName = SceneManager.GetActiveScene().name;
				char[] sceneChars = sceneName.ToCharArray();
				if (sceneChars[sceneChars.Length - 1] != '3')
				{
					int stageNumber = int.Parse(sceneChars[sceneChars.Length - 1].ToString());
					sceneName = sceneName.Substring(0, sceneName.Length - 1);
					SceneManager.LoadScene(sceneName + (stageNumber + 1).ToString());
				}
				else
					SceneManager.LoadScene(gameObject.scene.buildIndex);
			}
		}
	}
}
