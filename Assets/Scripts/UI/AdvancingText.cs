using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class AdvancingText : MonoBehaviour
{
    private const string richTextPrefix = "<color=#00000000>";
    private const string richTextSuffix = "</color>";

    [SerializeField]
    private float advanceSpeed;
    [SerializeField]
    private UnityEvent onComplete;

    private Text textComponent;
    private LocalizedText localizedText;
    private float progress;
    private string textString;
    
	void Start ()
    {
        textComponent = GetComponent<Text>();
        localizedText = GetComponent<LocalizedText>();
        resetAdvance();
	}

    public void resetAdvance()
    {
        if (localizedText != null)
            localizedText.enabled = false;
        textString = textComponent.text.Replace(richTextPrefix, "").Replace(richTextSuffix, "");
        progress = 0f;
        enabled = true;
        textComponent.text = getFittedText(textString, 0);

    }

    void Update ()
    {
        if (progress < textString.Length)
            updateText();
	}

    void updateText()
    {
        progress = Mathf.MoveTowards(progress, textString.Length, Time.deltaTime * advanceSpeed);
        textComponent.text = getFittedText(textString, (int)Mathf.Floor(progress));
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
}
