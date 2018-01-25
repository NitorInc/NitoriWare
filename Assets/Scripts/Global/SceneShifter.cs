using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneShifter : MonoBehaviour
{
  private const string QuitString = "QUITGAME";

  private const float DefaultShiftDuration = 1f, DefaultFadeDuration = .5f;
  private const float MinBlackScreenTime = .5f;

#pragma warning disable 0649
  [SerializeField]
  private Image blocker;
#pragma warning restore 0649

  private string goalScene;
  private float shiftDuration;
  private float fadeDuration;
  private bool leavingScene;
  private float shiftStartGameTime, shiftStartRealTime;
  private float sceneLoadedGameTime, sceneLoadedRealTime;

  private AsyncOperation operation;

  void Update()
  {
    if (leavingScene)
    {
      float timeLeft = getShiftStartTime() + shiftDuration - getCurrentTime();
      if (timeLeft < fadeDuration)
      {
        setBlockerAlpha(Mathf.Lerp(1f, 0f, timeLeft / fadeDuration));

        if (timeLeft < -MinBlackScreenTime)
        {
          if (!goalScene.Equals(QuitString))
          {
            SceneManager.LoadScene(goalScene);
            operation.allowSceneActivation = true;
            sceneLoadedGameTime = sceneLoadedRealTime = -1f;
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
      if (sceneLoadedGameTime < 0f)
      {
        sceneLoadedGameTime = Time.time;
        sceneLoadedRealTime = Time.realtimeSinceStartup;
        operation = null;
      }


      float timeSinceLoaded = getCurrentTime() - getSceneLoadedTime();
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
    shiftStartGameTime = Time.time;
    shiftStartRealTime = Time.realtimeSinceStartup;
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
    return Time.timeScale > 0f ? Time.time : Time.realtimeSinceStartup;
  }

  float getShiftStartTime()
  {
    return Time.timeScale > 0f ? shiftStartGameTime : shiftStartRealTime;
  }

  float getSceneLoadedTime()
  {
    return Time.timeScale > 0f ? sceneLoadedGameTime : sceneLoadedRealTime;
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
