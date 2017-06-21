using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using System.Collections;

public class MicrogameController : MonoBehaviour
{
	public static MicrogameController instance;
	private static int preserveDebugSpeed = -1;

	[SerializeField]
	private DebugSettings debugSettings;
	[System.Serializable]
	struct DebugSettings
	{
		public bool playMusic, displayCommand, showTimer, timerTick, simulateStartDelay, localizeText;
		public VoicePlayer.VoiceSet voiceSet;
		[Range(1, StageController.MAX_SPEED)]
		public int speed;
	}


	public UnityEvent onPause, onUnPause;
	public GameObject debugObjects, debugPauseManager;

	private MicrogameTraits traits;
	private bool victory, victoryDetermined;
	private Transform commandTransform;
	private VoicePlayer debugVoicePlayer;

	void Awake()
	{
		instance = this;

		string sceneName = gameObject.scene.name;
		if (sceneName.Equals("Template"))
			sceneName = "_Template1";
		traits = MicrogameTraits.findMicrogameTraits(sceneName.Substring(0, sceneName.Length - 1), int.Parse(sceneName.Substring(sceneName.Length - 1, 1)));
		Transform localization = transform.FindChild("Localization");

		if (StageController.instance == null)
		{
			//Debug Mode Start (scene open by itself)

			localization.gameObject.SetActive(debugSettings.localizeText);
			if (debugSettings.localizeText)
				localization.GetComponent<LocalizationManager>().Awake();

			traits.onAccessInStage(sceneName.Substring(0, sceneName.Length - 1));

			if (preserveDebugSpeed > -1)
			{
				Debug.Log("Debugging at speed " + preserveDebugSpeed);
				debugSettings.speed = preserveDebugSpeed;
				preserveDebugSpeed = -1;
			}

			StageController.beatLength = 60f / 130f;
			Time.timeScale = StageController.getSpeedMult(debugSettings.speed);

			debugObjects = Instantiate(debugObjects, Vector3.zero, Quaternion.identity) as GameObject;
			debugPauseManager = Instantiate(debugPauseManager, Vector3.zero, Quaternion.identity) as GameObject;

			MicrogameTimer.instance = debugObjects.transform.FindChild("UI Camera").FindChild("Timer").GetComponent<MicrogameTimer>();
			MicrogameTimer.instance.beatsLeft = (float)traits.getDurationInBeats() + (debugSettings.simulateStartDelay ? 1f : 0f);
			if (!debugSettings.showTimer)
				MicrogameTimer.instance.disableDisplay = true;
			if (debugSettings.timerTick)
				MicrogameTimer.instance.invokeTick();

			victory = traits.defaultVictory;
			victoryDetermined = false;

			if (debugSettings.playMusic && traits.musicClip != null)
			{
				AudioSource source = debugObjects.transform.FindChild("Music").GetComponent<AudioSource>();
				source.clip = traits.musicClip;
				source.pitch = StageController.getSpeedMult(debugSettings.speed);
				if (!debugSettings.simulateStartDelay)
					source.Play();
				else
					AudioHelper.playScheduled(source, StageController.beatLength);
			}

			Transform UICam = debugObjects.transform.FindChild("UI Camera");
			commandTransform = UICam.FindChild("Command");
			UICam.gameObject.SetActive(true);
			if (debugSettings.displayCommand)
			{
				commandTransform.gameObject.SetActive(true);
				commandTransform.FindChild("Text").GetComponent<TextMesh>().text = traits.localizedCommand;
			}

			Cursor.visible = traits.controlScheme == MicrogameTraits.ControlScheme.Mouse && !traits.hideCursor;

			debugVoicePlayer = debugObjects.transform.FindChild("Voice Player").GetComponent<VoicePlayer>();
			debugVoicePlayer.loadClips(debugSettings.voiceSet);
		}
		else if (!isBeingDiscarded())
		{
			//Normal Start

			if (localization.gameObject.activeInHierarchy)
				localization.gameObject.SetActive(false);

			StageController.instance.stageCamera.tag = "Camera";
			Camera.main.GetComponent<AudioListener>().enabled = false;

			StageController.instance.microgameMusicSource.clip = traits.musicClip;

			if (traits.hideCursor)
				Cursor.visible = false;

			commandTransform = StageController.instance.transform.root.FindChild("UI").FindChild("Command");

			StageController.instance.resetVictory();
			StageController.instance.onMicrogameAwake();
		}

	}

	void Start()
	{
		if (isBeingDiscarded())
		{
			GameObject[] rootObjects = gameObject.scene.GetRootGameObjects();
			for (int i = 0; i < rootObjects.Length; i++)
			{
				rootObjects[i].SetActive(false);
			}
		}
		else
			SceneManager.SetActiveScene(gameObject.scene);
	}

	bool isBeingDiscarded()
	{
		return StageController.instance.animationPart == StageController.AnimationPart.GameOver;
	}

	/// <summary>
	/// Returns MicrogameTraits for the microgame (from the prefab)
	/// </summary>
	/// <returns></returns>
	public MicrogameTraits getTraits()
	{
		return traits;
	}

	/// <summary>
	/// Returns true if microgame is in debug mode (scene open by itself)
	/// </summary>
	/// <returns></returns>
	public bool isDebugMode()
	{
		return StageController.instance == null;
	}

	public Transform getCommandTransform()
	{
		return commandTransform;
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
			//Debug victory
			if (victoryDetermined)
			{
				return;
			}
			this.victory = victory;
			victoryDetermined = final;
			if (final)
				debugVoicePlayer.playClip(victory, victory ? traits.victoryVoiceDelay : traits.failureVoiceDelay);
		}
		else
		{
			//StageController handles regular victory
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
	/// Re-displays the command text with the specified message. Only use this if the text will not need to be localized
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

	/// <summary>
	/// Re-displays the command text with a localized message. Key is automatically prefixed with "microgame.[ID]."
	/// </summary>
	/// <param name="command"></param>
	public void displayLocalizedCommand(string key, string defaultString)
	{
		displayCommand(TextHelper.getLocalizedMicrogameText(key, defaultString));
	}

	void Update ()
	{
		if (StageController.instance == null)
		{
			if (Input.GetKeyDown(KeyCode.R))
				SceneManager.LoadScene(gameObject.scene.buildIndex);
			else if (Input.GetKey(KeyCode.F))
			{
				preserveDebugSpeed = Mathf.Min(debugSettings.speed + 1, StageController.MAX_SPEED);
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
