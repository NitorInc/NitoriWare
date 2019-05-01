using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RapBattleTextAnimation : MonoBehaviour
{
    [SerializeField]
    private float verseFillTime;
    [SerializeField]
    private float rhymeAppearTime;
    [SerializeField]
    private float highlightSizeMult = 1.5f;
    [SerializeField]
    private int rhymeSpaceCount = 3;

    [SerializeField]
    private AnimationCurve sizeCurve;
    [SerializeField]
    private AnimationCurve highlightSizeCurve;

    private AdvancingText advancingText;
    private TextMeshPro tmProComponent;
    
    private string parsedText;
    private float progress;
    private float highlightReachedTime;
    private int highlightChar;
    private float initialFontSize;
    private bool rhymeStarted;

    private RapBattleTimingController.Rap rapData;
    private string verse;
    private string rhyme;

    public void setRap(RapBattleTimingController.Rap rap)
    {
        rapData = rap;
        verse = rap.verse;
        rhyme = rap.rhyme;
    }

    void Start ()
    {
        advancingText = GetComponent<AdvancingText>();
        tmProComponent = GetComponent<TextMeshPro>();

        textInit();
        tmProComponent.ForceMeshUpdate();
        initialFontSize = tmProComponent.fontSize;

        updateParsedText();
        advancingText.enabled = false;
        tmProComponent.maxVisibleCharacters = 0;

        //enabled = false;
        //Invoke("enable", startBeat * StageController.beatLength);
        enable();
    }

    void textInit()
    {
        rhyme = rhyme.Trim();
        if (!string.IsNullOrEmpty(rhyme))
        {
            verse = verse.Trim();
            for (int i = 0; i < rhymeSpaceCount; i++)
            {
                verse += " ";
            }
            highlightChar = verse.Length;
        }
        else
        {
            verse = verse.Trim() + $"...";
            highlightChar = 999;
        }
        tmProComponent.text = verse + rhyme;
        advancingText.setAdvanceSpeed(((float)verse.Length / verseFillTime));
    }

    void enable()
    {
        advancingText.enabled = true;
        advancingText.resetAdvance();
        enabled = true;
        if (!string.IsNullOrEmpty(rhyme))
            Invoke("showRhyme", rhymeAppearTime);
    }

    void showRhyme()
    {
        advancingText.Progress = advancingText.getTotalVisibleChars(false);
        highlightReachedTime = Time.time;
        rhymeStarted = true;
    }

    void updateParsedText()
    {
        tmProComponent.ForceMeshUpdate();
        parsedText = tmProComponent.GetParsedText();
    }
	
	void LateUpdate ()
    {
        float growCurveDuration = sizeCurve[sizeCurve.length - 1].time;
        progress = !advancingText.IsComplete ? advancingText.Progress
            : progress + (advancingText.getAdvanceSpeed() * Time.deltaTime);
        float fontSize = initialFontSize;

        string processedText = "";
        processedText += $"<size={initialFontSize}>";
        for (int i = 0; i < parsedText.Length; i++)
        {
            if (i == highlightChar)
            {
                if (rhymeStarted)
                {
                    fontSize *= highlightSizeMult;
                    processedText += $"<color={rapData.highlightColor}><size={fontSize}><i>";
                }
                else
                    processedText += $"<alpha=#00>";
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
                    //print(Time.time - highlightReachedTime);
                    //print(highlightSizeCurve.Evaluate(Time.time - highlightReachedTime));
                    int highlightSize = (int)(highlightSizeCurve.Evaluate(Time.time - highlightReachedTime) * (float)fontSize);
                    processedText += $"<size={highlightSize.ToString()}>";
                }
            }
            processedText += newChar;
        }
        tmProComponent.text = processedText;
	}
}
