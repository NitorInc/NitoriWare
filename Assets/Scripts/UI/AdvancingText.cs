using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class AdvancingText : MonoBehaviour
{
    private const string richTextPrefix = "<color=#00000000>";
    private const string richTextSuffix = "</color>";
    
    [SerializeField]
    private float advanceSpeed;
    [SerializeField]
    private UnityEvent onComplete;

    private TextMeshProUGUI textMeshPro;
    private float progress;
    private string textString;
    
	void Start ()
    {
        textMeshPro = GetComponent<TextMeshProUGUI>();
        progress = -1f;
        resetAdvance();
	}

    public void resetAdvance()
    {
        textString = getUnformattedText();
        progress = 0f;
        enabled = true;
        textMeshPro.text = getFittedText(textString, 0);

    }

    void Update ()
    {
        if (progress < 0f)
            resetAdvance();
        if (progress < textString.Length)
            updateText();
	}

    void updateText()
    {
        textString = getUnformattedText();
        progress = Mathf.MoveTowards(progress, textString.Length, Time.deltaTime * advanceSpeed);
        textMeshPro.text = getFittedText(textString, (int)Mathf.Floor(progress));
        if (progress >= textString.Length)
            onComplete.Invoke();
    }

    public string getFittedText(string textString, int visibleChars)
    {
        if (visibleChars >= textString.Length)
            return textString;

        return textString.Substring(0, visibleChars) + richTextPrefix
            + textString.Substring(visibleChars, textString.Length - visibleChars) + richTextSuffix;
    }

    public string getUnformattedText()
    {
        return textMeshPro.text.Replace(richTextPrefix, "").Replace(richTextSuffix, "");
    }
}
