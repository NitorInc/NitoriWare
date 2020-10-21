using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StageController : MonoBehaviour
{
	[SerializeField]
	private Stage stage;
	public bool godMode, ignoreStageMusic;
    public float editorStartDelay = .2f;

	public int maxStockpiledScenes;
    public ThreadPriority sceneLoadPriority;

	private int microgameCount, life;
    private bool sceneStarted;
    private float sceneStartTime;

	public AnimationPart animationPart = AnimationPart.Intro;

	[SerializeField]
	private MicrogameEventListener microgameEventListener;
    [SerializeField]
    private MicrogamePlayer microgamePlayer;

    public VoicePlayer voicePlayer;
	public Camera stageCamera;
	public Animator[] lifeIndicators;
	public AudioSource outroSource, introSource, speedUpSource, microgameMusicSource;
	public AudioClip victoryClip, failureClip;
	public GameObject scene;
	public Sprite[] controlSchemeSprites;
	public SpeedController speedController;

    public CommandDisplay commandDisplay;
    public ControlDisplay controlDisplay;

    public StageGameOverMenu gameOverMenu;

	public float beatLength;

	private float animationStartTime, outroPlayTime;
    private Animator[] sceneAnimators;
	
	//private Queue<Stage.Interruption> interruptionQueue;

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
    
    void Awake()
    {
        sceneAnimators = transform.root.GetComponentsInChildren<Animator>();
        if (commandDisplay == null)
            commandDisplay = transform.parent.Find("UI").Find("Command").GetComponent<CommandDisplay>();
        if (controlDisplay == null)
            controlDisplay = GameObject.FindObjectOfType<ControlDisplay>();
        //controlDisplay.stageController = this;
    }

    void Start()
	{
		beatLength = outroSource.clip.length / 4f;
		Application.backgroundLoadingPriority = sceneLoadPriority;
		voicePlayer.loadClips(stage.getVoiceSet());

        setAnimationPart(AnimationPart.Intro);

        Time.timeScale = 0f;
        Cursor.visible = false;
        sceneStartTime = Time.realtimeSinceStartup + (Application.isEditor ? editorStartDelay : 0f); // Editor mode has a bug when trying to load scenes async too soon and will activate them anyway
    }

    private void Update()
    {
        if (!sceneStarted && Time.realtimeSinceStartup >= sceneStartTime)
        {
            sceneStarted = true;
            resetStage(Time.time, true);
        }
    }

    void resetStage(float startTime, bool firstTime)
    {
        //stage.onStageStart(this);

        microgameCount = 0;
        speedController.Speed = stage.getStartSpeed();
		speedController.ApplySpeed();
        animationStartTime = startTime;

        UpdatePlayerMicrogameQueue(maxStockpiledScenes);

        resetLifeIndicators();
        MicrogameNumber.instance.resetNumber();

        introSource.pitch = speedController.GetSpeedTimeScaleMult();
        if (!ignoreStageMusic && firstTime)
            AudioHelper.playScheduled(introSource, startTime - Time.time);
        //updateMicrogameTraits();

        invokeIntroAnimations();
    }

	void UpdatePlayerMicrogameQueue(int maxQueueSize)
	{
		//Queue all available, unqueued microgames, make sure at least one is queued
		var queueCount = microgamePlayer.QueuedMicrogameCount();
		int index = microgameCount + queueCount;
		while (queueCount == 0 || (queueCount < maxQueueSize && stage.isMicrogameDetermined(index)))
		{
			var stageMicrogame = stage.getMicrogame(index);
			var microgame = stageMicrogame.microgame;
            var difficulty = stageMicrogame.difficulty;
            var newSession = microgame.CreateSession(difficulty);
			microgamePlayer.EnqueueSession(newSession);

			queueCount++;
			index++;
		}
	}

	void invokeOutroAnimations()
	{
		invokeAtBeat("updateToLastBeat", -5f);

        invokeAtBeat("updateToOutro", -4f);

        //invokeAtBeat("unloadMicrogame", 2f);
    }

	//void invokeInterruptions()
	//{

	//	interruptionQueue = new Queue<Stage.Interruption>();
	//	Stage.Interruption[] interruptions = stage.getInterruptions(microgameCount);
	//	float interruptionBeats = 0f;

	//	int endSpeed = speedController.Speed;
	//	for (int i = ; i < interruptions.Length; i++)
	//	{
	//		Stage.Interruption interruption = interruptions[i];
	//		interruptionQueue.Enqueue(interruption);
	//		invokeAtBeat("updateToInterruption", interruptionBeats);

	//		if (i == 0)
	//			scheduleNextInterruptionAudio(outroPlayTime + (beatLength * 4f));

	//		endSpeed = getChangedSpeed(endSpeed, interruption);
	//		interruptionBeats += interruption.beatDuration;
	//	}
	//	animationStartTime += interruptionBeats * beatLength;
	//	introSource.pitch = speedController.GetSpeedTimeScaleMult(endSpeed);
	//}

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
	}

	void updateToOutro()
	{
		outroSource.pitch = speedController.GetSpeedTimeScaleMult();
		if (!ignoreStageMusic)
			outroSource.Play();
		outroPlayTime = Time.time;

		setAnimationPart(AnimationPart.Outro);
		if (!microgamePlayer.CurrentMicrogameSession.VictoryStatus && !godMode)
			lowerLife();

		endMicrogame();
		microgameCount++;


		//Game over and interruption check
		if (life > 0)
		{
			UpdatePlayerMicrogameQueue(maxStockpiledScenes);

			float interruptionTime = animationStartTime;
			//invokeInterruptions();
			interruptionTime = animationStartTime - interruptionTime;
			invokeIntroAnimations();
			if (interruptionTime == 0f && !ignoreStageMusic)
				AudioHelper.playScheduled(introSource, beatLength * 4f);
		}
		else
		{
			//TODO game over music
			invokeAtBeat("updateToGameOver", 0f);
		}

	}

	//void updateToInterruption()
	//{
	//	Stage.Interruption interruption = interruptionQueue.Dequeue();
	//	if (interruption.animation != AnimationPart.Idle)
	//		setAnimationPart(interruption.animation);

	//	if (!interruption.applySpeedChangeAtEnd)
	//		speedController.Speed = getChangedSpeed(interruption);
	//	speedController.ApplySpeed();

	//	if (interruptionQueue.Count != 0)
	//	{
	//		scheduleNextInterruptionAudio(interruption.scheduledPlayTime + (interruption.beatDuration * beatLength));
	//	}
	//	else
	//	{
	//		if (interruption.applySpeedChangeAtEnd)
	//			speedController.Speed= getChangedSpeed(interruption);
	//		introSource.pitch = speedController.GetSpeedTimeScaleMult();
	//		if (!ignoreStageMusic && interruption.beatDuration > 0f)
	//			AudioHelper.playScheduled(introSource, (interruption.scheduledPlayTime + (interruption.beatDuration * beatLength)) - Time.time);
	//	}
	//}

	//void scheduleNextInterruptionAudio(float timeToPlay)
	//{
	//	Stage.Interruption interruption = interruptionQueue.Peek();
	//	interruption.scheduledPlayTime = timeToPlay;

	//	if (interruption.audioSource == null || interruption.audioClip == null)
	//		return;

	//	interruption.audioSource.Stop();
	//	interruption.audioSource.clip = interruption.audioClip;
	//	if (interruption.applySpeedChangeAtEnd)
	//		interruption.audioSource.pitch = speedController.GetSpeedTimeScaleMult();
	//	else
	//		interruption.audioSource.pitch = speedController.GetSpeedTimeScaleMult(getChangedSpeed(interruption));
	//	if (!ignoreStageMusic)
	//		AudioHelper.playScheduled(interruption.audioSource, timeToPlay - Time.time);
	//}

	//int getChangedSpeed(int speed, Stage.Interruption interruption)
	//{
	//	switch (interruption.speedChange)
	//	{
	//		case (Stage.Interruption.SpeedChange.SpeedUp):
	//			return Mathf.Clamp(speed + 1, 1, SpeedController.MAX_SPEED);
	//		case (Stage.Interruption.SpeedChange.ResetSpeed):
	//			return 1;
	//		case (Stage.Interruption.SpeedChange.Custom):
	//			return Mathf.Clamp(stage.getCustomSpeed(microgameCount, interruption), 1, SpeedController.MAX_SPEED);
	//		default:
	//			return speed;
	//	}
	//}

	//int getChangedSpeed(Stage.Interruption interruption)
	//{
	//	return getChangedSpeed(speedController.Speed, interruption);
	//}

	void updateToIntro()
	{

		//commandDisplay.setText(
		//	microgamePlayer.CurrentMicrogameSession.GetLocalizedCommand(),
		//	microgamePlayer.CurrentMicrogameSession.GetCommandAnimatorOverride());
		//controlDisplay.setControlScheme(microgamePlayer.CurrentMicrogame.controlScheme);

		setAnimationPart(AnimationPart.Intro);

		speedController.ApplySpeed();
        


		if (!introSource.isPlaying && !ignoreStageMusic)
			introSource.Play();
    }

	void updateToGameOver()
	{
		microgamePlayer.CancelRemainingMicrogames();
		setAnimationPart(AnimationPart.GameOver);
		speedController.Speed = 1;
		speedController.ApplySpeed();
		CancelInvoke();
		introSource.Stop();
		gameOverMenu.gameObject.SetActive(true);
        gameOverMenu.Trigger(MicrogameNumber.instance.getNumber());
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
		if (microgameMusicSource.clip != null)
			microgameMusicSource.Play();
	}

    public void lowerScore()
    {
        MicrogameNumber.instance.decreaseNumber();
    }

	public float getBeatsRemaining()
	{
		return (animationStartTime + (beatLength * (-4f)) - Time.time) / beatLength;
	}

	void startMicrogame()
    {
		microgamePlayer.ActivateScene();
		UpdateMicrogameVictory(microgamePlayer.CurrentMicrogameSession);
        stage.onMicrogameStart(microgameCount);
    }

    public void onMicrogameAwake(Microgame.Session session, UnityEngine.SceneManagement.Scene scene)
	{
		if (session.Cancelled)
			return;

        stageCamera.tag = "Camera";
        //microgameMusicSource.clip = session.GetMusicClip();

        //animationStartTime += beatLength * (12f + (float)session.microgame.getDurationInBeats());

        Cursor.lockState = session.GetCursorLockMode();

  //      MicrogameTimer.instance.beatsLeft = getBeatsRemaining();
		//MicrogameTimer.instance.gameObject.SetActive(true);
		//MicrogameTimer.instance.invokeTick();
		invokeOutroAnimations();
	}

	void endMicrogame()
	{

		//if (!getVictoryDetermined())
		//	voicePlayer.playClip(microgamePlayer.CurrentMicrogameSession.VictoryStatus, 0f);
		//else
		//	voicePlayer.ForcePlay();

		stageCamera.tag = "MainCamera";
		CameraController.instance = Camera.main.GetComponent<CameraController>();

		microgameMusicSource.Stop();
		Cursor.visible = false;
        Cursor.lockState = GameController.DefaultCursorMode;
        stageCamera.GetComponent<AudioListener>().enabled = true;

  //      MicrogameTimer.instance.beatsLeft = 0f;
		//MicrogameTimer.instance.gameObject.SetActive(false);


        //stage.onMicrogameEnd(microgameCount, microgamePlayer.CurrentMicrogameSession.VictoryStatus);

		microgamePlayer.StopMicrogame();

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
		Cursor.visible = microgamePlayer.CurrentMicrogame.controlScheme == Microgame.ControlScheme.Mouse;
	}

	/// <summary>
	/// Do NOT call this from a microgame, use MicrogameController.setVictory() instead
	/// </summary>
	/// <param name="victory"></param>
	/// <param name="final"></param>
	public void UpdateMicrogameVictory(Microgame.Session session)
	{
		var victory = session.VictoryStatus;
		if (victory && outroSource.clip != victoryClip)
		{
			outroSource.clip = victoryClip;
		}
		else if (!victory && outroSource.clip != failureClip)
		{
			outroSource.clip = failureClip;
		}

		setAnimationBool("microgameVictory", victory);
	}

	public void setFinalAnswer(Microgame.Session session) => setFinalAnswer();

	void setFinalAnswer()
	{
		//Can't have this happening in the beat before the microgame actually starts
		if (animationPart != AnimationPart.Idle && animationPart != AnimationPart.LastBeat)
		{
			Invoke("setFinalAnswer", beatLength);
			return;
		}

		var session = microgamePlayer.CurrentMicrogameSession;
		var microgame = microgamePlayer.CurrentMicrogame;
        var victory = session.VictoryStatus;
        
		
		if (microgame.isBossMicrogame())
		{
			float endInBeats = victory ? ((MicrogameBoss)microgame).victoryEndBeats
				: ((MicrogameBoss)microgame).failureEndBeats;
			CancelInvoke();
			animationStartTime = Time.time + ((endInBeats + 4f) * beatLength);
			invokeOutroAnimations();
		}
		else if (microgame.canEndEarly)
		{
			//float beatOffset = MicrogameTimer.instance.beatsLeft - 2f;
			//beatOffset -= beatOffset % 4f;
			//if (beatOffset > 0f)
			//{
			//	if (beatOffset > 8f)
			//		beatOffset = 8f;

			//	endMicrogameEarly(beatOffset);
			//}
		}
	}

	public bool getMicrogameVictory() => microgamePlayer.CurrentMicrogameSession.VictoryStatus;

	public bool getVictoryDetermined() => microgamePlayer.CurrentMicrogameSession.WasVictoryDetermined;

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
		//MicrogameTimer.instance.CancelInvoke();
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
