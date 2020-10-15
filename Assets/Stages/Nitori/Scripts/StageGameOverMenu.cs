using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Events;

public class StageGameOverMenu : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField]
    private string quitScene = "Title";
    [SerializeField]
    private Text scoreNumberText, highScoreNumberText;
    [SerializeField]
    private GameObject highScoreIndicator;
    [SerializeField]
    private float showcaseModeCooldown = 60f;
    [SerializeField]
    private float quitShiftTime;
    [SerializeField]
    private AudioSource musicSource;
    [SerializeField]
    private float retryTransitionDuration = .5f;
#pragma warning restore 0649

    private float BGGoalAlpha;
    private float lastGameOverTime;
    private bool gameOverActive;

    private Animator menuAnimator;
    [HideInInspector]
    public UnityEvent onRetry;

    private void Awake()
    {
        menuAnimator = GetComponent<Animator>();
    }

    public void Trigger(int score)
    {
        lastGameOverTime = Time.time;

        gameOverActive = true;
        PauseManager.disablePause = true;

        menuAnimator.SetBool("Active", true);

        musicSource.Play();

        int currentHighScore = PrefsHelper.getHighScore(gameObject.scene.name);
        if (highScoreIndicator != null)
            highScoreIndicator.SetActive(score > currentHighScore);
        if (score > currentHighScore)
        {
            currentHighScore = score;
            PrefsHelper.setHighScore(gameObject.scene.name, currentHighScore);
        }
        setNumber(scoreNumberText, score);
        setNumber(highScoreNumberText, currentHighScore);
	}

    void Update()
    {
        if (gameOverActive
            && GameController.instance.ShowcaseMode
            && Time.time > lastGameOverTime + showcaseModeCooldown)
        {
            GameMenu.subMenu = GameMenu.SubMenu.Splash;
            quit();
        }
    }

    public void retry()
    {
        // TODO callback to stage FSM
        menuAnimator.SetBool("Active", false);
        Invoke("EndRetryTransition", retryTransitionDuration);
        onRetry.Invoke();
    }

    void EndRetryTransition()
    {
        PauseManager.disablePause = false;
        musicSource.Stop();
    }

    public void quit()
    {
        menuAnimator.SetTrigger("Quit");
        GameController.instance.sceneShifter.startShift(quitScene, quitShiftTime);
        enabled = false;
    }

    void setNumber(Text textComponent, int score)
    {
        textComponent.text = textComponent.text.Substring(0, textComponent.text.Length - 3);

        int number = score;
        textComponent.text += number.ToString("D3");
    }
}
