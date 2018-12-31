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
    public ThreadPriority sceneLoadPriority;

	private int microgameCount, life;
	private bool microgameVictoryStatus, victoryDetermined;

	public AnimationPart animationPart = AnimationPart.Intro;

	public VoicePlayer voicePlayer;
	public Camera stageCamera;
	public Animator[] lifeIndicators;
	public AudioSource outroSource, introSource, speedUpSource, microgameMusicSource;
	public AudioClip victoryClip, failureClip;
	public GameObject scene;
	public Sprite[] controlSchemeSprites;

	public StageGameOverMenu gameOverMenu;

	public static float beatLength;

	private MicrogameTraits microgameTraits;
	private float animationStartTime, outroPlayTime;
    private Animator[] sceneAnimators;

    private CommandDisplay commandDisplay;
    private ControlDisplay controlDisplay;

	private Queue<MicrogameInstance> microgameQueue;
	private class MicrogameInstance
	{
		public MicrogameCollection.Microgame microgame;
		public int difficulty;
		public AsyncOperation asyncOperation;
        public Scene scene;
        public bool isBeingUnloaded;
	}
	private Queue<Stage.Interruption> interruptionQueue;
    private MicrogameInstance finishedMicrogame;

	public enum AnimationPart
	{
		Idle,			//0	- Animation does nothing, camera is disabled | Until microgame ends
		Intro,			//1	- Part that introduces the microgame, animation begins in this state | 8 beats
		Outro,			//2	- Part that changes depending on if you win/lose | 4 beats
		LastBeat,		//3	- Starts at the last "beat" before the microgame ends, allowing the scene objects to pop back onscreen before unloading the microgame | 1 Beat
		SpeedUp,		//4 - Interruption between Outro and Intro when the game increases speed, speed is actually changed in Intro | 8 Beats
		BossStage,		//5 - Interruption between Outro and Intro when a boss is first encountered during this round | 8 beats
		NextRound,		//6 - Used after a boss stage or when difficulty increases | 8 beats
		GameOver,		//7 - Player has lost all of their lives | Until stage is quit or restarted
		Retry,			//8 - Player has hit retry after Game Over | 4 beats
        OneUp,          //9 - Player has won the boss stage and gets an extra life | 8 beats (placeholder)
        WonStage        //10 - Player has won a character stage for the first time | 8 beats, ends stage
	}

	void Start()
	{
		beatLength = outroSource.clip.length / 4f;
		Application.backgroundLoadingPriority = sceneLoadPriority;
		voicePlayer.loadClips(stage.getVoiceSet());

		setAnimationPart(AnimationPart.Intro);
		resetStage(Time.time, true);
        Cursor.visible = false;
	}

	void resetStage(float startTime, bool firstTime)
	{
		stage.onStageStart();

		microgameCount = 0;
		speed = stage.getStartSpeed();
		Time.timeScale = getSpeedMult();
		animationStartTime = startTime;

		microgameQueue = new Queue<MicrogameInstance>();
		updateMicrogameQueue(maxStockpiledScenes);

		resetLifeIndicators();
		MicrogameNumber.instance.resetNumber();

		introSource.pitch = getSpeedMult();
		if (!muteMusic && firstTime)
			AudioHelper.playScheduled(introSource, startTime - Time.time);
        updateMicrogameTraits();

		invokeIntroAnimations();
	}

	void Awake()
	{
		instance = this;
        sceneAnimators = transform.root.GetComponentsInChildren<Animator>();
        commandDisplay = transform.parent.Find("UI").Find("Command").GetComponent<CommandDisplay>();
        controlDisplay = transform.parent.Find("UI").Find("Control Display").GetComponent<ControlDisplay>();
    }

	void updateMicrogameQueue(int maxQueueSize)
	{
		//Queue all available, unqueued microgames, make sure at least one is queued
		int index = microgameCount + microgameQueue.Count;
		while (microgameQueue.Count == 0 || (microgameQueue.Count < maxQueueSize && stage.isMicrogameDetermined(index)))
		{
			MicrogameInstance newInstance = new MicrogameInstance();
            Stage.Microgame stageMicrogame = stage.getMicrogame(index);
            newInstance.microgame = MicrogameCollection.instance.getMicrogame(stageMicrogame.microgameId);
			newInstance.difficulty = stage.getMicrogameDifficulty(stageMicrogame, index);
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

        yield return null;
        float holdSpeed = 0f;
        while (instance.asyncOperation.progress < .9f)
        {
            if (!instance.isBeingUnloaded && holdSpeed == 0f && instance.asyncOperation.allowSceneActivation)
            {
                holdSpeed = Time.timeScale;
                Time.timeScale = 0f;
                Application.backgroundLoadingPriority = ThreadPriority.High;
            }
            yield return null;
        }
        if (holdSpeed > 0f)
        {
            Time.timeScale = holdSpeed;
            Application.backgroundLoadingPriority = sceneLoadPriority;
        }
    }

	IEnumerator unloadMicrogamesRecursiveAsync(Queue<MicrogameInstance> queue)
	{
        MicrogameInstance instance = queue.Dequeue();
		instance.asyncOperation.allowSceneActivation = true;
		while (!instance.asyncOperation.isDone)
		{
			yield return null;
		}

        instance.isBeingUnloaded = true;
        instance.asyncOperation = SceneManager.UnloadSceneAsync(instance.microgame.microgameId + instance.difficulty.ToString());
        while (instance.asyncOperation == null)
        {
            yield return null;
        }
        while (!instance.asyncOperation.isDone)
        {
            yield return null;
        }
        if (queue.Count > 0)
            StartCoroutine(unloadMicrogamesRecursiveAsync(queue));
    }

	//Animation and music time is measured by "beats" here
	//the zeroth beat of each cycle is when the Intro animation starts
	void invokeOutroAnimations()
	{
		invokeAtBeat("updateToLastBeat", -5f);

        invokeAtBeat("updateToOutro", -4f);

        invokeAtBeat("unloadMicrogame", 2f);

        invokeAtBeat("updateMicrogameTraits", -2f);
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
		setAnimationPart(AnimationPart.LastBeat);
		MicrogameController.instance.displayCommand("");
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
		microgameCount++;


		//Game over and interruption check
		if (life > 0)
		{
			updateMicrogameQueue(maxStockpiledScenes);

			float interruptionTime = animationStartTime;
			invokeInterruptions();
			interruptionTime = animationStartTime - interruptionTime;
			invokeIntroAnimations();
			if (interruptionTime == 0f && !muteMusic)
				AudioHelper.playScheduled(introSource, beatLength * 4f);
		}
		else
		{
			//TODO game over music
			invokeAtBeat("updateToGameOver", 0f);
		}

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

		setAnimationPart(AnimationPart.Intro);

		Time.timeScale = getSpeedMult();

		commandDisplay.setText(microgameTraits.localizedCommand, microgameTraits.commandAnimatorOverride);
        controlDisplay.setControlScheme(microgameTraits.controlScheme);

		if (!introSource.isPlaying && !muteMusic)
			introSource.Play();
	}

	void updateToGameOver()
	{
		if (microgameQueue.Count > 0)
		{
            microgameQueue.Enqueue(finishedMicrogame);
			StartCoroutine(unloadMicrogamesRecursiveAsync(microgameQueue));
		}
		setAnimationPart(AnimationPart.GameOver);
		speed = 1;
		Time.timeScale = 1f;
		CancelInvoke();
		introSource.Stop();
		gameOverMenu.gameObject.SetActive(true);
        gameOverMenu.initialize(MicrogameNumber.instance.getNumber());
        Cursor.visible = true;
	}

    public bool isGameOver()
    {
        return animationPart == AnimationPart.GameOver || animationPart == AnimationPart.WonStage;
    }

	void updateToIdle()
	{
		setAnimationPart(AnimationPart.Idle);
	}

	void playMicrogameMusic()
	{
		microgameMusicSource.pitch = getSpeedMult();
		if (!muteMusic && microgameMusicSource.clip != null)
			microgameMusicSource.Play();
	}

    public void lowerScore()
    {
        MicrogameNumber.instance.decreaseNumber();
    }

    void updateMicrogameTraits()
	{
        if (microgameQueue.Count <= 0)
            return;

		MicrogameInstance instance = getCurrentMicrogameInstance();
        microgameTraits = instance.microgame.difficultyTraits[instance.difficulty - 1];
		microgameTraits.onAccessInStage(instance.microgame.microgameId, instance.difficulty);
	}

	public float getBeatsRemaining()
	{
		return (animationStartTime + (beatLength * (-4f)) - Time.time) / beatLength;
	}

	void startMicrogame()
	{
		getCurrentMicrogameInstance().asyncOperation.allowSceneActivation = true;
        stage.onMicrogameStart(microgameCount);
    }

	public void resetVictory()
	{
		victoryDetermined = false;
		setMicrogameVictory(microgameTraits.defaultVictory, false);
	}

	//Called from MicrogameController on Awake()
	public void onMicrogameAwake()
	{
		animationStartTime += beatLength * (12f + (float)microgameTraits.getDurationInBeats());
        microgameQueue.Peek().scene = MicrogameController.instance.gameObject.scene;

        stageCamera.GetComponent<AudioListener>().enabled = false;
        Cursor.lockState = microgameTraits.cursorLockState;

        MicrogameTimer.instance.beatsLeft = StageController.instance.getBeatsRemaining();
		MicrogameTimer.instance.gameObject.SetActive(true);
		MicrogameTimer.instance.invokeTick();
		invokeOutroAnimations();
	}

	void endMicrogame()
	{
		if (!getVictoryDetermined())
			voicePlayer.playClip(microgameVictoryStatus, 0f);
		else
			voicePlayer.forcePlay();

		MicrogameController.instance.shutDownMicrogame();
        SceneManager.SetActiveScene(gameObject.scene);

		stageCamera.tag = "MainCamera";
		CameraController.instance = Camera.main.GetComponent<CameraController>();

		microgameMusicSource.Stop();
		Cursor.visible = false;
        Cursor.lockState = GameController.DefaultCursorMode;
        stageCamera.GetComponent<AudioListener>().enabled = true;

        MicrogameTimer.instance.beatsLeft = 0f;
		MicrogameTimer.instance.gameObject.SetActive(false);


        stage.onMicrogameEnd(microgameCount, microgameVictoryStatus);

        finishedMicrogame = microgameQueue.Dequeue();
        MicrogameController.instance = null;
	}

    void unloadMicrogame()
    {
        finishedMicrogame.isBeingUnloaded = true;
        SceneManager.UnloadSceneAsync(finishedMicrogame.scene);
        MicrogameController.instance = null;
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

    public int getLife()
    {
        return life;
    }

    public void setLife(int life)
    {
        this.life = life;
        for (int i = 0; i < lifeIndicators.Length; i++)
        {
            lifeIndicators[i].SetInteger("life", life - i);
        }
    }

    public void retry()
    {
        setAnimationPart(AnimationPart.Retry);
        resetStage(Time.time + (beatLength * 4f), false);
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
			getMicrogameVictory() ? microgameTraits.victoryVoiceDelay : microgameTraits.failureVoiceDelay);

		if (microgameTraits.isBossMicrogame())
		{
			float endInBeats = microgameVictoryStatus ? ((MicrogameBossTraits)microgameTraits).victoryEndBeats
				: ((MicrogameBossTraits)microgameTraits).failureEndBeats;
			CancelInvoke();
			animationStartTime = Time.time + ((endInBeats + 4f) * beatLength);
			invokeOutroAnimations();
		}
		else if (microgameTraits.canEndEarly)
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
			lifeIndicators[i].SetInteger("life", life - i);
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

    public Stage getStage()
    {
        return stage;
    }

	private void setAnimationInteger(string name, int state)
	{
		foreach (Animator animator in sceneAnimators)
		{
			animator.SetInteger(name, state);
		}
	}

	private void setAnimationBool(string name, bool state)
	{
		foreach (Animator animator in sceneAnimators)
		{
			animator.SetBool(name, state);
		}
	}

}
