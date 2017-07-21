using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StageGameOverMenu : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField]
    private GameObject menuItems;
    [SerializeField]
    private Text scoreNumberText, highScoreNumberText;
    [SerializeField]
    private GameObject highScoreIndicator;
    [SerializeField]
    private Image fadeBG;
    [SerializeField]
    private float fadeSpeed;
    [SerializeField]
    private float quitShiftTime;
    [SerializeField]
    private MenuButton[] menuButtons;
    [SerializeField]
    private FadingMusic fadingMusic;
#pragma warning restore 0649

    private float BGGoalAlpha;

    private State state;
    private enum State
    {
        FadeIn,
        Menu,
        FadeOut
    }


    public void initialize(int score)
    {
        if (BGGoalAlpha == 0f)
            BGGoalAlpha = getBGAlpha();
        setBGAlpha(0f);

        state = State.FadeIn;
        gameObject.SetActive(true);
        PauseManager.disablePause = true;

        fadingMusic.GetComponent<AudioSource>().time = 0f;
        fadingMusic.startFade();

        foreach (MenuButton menuButton in menuButtons)
        {
            menuButton.forceDisable = false;
        }

        int currentHighScore = PrefsHelper.getHighScore(gameObject.scene.name);
        if (score > currentHighScore)
        {
            if (currentHighScore > 0)
                highScoreIndicator.SetActive(true);
            currentHighScore = score;
            PrefsHelper.setHighScore(gameObject.scene.name, currentHighScore);
        }
        setNumber(scoreNumberText, score);
        setNumber(highScoreNumberText, currentHighScore);

	}

    void Update()
    {
        switch(state)
        {
            case (State.FadeIn):
                UpdateFade(true);
                break;
            case (State.Menu):
                break;
            case (State.FadeOut):
                UpdateFade(false);
                break;
            default:
                break;
        }
    }

    void UpdateFade(bool fadeIn)
    {
        float diff = fadeSpeed * Time.deltaTime,
            alpha = getBGAlpha();
        if (fadeIn)
        {
            if (alpha + diff >= BGGoalAlpha)
            {
                setBGAlpha(BGGoalAlpha);
                menuItems.SetActive(true);
                foreach (MenuButton menuButton in menuButtons)
                {
                    menuButton.GetComponent<Animator>().Play("Normal");
                }
                state = State.Menu;
            }
            else
                setBGAlpha(alpha + diff);
        }
        else
        {
            if (alpha - diff <= 0f)
            {
                setBGAlpha(0f);
                PauseManager.disablePause = false;
                //gameObject.SetActive(false);
            }
            else
                setBGAlpha(alpha - diff);
        }
    }

    public void retry()
    {
        if (state != State.Menu)
            return;

        Invoke("disableMenuItems", .15f);
        StageController.instance.retry();
        state = State.FadeOut;
        fadingMusic.startFade();
    }

    public void quit()
    {
        if (state != State.Menu)
            return;

        GameController.instance.sceneShifter.startShift("Title", quitShiftTime);
        fadingMusic.startFade();
    }

    void disableMenuItems()
    {
        menuItems.SetActive(false);
    }

    void setNumber(Text textComponent, int score)
    {
        textComponent.text = textComponent.text.Substring(0, textComponent.text.Length - 3);

        int number = score;
        textComponent.text += number.ToString("D3");
    }

    void setBGAlpha(float alpha)
    {
        Color color = fadeBG.color;
        color.a = alpha;
        fadeBG.color = color;
    }

    float getBGAlpha()
    {
        return fadeBG.color.a;
    }
}
