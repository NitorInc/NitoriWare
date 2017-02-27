using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class StageController : MonoBehaviour
{
	public static StageController instance;

	public const int MAX_SPEED = 10;

	public bool shuffleOn, speedIncreaseOn, difficultyIncreaseOn;
	[Range(1, MAX_SPEED)]
	public int speed;
	public bool muteMusic, speedUpAnimation;
	public Microgame[] microgamePool;
	public VoicePlayer.VoiceSet voiceSet;

	public int maxStockpiledScenes;

	private int microgameCount, microgameIndex, loadedMicrogameCount, round, life;
	private bool microgameVictory, victoryDetermined;

	public AnimationPart animationPart = AnimationPart.Intro;

	public VoicePlayer voicePlayer;
	public MicrogameInfoParser infoParser;
	public Camera stageCamera;
	public Animator[] lifeIndicators;
	public AudioSource outroSource, introSource, speedUpSource, microgameMusicSource;
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
		Idle,		//0	- Animation does nothing, camera is disabled | Until microgame ends
		Intro,		//1	- Part that introduces the microgame, animation begins in this state | 8 beats
		Outro,		//2	- Part that changes depending on if you win/lose | 4 beats
		LastBeat,	//3	- Starts at the last "beat" before the microgame ends, allowing the scene objects to pop back onscreen before unloading the microgame | 1 Beat

		SpeedUp		//4 - Interruption between Outro and Intro when the game increases speed, speed is actually changed in Intro | 8 Beats
	}

	//Interruptions happen between Outro and Intro, signaling animations such as speeding up and introducing a boss microgame
	public Interruption interruption;
	public enum Interruption
	{
		Nothing,
		SpeedUp
	}

	void Start()
	{

		setAnimationPart(animationPart);
		beatLength = outroSource.clip.length / 4f;

		animationStartTime = Time.time;
		setMicrogameVictory(true, false);

		microgameCount = 0;
		round = 0;
		startNextRound();
		Application.backgroundLoadingPriority = ThreadPriority.Low;

		loadedMicrogameCount = 0;
		loadNextMicrogame();

		resetLifeIndicators();

		Time.timeScale = getSpeedMult();

		voicePlayer.loadClips(voiceSet);

		introSource.pitch = getSpeedMult();
		introSource.Play();
		invokeIntroAnimations();
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
			//Shuffle microgame order
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
	//the zeroth beat of each cycle is when the Intro animation starts
	void invokeOutroAnimations()
	{
		invokeAtBeat("updateToLastBeat", -5f);

		invokeAtBeat("updateToOutro", -4f);

		invokeAtBeat("updateMicrogameLoading", -2f);
	}

	void invokeIntroAnimations()
	{
		invokeAtBeat("updateToIntro", 0f);

		invokeAtBeat("updateCursorVisibility", 1f);

		invokeAtBeat("startMicrogame", 7f);

		invokeAtBeat("playMicrogameMusic", 8f);

		invokeAtBeat("updateToIdle", 9f);
	}


	void updateToLastBeat()
	{
		//scene.SetActive(true);
		setAnimationPart(AnimationPart.LastBeat);
		//if (MicrogameController.instance != null)
		MicrogameController.instance.displayCommand("");
	}

	void updateToOutro()
	{
		outroSource.pitch = getSpeedMult();
		if (!muteMusic)
			outroSource.Play();
		setAnimationPart(AnimationPart.Outro);
		if (!microgameVictory)
			lowerLife();

		endMicrogame();
		microgameCount++;

		//Reload microgames if we've cycled through them
		if (getMicrogameIndex() >= microgamePool.Length)
		{
			startNextRound();
			loadedMicrogameCount = 0;
			loadNextMicrogame();
		}

		//Determine interruptions and apply speed up
		interruption = Interruption.Nothing;
		if (speedIncreaseOn)
		{
			int index = getMicrogameIndex();
			if (index == 4 || index == 8)
			{
				speed++;
				if (speedUpAnimation)
					interruption = Interruption.SpeedUp;
			}
		}

		//Apply speed increase between rounds
		if (speedIncreaseOn)
		{
			if (getMicrogameIndex() == 0)
			{
				if (round > 3)
					speed = round - 2;
				else
					speed = 1;
			}
			if (speed > MAX_SPEED)
				speed = MAX_SPEED;
		}


		if (interruption != Interruption.Nothing)
			invokeAtBeat("updateTo" + interruption.ToString(), 0f);

		animationStartTime += getInterruptionBeats() * beatLength;
		invokeIntroAnimations();

		introSource.pitch = getSpeedMult();
		if (!muteMusic && life > 0)
		{
				AudioHelper.playScheduled(introSource, beatLength * (4f + getInterruptionBeats()));
		}


	}

	void updateToIntro()
	{

		//Placeholder "game over"
		if (life <= 0)
		{
			Time.timeScale = 1f;
			CancelInvoke();
			introSource.Stop();
			placeholderResults.transform.parent.gameObject.SetActive(true);
			placeholderResults.setScore(microgameCount);
			return;
		}

		setAnimationPart(AnimationPart.Intro);

		Time.timeScale = getSpeedMult();
		introSource.pitch = getSpeedMult();

		getMicrogameInfo();

		if (!introSource.isPlaying)
			introSource.Play();
	}

	void updateToIdle()
	{
		setAnimationPart(AnimationPart.Idle);
		//scene.SetActive(false);
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

		MicrogameInfoParser.MicrogameInfoGroup info = infoParser.getMicrogameInfo(microgamePool[i].name);
		command.text = info.commands[getMicrogameDifficulty(i) - 1];
		controlDisplay.sprite = controlSchemeSprites[(int)info.controlSchemes[getMicrogameDifficulty(i) - 1]];
	}

	public float getBeatsRemaining()
	{
		return (animationStartTime + (beatLength * (-4f)) - Time.time) / beatLength;
		//return (animationStartTime + (beatLength * (8f + (float)MicrogameController.instance.beatDuration) + getInterruptionBeats()) - Time.time) / beatLength;
	}

	void startMicrogame()
	{

		Microgame microgame = microgamePool[getMicrogameIndex()];
		microgame.asyncOperation.allowSceneActivation = true;
	}

	public void resetVictory()
	{
		victoryDetermined = false;
		setMicrogameVictory(MicrogameController.instance.getTraits().defaultVictory, false);
	}

	//Called from MicrogameController on Awake()
	public void onMicrogameAwake()
	{
		animationStartTime += beatLength * (12f + (float)MicrogameController.instance.getTraits().getDurationInBeats());

		MicrogameTimer.instance.beatsLeft = StageController.instance.getBeatsRemaining();
		MicrogameTimer.instance.gameObject.SetActive(true);
		MicrogameTimer.instance.invokeTick();
		invokeOutroAnimations();
		
		microgameIndex++;
	}

	void endMicrogame()
	{
		if (!getVictoryDetermined())
			voicePlayer.playClip(microgameVictory, 0f);
				//getMicrogameVictory() ? MicrogameController.instance.getTraits().victoryVoiceDelay : MicrogameController.instance.getTraits().failureVoiceDelay);
		else
			voicePlayer.forcePlay();

		MicrogameController.instance.gameObject.SetActive(false);
		SceneManager.UnloadScene(MicrogameController.instance.gameObject.scene);
		SceneManager.SetActiveScene(gameObject.scene);

		stageCamera.tag = "MainCamera";
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

	public void onPause()
	{
		if (animationPart == AnimationPart.Intro)
			introSource.Pause();
		else if (introSource.isPlaying)
			introSource.Stop();
	}

	public void onUnPause()
	{
		if (animationPart == AnimationPart.Intro)
			introSource.UnPause();
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
		MicrogameInfoParser.MicrogameInfoGroup info = infoParser.getMicrogameInfo(microgamePool[getMicrogameIndex()].name);
		Cursor.visible = info.controlSchemes[getMicrogameDifficulty(getMicrogameIndex()) - 1] == MicrogameTraits.ControlScheme.Mouse;
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

		if (victory && outroSource.clip != victoryClip)
		{
			outroSource.clip = victoryClip;
		}
		else if (!victory && outroSource.clip != failureClip)
		{
			outroSource.clip = failureClip;
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
		voicePlayer.playClip(microgameVictory,
			getMicrogameVictory() ? MicrogameController.instance.getTraits().victoryVoiceDelay : MicrogameController.instance.getTraits().failureVoiceDelay);

		if (MicrogameController.instance.getTraits().canEndEarly)
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
		invokeOutroAnimations();

		//Redo invokes with new time
		//if (interruption == Interruption.SpeedUp && speedUpAnimation)
		//{
		//	animationStartTime -= beatLength * 8f;
		//	invokeOutroAnimations();
		//	invokeAtBeat("updateToSpeedUp", 0f);
		//	animationStartTime += beatLength * 8f;
		//}
		//else
		
	}

	void invokeAtBeat(string function, float beatFromAnimationStart)
	{
		invokeAtTime(function, animationStartTime + (beatLength * beatFromAnimationStart));
	}

	void invokeAtTime(string function, float time)
	{
		Invoke(function, time - Time.time);
	}


	public float getSpeedMult()
	{
		return getSpeedMult(speed);
	}

	public static float getSpeedMult(int speed)
	{
		return 1f + ((float)(speed - 1) * .125f);
	}

	private void setAnimationInteger(string name, int state)
	{
		foreach (Animator animator in transform.root.GetComponentsInChildren<Animator>())
		{
			animator.SetInteger(name, state);
		}
	}

	private void setAnimationBool(string name, bool state)
	{
		foreach (Animator animator in transform.root.GetComponentsInChildren<Animator>())
		{
			animator.SetBool(name, state);
		}
	}

	private float getInterruptionBeats()
	{
		return getInterruptionBeats(interruption);
	}

	private float getInterruptionBeats(Interruption interruption)
	{
		switch (interruption)
		{
			case (Interruption.SpeedUp):
				return 8f;
			default:
				return 0f;
		}
	}

}
