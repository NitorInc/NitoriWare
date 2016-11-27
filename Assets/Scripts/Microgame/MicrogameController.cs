using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
//using UnityEngine.SceneManagement;

public class MicrogameController : MonoBehaviour
{
	public static MicrogameController instance;


	public ControlScheme controlScheme;
	public int beatDuration;
	public string command;
	public bool defaultVictory, canEndEarly;
	public AudioClip musicClip;

	public bool debugMusic, debugCommand, debugTimer, debugTimerTick, debugSimulateDelay;
	[Range(1, ScenarioController.MAX_SPEED)]
	public int debugSpeed;
	public GameObject debugObjects;

	private bool victory, victoryDetermined;
	private Transform commandTransform;

	public enum ControlScheme
	{
		Touhou,
		Mouse
	}

	void Awake()
	{
		instance = this;

		if (ScenarioController.instance == null)
		{
			//Debug Mode Start (scene open by itself)
			ScenarioController.beatLength = 60f / 130f;
			Time.timeScale = ScenarioController.getSpeedMult(debugSpeed);

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
				source.pitch = ScenarioController.getSpeedMult(debugSpeed);
				if (!debugSimulateDelay)
					source.Play();
				else
					AudioHelper.playScheduled(source, ScenarioController.beatLength);
			}

			Transform UICam = debugObjects.transform.FindChild("UI Camera");
			commandTransform = UICam.FindChild("Command");
			UICam.gameObject.SetActive(true);
			if (debugCommand)
			{
				commandTransform.gameObject.SetActive(true);
				commandTransform.FindChild("Text").GetComponent<TextMesh>().text = command;
			}

			if (controlScheme == ControlScheme.Mouse)
				Cursor.visible = true;
		}
		else
		{
			//Normal Start
			ScenarioController.instance.scenarioCamera.tag = "Camera";
			Camera.main.GetComponent<AudioListener>().enabled = false;
			SceneManager.SetActiveScene(gameObject.scene);

			MicrogameTimer.instance.beatsLeft = ScenarioController.instance.getBeatsRemaining();
			MicrogameTimer.instance.gameObject.SetActive(true);

			ScenarioController.instance.microgameMusicSource.clip = musicClip;

			commandTransform = ScenarioController.instance.transform.FindChild("Command");

			ScenarioController.instance.resetVictory();
			ScenarioController.instance.invokeNextCycle();
		}

	}


	/// <summary>
	/// Call this to have the player win/lose a microgame, set final to true if the victory status will NOT be changed again
	/// </summary>
	/// <param name="victory"></param>
	/// <param name="final"></param>
	public void setVictory(bool victory, bool final)
	{
		if (ScenarioController.instance == null)
		{
			if (victoryDetermined)
			{
				return;
			}
			this.victory = victory;
			victoryDetermined = final;
		}
		else
		{
			ScenarioController.instance.setMicrogameVictory(victory, final);
		}
	}
	
	/// <summary>
	/// Returns whether the game would be won if it ends now
	/// </summary>
	/// <returns></returns>
	public bool getVictory()
	{
		if (ScenarioController.instance == null)
		{
			return victory;
		}
		else
			return ScenarioController.instance.getMicrogameVictory();
	}

	/// <summary>
	/// Returns true if the game's victory outcome will not be changed for the rest of its duration
	/// </summary>
	/// <returns></returns>
	public bool getVictoryDetermined()
	{
		if (ScenarioController.instance == null)
		{
			return victoryDetermined;
		}
		else
			return ScenarioController.instance.getVictoryDetermined();
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
		if (ScenarioController.instance == null && Input.GetKeyDown(KeyCode.R))
		{
			SceneManager.LoadScene(gameObject.scene.buildIndex);
		}
	}
}
