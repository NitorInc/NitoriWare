using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSplash : MonoBehaviour
{

#pragma warning disable 0649
    [SerializeField]
    private float fadeSpeed;
    [SerializeField]
    private Image blocker;
    [SerializeField]
    private string goalScene;
    [SerializeField]
    private float shiftTime;
#pragma warning restore 0649

    private State state;
    private enum State
    {
        WaitForLocalization,
        FadeIn,
        Show,
        FadeOut
    }


	void Start()
	{
        state = State.WaitForLocalization;
        blocker.gameObject.SetActive(true);
	}
	
	void Update()
	{
        switch(state)
        {
            case(State.WaitForLocalization):
                if (!string.IsNullOrEmpty(TextHelper.getLoadedLanguageID()))
                    state = State.FadeIn;
                break;
            case (State.FadeIn):
                UpdateFade(false);
                break;
            case (State.Show):
                break;
            case (State.FadeOut):
                UpdateFade(true);
                break;
            default:
                break;

        }
    }

    void UpdateFade(bool fadeIn)
    {
        float diff = fadeSpeed * Time.deltaTime,
            alpha = getBlockerAlpha();
        if (fadeIn)
        {
            if (alpha + diff >= 1f)
            {
                setBlockerAlpha(1f);
            }
            else
                setBlockerAlpha(alpha + diff);
        }
        else
        {
            if (alpha - diff <= 0f)
            {
                setBlockerAlpha(0f);
                blocker.gameObject.SetActive(false);
                state = State.Show;
            }
            else
                setBlockerAlpha(alpha - diff);
        }
    }

    public void startGame()
    {
        GameController.instance.sceneShifter.startShift(goalScene, shiftTime);
        state = State.FadeOut;
        blocker.gameObject.SetActive(true);
    }

    float getBlockerAlpha()
    {
        return blocker.color.a;
    }

    void setBlockerAlpha(float alpha)
    {
        Color color = blocker.color;
        color.a = alpha;
        blocker.color = color;
    }
}
