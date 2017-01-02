using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class ScenarioController : MonoBehaviour
{
	public static ScenarioController instance;

	public const int MAX_SPEED = 10;

	public bool shuffleOn, speedIncreaseOn, difficultyIncreaseOn;
	[Range(1, MAX_SPEED)]
	public int speed;
	public bool muteMusic;
	public Microgame[] microgamePool;

	public int maxStockpiledScenes;

	private int microgameCount, microgameIndex, loadedMicrogameCount, round, life;
	private bool microgameVictory, victoryDetermined;

	public AnimationPart animationPart;

	public MicrogameInfoParser infoParser;
	public Camera scenarioCamera;
	public Animator[] lifeIndicators;
	public AudioSource introSource, outroSource, speedUpSource, microgameMusicSource;
	public AudioClip victoryClip, failureClip;
	public TextMesh command;
	public GameObject scene;
	public SpriteRenderer controlDisplay;
	public Sprite[] controlSchemeSprites;

	public NitoriScorePlaceholder placeholderResults;

	public static float beatLength;

	private float animationStartTime;

	[System.Serializable]
	public struct Microgame
	{
		public string name;
		public int baseDifficulty;

		[HideInInspector]
		public AsyncOperation asyncOperation;

	}

	public enum AnimationPart
	{
		Idle,		//0	Animation does nothing, camera is disabled
		Intro,		//1	Part that changes depending on if you win/lose
		Outro,		//2	Part after Outro, shows the control scheme and current score
		LastBeat,	//4	Starts at the last "beat" before the microgame ends, allowing the scene objects to pop back onscreen before unloading the microgame
		SpeedUp		//5 Between Intro and Outro when the game increases speed, speed is actually changed in Outro
	}

	public Interruption interruption;
	public enum Interruption
	{
		Nothing,
		SpeedUp
	}

	void Start()
	{	

		setAnimationPart(animationPart);

		beatLength = introSource.clip.length / 4f;
		animationStartTime = Time.time + 1.25f;
		setMicrogameVictory(true, false);
		Invoke("invokeIntroAnimations", .2f);
		Invoke("invokeOutroAnimations", .2f);

		microgameCount = 0;
		round = 0;
		startNextRound();
		Application.backgroundLoadingPriority = ThreadPriority.Low;

		loadedMicrogameCount = 0;
		loadNextMicrogame();

		resetLifeIndicators();

		Time.timeScale = getSpeedMult();
	}

	void Awake()
	{
		instance = this;
	}

	public void loadNextMicrogame()
	{
		if (loadedMicrogameCount >= microgamePool.Length)
			return;

		StartCoroutine(loadMicrogameAsync(loadedMicrogameCount));
		loadedMicrogameCount++;
	}

	IEnumerator loadMicrogameAsync(int index)
	{
		microgamePool[index].asyncOperation = SceneManager.LoadSceneAsync(microgamePool[index].name + getMicrogameDifficulty(index).ToString()
		, LoadSceneMode.Additive);
		microgamePool[index].asyncOperation.allowSceneActivation = false;
		microgamePool[index].asyncOperation.priority = 999 - index;

		while (microgamePool[index].asyncOperation.progress < .9f)
		{
			yield return null;
		}
	}

	void startNextRound()
	{
		microgameIndex = 0;
		round++;

		if (shuffleOn)
		{
			int index = 0, choice;
			Microgame hold;
			while (index < microgamePool.Length)
			{
				choice = Random.Range(index, microgamePool.Length);
				if (choice != index)
				{
					hold = microgamePool[index];
					microgamePool[index] = microgamePool[choice];
					microgamePool[choice] = hold;
				}
				index++;
			}
		}
	}


	//Animation and music time is measured by "beats" here
	//the zeroth beat of each cycle is when the Intro animation starts (right when the last microgame ends)
	void invokeIntroAnimations()
	{
		invokeAtBeat("updateToLastBeat", -1f);

		invokeAtBeat("updateToIntro", 0f);

		invokeAtBeat("updateMicrogameLoading", 2f);
	}

	void invokeOutroAnimations()
	{
		invokeAtBeat("updateToOutro", 4f);

		invokeAtBeat("updateCursorVisibility", 5f);

		invokeAtBeat("startMicrogame", 11f);

		invokeAtBeat("playMicrogameMusic", 12f);

		invokeAtBeat("updateToIdle", 13f);
	}


	void updateToLastBeat()
	{
		scene.SetActive(true);
		setAnimationPart(AnimationPart.LastBeat);
	}

	void updateToIntro()
	{
		introSource.pitch = getSpeedMult();
		if (!muteMusic)
			introSource.Play();
		setAnimationPart(AnimationPart.Intro);
		if (!microgameVictory)
			lowerLife();

		if (microgameCount > 0)
		{
			endMicrogame();
		}

		microgameCount++;

		//Reload microgames if we've cycled through them
		if (microgameIndex >= microgamePool.Length)
		{
			startNextRound();
			loadedMicrogameCount = 0;
			loadNextMicrogame();
		}

		//Apply speed increase
		if (speedIncreaseOn)
		{
			if (interruption == Interruption.SpeedUp)
				speed++;
			else if (getMicrogameIndex() == 0)
			{

				if (round > 3)
					speed = round - 2;
				else
					speed = 1;
			}
			if (speed > MAX_SPEED)
				speed = MAX_SPEED;
		}
		
		

		outroSource.pitch = getSpeedMult();
		if (!muteMusic && life > 0)
		{
			if (interruption == Interruption.Nothing)
				AudioHelper.playScheduled(outroSource, beatLength * 4f);
			else
				AudioHelper.playScheduled(outroSource, beatLength * 12f);
		}

	}

	void updateToOutro()
	{

		//Placeholder "game over"
		if (life <= 0)
		{
			Time.timeScale = 1f;
			CancelInvoke();
			outroSource.Stop();
			placeholderResults.transform.parent.gameObject.SetActive(true);
			placeholderResults.setScore(microgameCount);
			return;
		}

		Time.timeScale = getSpeedMult();
		setAnimationPart(AnimationPart.Outro);

		getMicrogameInfo();


	}

	void updateToIdle()
	{
		setAnimationPart(AnimationPart.Idle);
		scene.SetActive(false);
	}

	void updateToSpeedUp()
	{
		setAnimationPart(AnimationPart.SpeedUp);
		speedUpSource.pitch = getSpeedMult(speed - 1);
		speedUpSource.Play();
	}

	void playMicrogameMusic()
	{
		microgameMusicSource.pitch = getSpeedMult();
		if (!muteMusic && microgameMusicSource.clip != null)
			microgameMusicSource.Play();
	}


	int getMicrogameIndex()
	{
		return microgameIndex;
	}

	void getMicrogameInfo()
	{
		int i = getMicrogameIndex();

		MicrogameInfoParser.MicrogameInfo info = infoParser.getMicrogameInfo(microgamePool[i].name);
		command.text = info.commands[getMicrogameDifficulty(i) - 1];
		controlDisplay.sprite = controlSchemeSprites[(int)info.controlSchemes[getMicrogameDifficulty(i) - 1]];
	}

	public float getBeatsRemaining()
	{
		return (animationStartTime + (beatLength * (12f + (float)MicrogameController.instance.beatDuration)) - Time.time) / beatLength;
	}

	void startMicrogame()
	{

		Microgame microgame = microgamePool[getMicrogameIndex()];
		microgame.asyncOperation.allowSceneActivation = true;
	}

	public void resetVictory()
	{
		victoryDetermined = false;
		setMicrogameVictory(MicrogameController.instance.defaultVictory, false);
	}

	//Called from MicrogameController on Awake()
	public void invokeNextCycle ()
	{
		animationStartTime += beatLength * (12f + (float)MicrogameController.instance.beatDuration);
		MicrogameTimer.instance.invokeTick();
		microgameIndex++;

		invokeIntroAnimations();

		//Increase speed periodically
		if (speedIncreaseOn)
		{
			int index = getMicrogameIndex();
			if (index == 4 || index == 8)
			{
				interruption = Interruption.SpeedUp;
				invokeAtBeat("updateToSpeedUp", 4f);
				animationStartTime += beatLength * 8f;
			}
			else
				interruption = Interruption.Nothing;
		}
		else
			interruption = Interruption.Nothing;

		invokeOutroAnimations();

	}

	void endMicrogame()
	{

		MicrogameController.instance.gameObject.SetActive(false);
		SceneManager.UnloadScene(MicrogameController.instance.gameObject.scene);
		SceneManager.SetActiveScene(gameObject.scene);

		scenarioCamera.tag = "MainCamera";
		MicrogameController.instance = null;
		CameraController.instance = Camera.main.GetComponent<CameraController>();
		
		microgameMusicSource.Stop();
		Cursor.visible = false;

		MicrogameTimer.instance.beatsLeft = 0f;
		MicrogameTimer.instance.gameObject.SetActive(false);

		//sceneLoader.removeMicrogame(microgamePool[microgameIndex]);
	}

	int getMicrogameDifficulty(int index)
	{
		Microgame microgame = microgamePool[index];

		if (!difficultyIncreaseOn)
			return microgame.baseDifficulty;

		int difficulty = round + microgame.baseDifficulty - 1;

		if (difficulty > 3)
			difficulty = 3;

		return difficulty;

	}

	
	void Update()
	{

		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Application.Quit();
		}
		else if (Input.GetKeyDown(KeyCode.R))
		{
			SceneManager.LoadScene(gameObject.scene.buildIndex);
		}
	}

	void updateMicrogameLoading()
	{
		if (getMicrogameIndex() > loadedMicrogameCount - maxStockpiledScenes)
		{
			loadNextMicrogame();
		}
	}

	public void setAnimationPart(AnimationPart animationPart)
	{
		this.animationPart = animationPart;
		setAnimationInteger("part", (int)animationPart);
	}

	void updateCursorVisibility()
	{
		MicrogameInfoParser.MicrogameInfo info = infoParser.getMicrogameInfo(microgamePool[getMicrogameIndex()].name);
		Cursor.visible = info.controlSchemes[getMicrogameDifficulty(getMicrogameIndex()) - 1] == MicrogameController.ControlScheme.Mouse;
	}

	/// <summary>
	/// Do NOT call this from a microgame, use MicrogameController.setVictory() instead
	/// </summary>
	/// <param name="victory"></param>
	/// <param name="final"></param>
	public void setMicrogameVictory(bool victory, bool final)
	{
		if (victoryDetermined)
		{
			return;
		}

		if (victory && introSource.clip != victoryClip)
		{
			introSource.clip = victoryClip;
		}
		else if (!victory && introSource.clip != failureClip)
		{
			introSource.clip = failureClip;
		}
		microgameVictory = victory;

		if (final)
			setFinalAnswer();
		victoryDetermined = final;

		setAnimationBool("microgameVictory", microgameVictory);
	}

	void setFinalAnswer()
	{
		//Can't have this happening before the microgame actually starts
		if (animationPart != AnimationPart.Idle && animationPart != AnimationPart.LastBeat)
		{
			Invoke("setFinalAnswer", beatLength);
			return;
		}

		victoryDetermined = true;

		if (MicrogameController.instance.canEndEarly)
		{
			float beatOffset = MicrogameTimer.instance.beatsLeft - 2f;
			beatOffset -= beatOffset % 4f;
			if (beatOffset > 0f)
			{
				if (beatOffset > 8f)
					beatOffset = 8f;

				endMicrogameEarly(beatOffset);
			}
		}
	}

	public bool getMicrogameVictory()
	{
		return microgameVictory;
	}

	public bool getVictoryDetermined()
	{
		return victoryDetermined;
	}

	void resetLifeIndicators()
	{
		life = 4;
		for (int i = 0; i < lifeIndicators.Length; i++)
		{
			lifeIndicators[i].SetInteger("life", lifeIndicators.Length - i);
		}

	}

	void lowerLife()
	{
		life--;
		for (int i = 0; i < lifeIndicators.Length; i++)
		{
			lifeIndicators[i].SetInteger("life", lifeIndicators[i].GetInteger("life") - 1);
		}
	}


	public void endMicrogameEarly(float beatsEarly)
	{

		CancelInvoke();
		MicrogameTimer.instance.CancelInvoke();

		animationStartTime -= beatLength * beatsEarly;

		//Redo invokes with new time
		if (interruption == Interruption.Nothing)
			invokeIntroAnimations();
		else if (interruption == Interruption.SpeedUp)
		{
			animationStartTime -= beatLength * 8f;
			invokeIntroAnimations();
			invokeAtBeat("updateToSpeedUp", 4f);
			animationStartTime += beatLength * 8f;
		}

		invokeOutroAnimations();
	}

	void invokeAtBeat(string function, float beatFromAnimationStart)
	{
		invokeAtTime(function, animationStartTime + (beatLength * beatFromAnimationStart));
	}

	void invokeAtTime(string function, float time)
	{
		Invoke(function, time - Time.time);
	}


	private void setAnimationInteger(string name, int state)
	{
		foreach (Animator animator in GetComponentsInChildren<Animator>())
		{
			animator.SetInteger(name, state);
		}
	}

	public float getSpeedMult()
	{
		return getSpeedMult(speed);
	}

	public static float getSpeedMult(int speed)
	{
		return 1f + ((float)(speed - 1) * .125f);
	}

	private void setAnimationBool(string name, bool state)
	{
		foreach (Animator animator in GetComponentsInChildren<Animator>())
		{
			animator.SetBool(name, state);
		}
	}

}
