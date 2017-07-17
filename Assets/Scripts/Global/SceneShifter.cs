using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneShifter : MonoBehaviour
{
    private const float DefaultShiftDuration = 1f, DefaultFadeDuration = .25f;

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
	
	void Update()
	{
        if (leavingScene)
        {
            float timeLeft = shiftStartTime + shiftDuration - getCurrentTime();
            if (timeLeft < fadeDuration)
            {
                if (getblockerAlpha() >= 1f)
                {
                    SceneManager.LoadScene(goalScene);
                    sceneLoadedTime = -1f;
                    leavingScene = false;
                }
                else
                {
                    setBlockerAlpha(Mathf.Lerp(1f, 0f, timeLeft / fadeDuration));
                }
            }
        }
        else
        {
            if (sceneLoadedTime < 0f)
            {

                sceneLoadedTime = getCurrentTime();
                //fadeDuration = .5f;
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
        PauseManager.disablePause = true;
        gameObject.SetActive(true);
        this.shiftDuration = shiftDuration;
        this.fadeDuration = fadeDuration;
        this.goalScene = goalScene;
        leavingScene = true;
        shiftStartTime = getCurrentTime();
        setBlockerAlpha(0f);
    }

    //public float getShiftDuration()
    //{
    //    return shiftDuration;
    //}

    //private void setShiftDuration(float shiftDuration)
    //{
    //    this.shiftDuration = shiftDuration;
    //}

    //public float getFadeDuration()
    //{
    //    return fadeDuration;
    //}

    //private void setFadeDuration(float fadeDuration)
    //{
    //    this.fadeDuration = fadeDuration;
    //}

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
