using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class StageController : MonoBehaviour
{
	public static StageController instance;

	public const int MAX_SPEED = 10;

	[SerializeField]
	private Stage stage;
	[Range(1, MAX_SPEED)]
	public int speed;
	public bool godMode, muteMusic;

	public int maxStockpiledScenes;

	private int microgameCount, life;
	private bool microgameVictoryStatus, victoryDetermined;

	public AnimationPart animationPart = AnimationPart.Intro;

	public VoicePlayer voicePlayer;
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

	private MicrogameTraits microgameTraits;
	private float animationStartTime, outroPlayTime;

	private Queue<MicrogameInstance> microgameQueue;
	private class MicrogameInstance
	{
		public Stage.Microgame microgame;
		public int difficulty;
		public AsyncOperation asyncOperation;
	}
	private Queue<Stage.Interruption> interruptionQueue;

	public enum AnimationPart
	{
		Idle,			//0	- Animation does nothing, camera is disabled | Until microgame ends
		Intro,			//1	- Part that introduces the microgame, animation begins in this state | 8 beats
		Outro,			//2	- Part that changes depending on if you win/lose | 4 beats
		LastBeat,		//3	- Starts at the last "beat" before the microgame ends, allowing the scene objects to pop back onscreen before unloading the microgame | 1 Beat
		SpeedUp,		//4 - Interruption between Outro and Intro when the game increases speed, speed is actually changed in Intro | 8 Beats
		BossStage,		//5 - Interruption between Outro and Intro when a boss is first encountered during this round | 8 beats
		NextRound		//6 - Used after a boss stage or when difficulty increases | 8 beats
	}

	void Start()
	{
		setAnimationPart(animationPart);
		beatLength = outroSource.clip.length / 4f;

		animationStartTime = Time.time;
		setMicrogameVictory(true, false);

		microgameCount = 0;
		Application.backgroundLoadingPriority = ThreadPriority.Low;

		microgameQueue = new Queue<MicrogameInstance>();
		updateMicrogameQueue(maxStockpiledScenes);

		resetLifeIndicators();

		speed = stage.getStartSpeed();
		Time.timeScale = getSpeedMult();

		voicePlayer.loadClips(stage.voiceSet);

		introSource.pitch = getSpeedMult();
		if (!muteMusic)
			introSource.Play();
		invokeIntroAnimations();
	}

	void Awake()
	{
		instance = this;
	}

	void updateMicrogameQueue(int maxQueueSize)
	{
		//Queue all available, unqueued microgames, make sure at least one is queued
		int index = microgameCount + microgameQueue.Count;
		while (microgameQueue.Count == 0 || (microgameQueue.Count < maxQueueSize && stage.isMicrogameDetermined(index)))
		{
			MicrogameInstance newInstance = new MicrogameInstance();
			newInstance.microgame = stage.getMicrogame(index);
			newInstance.difficulty = stage.getMicrogameDifficulty(newInstance.microgame, index);
			StartCoroutine(loadMicrogameAsync(newInstance));
			microgameQueue.Enqueue(newInstance);

			index++;
		}
	}

	IEnumerator loadMicrogameAsync(MicrogameInstance instance)
	{
		instance.asyncOperation = SceneManager.LoadSceneAsync(instance.microgame.microgameId + instance.difficulty.ToString()
		, LoadSceneMode.Additive);
		instance.asyncOperation.allowSceneActivation = false;
		instance.asyncOperation.priority = int.MaxValue - (microgameCount + microgameQueue.Count);	//Is this too much?

		while (instance.asyncOperation.progress < .9f)
		{
			yield return null;
		}
	}

	//TODO Delete?
	//void startNextRound()
	//{
	//	if (shuffleOn)
	//	{
	//		//Shuffle microgame order
	//		int index = 0, choice;
	//		Microgame hold;
	//		while (index < microgamePool.Length)
	//		{
	//			choice = Random.Range(index, microgamePool.Length);
	//			if (choice != index)
	//			{
	//				hold = microgamePool[index];
	//				microgamePool[index] = microgamePool[choice];
	//				microgamePool[choice] = hold;
	//			}
	//			index++;
	//		}
	//	}
	//}


	//Animation and music time is measured by "beats" here
	//the zeroth beat of each cycle is when the Intro animation starts
	void invokeOutroAnimations()
	{
		invokeAtBeat("updateToLastBeat", -5f);

		invokeAtBeat("updateToOutro", -4f);
	}

	void invokeInterruptions()
	{
		interruptionQueue = new Queue<Stage.Interruption>();
		Stage.Interruption[] interruptions = stage.getInterruptions(microgameCount);
		float interruptionBeats = 0f;

		int endSpeed = speed;
		for (int i = 0; i < interruptions.Length; i++)
		{
			Stage.Interruption interruption = interruptions[i];
			interruptionQueue.Enqueue(interruption);
			invokeAtBeat("updateToInterruption", interruptionBeats);

			if (i == 0)
				scheduleNextInterruptionAudio(outroPlayTime + (beatLength * 4f));

			endSpeed = getChangedSpeed(endSpeed, interruption);
			interruptionBeats += interruption.beatDuration;
		}
		animationStartTime += interruptionBeats * beatLength;
		introSource.pitch = getSpeedMult(endSpeed);
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

		//outroSource.pitch = getSpeedMult();
		//outroScheduledPlayTime = animationStartTime - (4f * beatLength);
		//AudioHelper.playScheduled(outroSource, outroScheduledPlayTime - Time.time);
	}

	void updateToOutro()
	{
		outroSource.pitch = getSpeedMult();
		if (!muteMusic)
			outroSource.Play();
		outroPlayTime = Time.time;

		setAnimationPart(AnimationPart.Outro);
		if (!microgameVictoryStatus)
			lowerLife();

		endMicrogame();
		microgameQueue.Dequeue();
		microgameCount++;

		updateMicrogameQueue(maxStockpiledScenes);

		float interruptionTime = animationStartTime;
		if (life > 0)
			invokeInterruptions();
		interruptionTime = animationStartTime - interruptionTime;

		invokeIntroAnimations();

		if (interruptionTime == 0f && !muteMusic && life > 0)
			AudioHelper.playScheduled(introSource, beatLength * 4f);

	}

	void updateToInterruption()
	{
		Stage.Interruption interruption = interruptionQueue.Dequeue();
		if (interruption.animation != AnimationPart.Idle)
			setAnimationPart(interruption.animation);

		if (!interruption.applySpeedChangeAtEnd)
			speed = getChangedSpeed(interruption);
		Time.timeScale = getSpeedMult();

		if (interruptionQueue.Count != 0)
		{
			Stage.Interruption nextInterruption = interruptionQueue.Peek();
			scheduleNextInterruptionAudio(interruption.scheduledPlayTime + (interruption.beatDuration * beatLength));
		}
		else
		{
			if (interruption.applySpeedChangeAtEnd)
				speed = getChangedSpeed(interruption);
			introSource.pitch = getSpeedMult();
			if (!muteMusic && interruption.beatDuration > 0f)
				AudioHelper.playScheduled(introSource, (interruption.scheduledPlayTime + (interruption.beatDuration * beatLength)) - Time.time);
		}
	}

	void scheduleNextInterruptionAudio(float timeToPlay)
	{
		Stage.Interruption interruption = interruptionQueue.Peek();
		interruption.scheduledPlayTime = timeToPlay;

		if (interruption.audioSource == null || interruption.audioClip == null)
			return;

		interruption.audioSource.Stop();
		interruption.audioSource.clip = interruption.audioClip;
		if (interruption.applySpeedChangeAtEnd)
			interruption.audioSource.pitch = getSpeedMult();
		else
			interruption.audioSource.pitch = getSpeedMult(getChangedSpeed(interruption));
		if (!muteMusic)
			AudioHelper.playScheduled(interruption.audioSource, timeToPlay - Time.time);
	}

	int getChangedSpeed(int speed, Stage.Interruption interruption)
	{
		switch (interruption.speedChange)
		{
			case (Stage.Interruption.SpeedChange.SpeedUp):
				return Mathf.Clamp(speed + 1, 1, MAX_SPEED);
			case (Stage.Interruption.SpeedChange.ResetSpeed):
				return 1;
			case (Stage.Interruption.SpeedChange.Custom):
				return Mathf.Clamp(stage.getCustomSpeed(microgameCount, interruption), 1, MAX_SPEED);
			default:
				return speed;
		}
	}

	int getChangedSpeed(Stage.Interruption interruption)
	{
		return getChangedSpeed(speed, interruption);
	}

	MicrogameInstance getCurrentMicrogameInstance()
	{
		return microgameQueue.Peek();
	}

	void updateToIntro()
	{

		//Placeholder "game over"
		if (life == 0)
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
		//introSource.pitch = getSpeedMult();

		updateMicrogameTraits();

		//TODO re-enable command warnings
		command.text = microgameTraits.localizedCommand;
		controlDisplay.sprite = controlSchemeSprites[(int)microgameTraits.controlScheme];
		controlDisplay.transform.FindChild("Text").GetComponent<TextMesh>().text =
			TextHelper.getLocalizedTextNoWarnings("control." + microgameTraits.controlScheme.ToString().ToLower(), getDefaultControlString());

		if (!introSource.isPlaying && !muteMusic)
			introSource.Play();
	}

	string getDefaultControlString()
	{
		switch (microgameTraits.controlScheme)
		{
			case(MicrogameTraits.ControlScheme.Key):
				return "USE DA KEYZ";
			case (MicrogameTraits.ControlScheme.Mouse):
				return "USE DA MOUSE";
			default:
				return "USE SOMETHING";
		}
	}

	void updateToIdle()
	{
		setAnimationPart(AnimationPart.Idle);
		//scene.SetActive(false);
	}

	void playMicrogameMusic()
	{
		microgameMusicSource.pitch = getSpeedMult();
		if (!muteMusic && microgameMusicSource.clip != null)
			microgameMusicSource.Play();
	}

	void updateMicrogameTraits()
	{
		MicrogameInstance instance = getCurrentMicrogameInstance();
		microgameTraits = MicrogameTraits.findMicrogameTraits(instance.microgame.microgameId, instance.difficulty);
		microgameTraits.onAccessInStage(instance.microgame.microgameId);
	}

	public float getBeatsRemaining()
	{
		return (animationStartTime + (beatLength * (-4f)) - Time.time) / beatLength;
		//return (animationStartTime + (beatLength * (8f + (float)MicrogameController.instance.beatDuration) + getInterruptionBeats()) - Time.time) / beatLength;
	}

	void startMicrogame()
	{
		getCurrentMicrogameInstance().asyncOperation.allowSceneActivation = true;
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
	}

	void endMicrogame()
	{
		if (!getVictoryDetermined())
			voicePlayer.playClip(microgameVictoryStatus, 0f);
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

		stage.onMicrogameEnd(microgameCount, microgameVictoryStatus);

		//sceneLoader.removeMicrogame(microgamePool[microgameIndex]);
	}

	//TODO Delete?
	//int getMicrogameDifficulty(int index)
	//{
	//	Microgame microgame = microgamePool[index];

	//	if (!difficultyIncreaseOn)
	//		return microgame.baseDifficulty;

	//	int difficulty = round + microgame.baseDifficulty - 1;

	//	if (difficulty > 3)
	//		difficulty = 3;

	//	return difficulty;

	//}

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

		//Debug scene reset
		if (Input.GetKeyDown(KeyCode.R))
		{
			SceneManager.LoadScene(gameObject.scene.buildIndex);
		}
	}

	public void setAnimationPart(AnimationPart animationPart)
	{
		this.animationPart = animationPart;
		setAnimationInteger("part", (int)animationPart);
	}

	void updateCursorVisibility()
	{
		Cursor.visible = microgameTraits.controlScheme == MicrogameTraits.ControlScheme.Mouse;
	}

	/// <summary>
	/// Do NOT call this from a microgame, use MicrogameController.setVictory() instead
	/// </summary>
	/// <param name="victory"></param>
	/// <param name="final"></param>
	public void setMicrogameVictory(bool victory, bool final)
	{
		if (godMode)
			victory = true;

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
		microgameVictoryStatus = victory;

		if (final)
			setFinalAnswer();
		victoryDetermined = final;

		setAnimationBool("microgameVictory", microgameVictoryStatus);
	}

	void setFinalAnswer()
	{
		//Can't have this happening in the beat before the microgame actually starts
		if (animationPart != AnimationPart.Idle && animationPart != AnimationPart.LastBeat)
		{
			Invoke("setFinalAnswer", beatLength);
			return;
		}

		victoryDetermined = true;
		voicePlayer.playClip(microgameVictoryStatus,
			getMicrogameVictory() ? MicrogameController.instance.getTraits().victoryVoiceDelay : MicrogameController.instance.getTraits().failureVoiceDelay);

		if (MicrogameController.instance.getTraits().GetType() == typeof(MicrogameBossTraits))
		{
			float endInBeats = microgameVictoryStatus ? ((MicrogameBossTraits)MicrogameController.instance.getTraits()).victoryEndBeats
				: ((MicrogameBossTraits)MicrogameController.instance.getTraits()).failureEndBeats;
			CancelInvoke();
			animationStartTime = Time.time + ((endInBeats + 4f) * beatLength);
			invokeOutroAnimations();
		}
		else if (MicrogameController.instance.getTraits().canEndEarly)
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
		return microgameVictoryStatus;
	}

	public bool getVictoryDetermined()
	{
		return victoryDetermined;
	}

	void resetLifeIndicators()
	{
		life = stage.getMaxLife();
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

	void invokeAtBeat(string function, float beatFromCycleStart)
	{
		invokeAtTime(function, animationStartTime + (beatLength * beatFromCycleStart));
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

}
