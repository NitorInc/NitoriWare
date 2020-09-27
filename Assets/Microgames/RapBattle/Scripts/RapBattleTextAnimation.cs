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
    private float rhymelessSizeMult = .66f;

    [SerializeField]
    private AnimationCurve sizeCurve;
    [SerializeField]
    private AnimationCurve highlightSizeCurve;

    private AdvancingText advancingText;
    private TextMeshPro tmProComponent;
    private RectTransform rectTransform;

    private string parsedText;
    private float progress;
    private float verseStartTime;
    private float highlightReachedTime;
    private int highlightChar;
    private float initialFontSize;
    private bool rhymeStarted;
    
    private string verse;
    private string rhyme;
    private Color rhymeHighlightColor;
    private bool sizeFit = false;

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
        rectTransform = GetComponent<RectTransform>();
        initialFontSize = tmProComponent.fontSize;

        textInit();
        setInitialTextString();
        tmProComponent.ForceMeshUpdate();

        updateParsedText();
        advancingText.enabled = false;
        tmProComponent.maxVisibleCharacters = 0;

        //enabled = false;
        //Invoke("enable", startBeat * Microgame.BeatLength);
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

            // Cut max size down in rhymeless verses to preserve gap for last rhyme
            rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x * rhymelessSizeMult, rectTransform.sizeDelta.y);
            tmProComponent.ForceMeshUpdate();
        }
        advancingText.setAdvanceSpeed(((float)verse.Length / verseFillTime));

    }

    // Initial text string has rhyme at max size to determine text fitting
    void setInitialTextString()
    {
        var maxHighlightSizeMult = highlightSizeCurve.keys.Max(a => a.value) * highlightSizeMult;
        int rhymeFontSize = (int)(initialFontSize * maxHighlightSizeMult);
        tmProComponent.text = $"<size={initialFontSize}>" + verse + $"<size={rhymeFontSize}>" + rhyme;
    }
    
    // Changes object scale make sure the text can fit in the allotted size when expanding
    // Needs to be run on first LateUpdate due to some complications I haven't quite figured out
    void fitToSize()
    {
        var textSize = tmProComponent.GetRenderedValues(false);
        if (textSize.x > rectTransform.sizeDelta.x)
        {
            transform.localScale *= rectTransform.sizeDelta.x / textSize.x;
        }
    }

    void enable()
    {
        advancingText.enabled = true;
        advancingText.resetAdvance();
        enabled = true;
        if (!string.IsNullOrEmpty(rhyme))
            Invoke("showRhyme", rhymeAppearTime);
        verseStartTime = Time.time;
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

    //private void Update()
    //{
    //    fitToSize();
    //}

    void LateUpdate ()
    {
        if (!sizeFit)
        {
            fitToSize();
            sizeFit = true;
        }

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
                    //float positionOnGrowCurve = (progress - 1) - i;
                    var charAppearedAt = verseStartTime + ((float)i / advancingText.getAdvanceSpeed());
                    var timeIntoChar = Time.time - charAppearedAt;
                    float positionOnGrowCurve = timeIntoChar;
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
