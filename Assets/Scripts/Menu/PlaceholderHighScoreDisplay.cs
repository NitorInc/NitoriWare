using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlaceholderHighScoreDisplay : MonoBehaviour
{

#pragma warning disable 0649
    [SerializeField]
    private Text scoreText;
#pragma warning restore 0649

	void Start()
	{
        scoreText.text = scoreText.text.Replace("000", PlayerPrefs.GetInt(GetComponent<SceneButton>().targetStage + "HighScore", 0).ToString("D3"));
	}
}
