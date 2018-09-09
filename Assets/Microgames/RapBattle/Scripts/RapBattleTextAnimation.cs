using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RapBattleTextAnimation : MonoBehaviour
{
    [SerializeField]
    private int highlightChar;
    [SerializeField]
    private string highlightColor;
    [SerializeField]
    private float highlightSizeMult = 1.5f;

    [SerializeField]
    private AnimationCurve sizeCurve;
    [SerializeField]
    private AnimationCurve highlightSizeCurve;
    [SerializeField]
    private float startTime;

    private AdvancingText advancingText;
    private TextMeshPro tmProComponent;

    private string parsedText;
    private float progress;
    private float highlightReachedTime;
    
	void Start ()
    {
        advancingText = GetComponent<AdvancingText>();
        tmProComponent = GetComponent<TextMeshPro>();

        updateParsedText();
        advancingText.enabled = false;
        tmProComponent.maxVisibleCharacters = 0;

        enabled = false;

        Invoke("enable", startTime);
    }

    void enable()
    {
        advancingText.enabled = true;
        advancingText.resetAdvance();
        enabled = true;
    }

    void updateParsedText()
    {
        tmProComponent.ForceMeshUpdate();
        parsedText = tmProComponent.GetParsedText();
    }
	
	void Update ()
    {
        float growCurveDuration = sizeCurve[sizeCurve.length - 1].time;
        progress = !advancingText.IsComplete ? advancingText.Progress
            : progress + (advancingText.getAdvanceSpeed() * Time.deltaTime);
        float fontSize = tmProComponent.fontSize;

        string processedText = "";
        for (int i = 0; i < parsedText.Length; i++)
        {
            if (i == highlightChar)
            {
                fontSize *= highlightSizeMult;
                processedText += $"<color={highlightColor}><size={fontSize}>";
            }
            var newChar = parsedText[i];
            if (i <= progress - 1) //Characer is visible
            {
                if (i < highlightChar)  //Stop adjusting size at highlight char
                {
                    float positionOnGrowCurve = (progress - 1) - i;
                    if (positionOnGrowCurve < growCurveDuration)
                    {
                        //Character i is visible
                        int charSize = (int)(sizeCurve.Evaluate(positionOnGrowCurve) * (float)fontSize);
                        processedText += $"<size={charSize.ToString()}>";
                    }
                }
                else if (i == highlightChar)
                {
                    if (!advancingText.IsComplete)  //Force text complete if we've just reached highlight char
                    {
                        highlightReachedTime = Time.time;
                        advancingText.Progress = advancingText.getTotalVisibleChars(false);
                    }
                    print(Time.time - highlightReachedTime);
                    print(highlightSizeCurve.Evaluate(Time.time - highlightReachedTime));
                    int highlightSize = (int)(highlightSizeCurve.Evaluate(Time.time - highlightReachedTime) * (float)fontSize);
                    processedText += $"<size={highlightSize.ToString()}>";
                }
            }
            processedText += newChar;
        }
        tmProComponent.text = processedText;
	}
}
