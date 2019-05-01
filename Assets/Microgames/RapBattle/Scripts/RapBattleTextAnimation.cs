using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

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
    
    private string verse;
    private string rhyme;
    private Color rhymeHighlightColor;

    public void setData(string verse, string rhyme, Color rhymeHighlightColor)
    {
        this.verse = verse;
        this.rhyme = rhyme;
        this.rhymeHighlightColor = rhymeHighlightColor;
    }

    void Start ()
    {
        advancingText = GetComponent<AdvancingText>();
        tmProComponent = GetComponent<TextMeshPro>();

        textInit();
        initialFontSize = tmProComponent.fontSize;
        fitToSize();
        tmProComponent.ForceMeshUpdate();

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
    
    // Performs some extra calculations to make sure the text can fit in the allotted size when expanding
    void fitToSize()
    {
        var rectTransform = GetComponent<RectTransform>();
        var holdText = tmProComponent.text;

        var maxHighlightSizeMult = highlightSizeCurve.keys.Max(a => a.value) * highlightSizeMult;
        int rhymeFontSize = (int)(initialFontSize * maxHighlightSizeMult);
        tmProComponent.text = $"<size={initialFontSize}>" + verse + $"<size={rhymeFontSize}>" + rhyme;
        tmProComponent.ForceMeshUpdate();

        var visibleTextSize = tmProComponent.GetRenderedValues(true);
        if (visibleTextSize.x > rectTransform.sizeDelta.x)
        {
            initialFontSize *= rectTransform.sizeDelta.x / visibleTextSize.x;
            initialFontSize = Mathf.Floor(initialFontSize);
        }

        tmProComponent.text = holdText;
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
                    processedText += $"<color=#{ColorUtility.ToHtmlStringRGBA(rhymeHighlightColor)}><size={fontSize}><i>";
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
