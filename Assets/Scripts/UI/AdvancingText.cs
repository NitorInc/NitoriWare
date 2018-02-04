using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class AdvancingText : MonoBehaviour
{
    
    [SerializeField]
    private float advanceSpeed;
    [SerializeField]
    private UnityEvent onComplete;

    private TMP_Text textMeshProComponent;
    private float progress;
    
	void Start ()
    {
        textMeshProComponent = GetComponent<TMP_Text>();;
        resetAdvance();
	}

    public void resetAdvance()
    {
        progress = 0f;
        setVisibleChars(0);
    }

    void Update ()
    {
        if (progress < getTotalVisibleChars())
            updateText();
	}

    void updateText()
    {
        progress = Mathf.MoveTowards(progress, getTotalVisibleChars(), Time.deltaTime * advanceSpeed);

        setVisibleChars((int)Mathf.Floor(progress));
        if (progress >= getTotalVisibleChars())
            onComplete.Invoke();
    }

    void setVisibleChars(int amount)
    {
        textMeshProComponent.maxVisibleCharacters = amount;
    }

    public int getVisibleChars()
    {
        return textMeshProComponent.maxVisibleCharacters;
    }

    public int getTotalVisibleChars()
    {
        var textInfo = textMeshProComponent.textInfo;
        return textInfo.characterCount; 
    }
}
