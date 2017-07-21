using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StageGameOverMenu : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField]
    private Text scoreNumberText, highScoreNumberText;
    [SerializeField]
    private GameObject highScoreIndicator;
    [SerializeField]
    private float fadeTime;
#pragma warning restore 0649

    public void setScore(int score)
	{
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

    public void initialize()
    {
        gameObject.SetActive(true);
        PauseManager.disablePause = true;
    }

    void setNumber(Text textComponent, int score)
    {
        textComponent.text = textComponent.text.Substring(0, textComponent.text.Length - 3);

        int number = score;
        textComponent.text += number.ToString("D3");
    }
}
