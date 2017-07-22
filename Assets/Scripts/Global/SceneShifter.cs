using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneShifter : MonoBehaviour
{
    private const string QuitString = "QUITGAME";

    private const float DefaultShiftDuration = 1f, DefaultFadeDuration = .25f;
    private const float MinBlackScreenTime = .5f;

#pragma warning disable 0649   //Serialized Fields
    [SerializeField]
    private bool useRealTime = true;
    [SerializeField]
    private Image blocker;
#pragma warning restore 0649

    private string goalScene;
    private float shiftDuration;
    private float fadeDuration;
    private bool leavingScene;
    private float shiftStartTime;
    private float sceneLoadedTime;

    private AsyncOperation operation;
	
	void Update()
	{
        if (leavingScene)
        {
            float timeLeft = shiftStartTime + shiftDuration - getCurrentTime();
            if (timeLeft < fadeDuration)
            {
                setBlockerAlpha(Mathf.Lerp(1f, 0f, timeLeft / fadeDuration));

                if (timeLeft < -MinBlackScreenTime)
                {
                    if (!goalScene.Equals(QuitString))
                    {
                        SceneManager.LoadScene(goalScene);
                        operation.allowSceneActivation = true;
                        sceneLoadedTime = -1f;
                        leavingScene = false;
                    }
                    else
                    {
                        Application.Quit();
                    }
                }
            }
        }
        else
        {
            if (sceneLoadedTime < 0f)
            {
                sceneLoadedTime = getCurrentTime();
                operation = null;
            }


            float timeSinceLoaded = getCurrentTime() - sceneLoadedTime;
            float alpha = Mathf.Lerp(1f, 0f, timeSinceLoaded / fadeDuration);
            setBlockerAlpha(alpha);
            if (alpha <= 0f)
            {
                PauseManager.disablePause = false;
                gameObject.SetActive(false);
            }
        }
	}

    public void startShift(string goalScene, float shiftDuration = DefaultShiftDuration, float fadeDuration = DefaultFadeDuration)
    {
        if (operation != null)
            return;

        PauseManager.disablePause = true;
        gameObject.SetActive(true);
        this.shiftDuration = shiftDuration;
        this.fadeDuration = fadeDuration;
        this.goalScene = goalScene;
        leavingScene = true;
        shiftStartTime = getCurrentTime();
        setBlockerAlpha(0f);

        if (!goalScene.Equals(QuitString))
        {
            operation = SceneManager.LoadSceneAsync(goalScene);
            operation.allowSceneActivation = false;
        }
    }

    public void shiftToQuitGame(float shiftDuration = DefaultShiftDuration, float fadeDuration = DefaultFadeDuration)
    {
        startShift(QuitString, shiftDuration, fadeDuration);
    }

    public float getShiftDuration()
    {
        return shiftDuration;
    }

    public void setShiftDuration(float shiftDuration)
    {
        this.shiftDuration = shiftDuration;
    }

    public float getFadeDuration()
    {
        return fadeDuration;
    }

    public void setFadeDuration(float fadeDuration)
    {
        this.fadeDuration = fadeDuration;
    }

    float getCurrentTime()
    {
        return useRealTime ? Time.realtimeSinceStartup : Time.time;
    }

    void setBlockerAlpha(float alpha)
    {
        blocker.color = new Color(0f, 0f, 0f, alpha);
    }

    float getblockerAlpha()
    {
        return blocker.color.a;
    }
}
